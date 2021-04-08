// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels.MapInfos.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.MapInfos.Parsing
{
    public sealed class MapInfoTextReaderTests
    {
        [Fact]
        public void ShouldPeekTheNextLine()
        {
            using (var reader = new StringReader("  line 1  \n line2 "))
            {
                var miReader = new MapInfoTextReader(reader);

                miReader.PeekLine().Should().Be("line 1", "first line should be peeked");
                miReader.ReadLine().Should().Be("line 1", "first line should be read for real");
                miReader.PeekLine().Should().Be("line2", "second line should be peeked");
            }
        }

        [Fact]
        public void ShouldIgnoreEmptyLines()
        {
            using (var reader = new StringReader("\nLine 1\n\nLine 2\n"))
            {
                var miReader = new MapInfoTextReader(reader);

                ReadAllLines(miReader).Should()
                    .BeEquivalentTo(new[] {"Line 1", "Line 2"}, "correct lines should be returned");
            }
        }

        [Fact]
        public void ShouldIgnoreComments()
        {
            using (var reader = new StringReader("Line 1\n//Comment\nLine 2\n"))
            {
                var miReader = new MapInfoTextReader(reader);

                ReadAllLines(miReader).Should()
                    .BeEquivalentTo(new[] {"Line 1", "Line 2"}, "correct lines should be returned");
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