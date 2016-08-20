// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using NUnit.Framework;
using Tiledriver.Core.Uwmf;
using Tiledriver.Core.Uwmf.Parsing;

namespace Tiledriver.Core.Tests.Uwmf.Parsing
{
    [TestFixture]
    public sealed class ParserTests
    {
        [Test]
        public void ShouldRoundTripMinimalMap()
        {
            var map = new Map
            {
                Namespace = "1",
                Width = 2,
                Height = 3,
                TileSize = 4,
                Name = "5",
            };

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                var roundTripped = Parser.Parse(new LexerOld(new UwmfCharReader(stream)));

                UwmfComparison.AssertEqual(roundTripped,map);
            }
        }

        [Test]
        public void ShouldRoundTripDemoMap()
        {
            var map = DemoMap.Create();

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                var roundTripped = Parser.Parse(new LexerOld(new UwmfCharReader(stream)));

                UwmfComparison.AssertEqual(roundTripped, map);
            }
        }

        [Test]
        public void ShouldParseOldDemoMap()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Uwmf", "Parsing", "TEXTMAP.txt")))
            {
                var map = Parser.Parse(new LexerOld(new UwmfCharReader(stream)));
            }
        }
    }
}