// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.MapInfos.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.MapInfos.Parsing
{
    [TestFixture]
    public sealed class MapInfoTextReaderTests
    {
        [Test]
        public void ShouldPeekTheNextLine()
        {
            using (var reader = new StringReader("  line 1  \n line2 "))
            {
                var miReader = new MapInfoTextReader(reader);

                Assert.That(miReader.PeekLine(),Is.EqualTo("line 1"), "Did not peek the first line.");
                Assert.That(miReader.ReadLine(), Is.EqualTo("line 1"), "Did not return the first line for real.");
                Assert.That(miReader.PeekLine(), Is.EqualTo("line2"), "Did not peek the second line.");
            }
        }

        [Test]
        public void ShouldIgnoreEmptyLines()
        {
            using (var reader = new StringReader("\nLine 1\n\nLine 2\n"))
            {
                var miReader = new MapInfoTextReader(reader);

                Assert.That(
                    ReadAllLines(miReader),
                    Is.EquivalentTo(new[]{"Line 1","Line 2"}),
                    "Did not return the correct lines.");
            }
        }

        [Test]
        public void ShouldIgnoreComments()
        {
            using (var reader = new StringReader("Line 1\n//Comment\nLine 2\n"))
            {
                var miReader = new MapInfoTextReader(reader);

                Assert.That(
                    ReadAllLines(miReader),
                    Is.EquivalentTo(new[] { "Line 1", "Line 2" }),
                    "Did not return the correct lines.");
            }
        }

        private static List<string> ReadAllLines(MapInfoTextReader reader)
        {
            var lines = new List<string>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            return lines;
        }
    }
}