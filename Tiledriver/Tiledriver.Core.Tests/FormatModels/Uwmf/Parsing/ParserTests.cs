// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Core.FormatModels.Uwmf.Parsing.Syntax;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Parsing
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

                using (var textReader = new StreamReader(stream))
                {
                    var sa = new SyntaxAnalyzer();
                    var roundTripped = Parser.Parse(sa.Analyze(new UwmfLexer(textReader)));
                    UwmfComparison.AssertEqual(roundTripped, map);
                }
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

                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var sa = new SyntaxAnalyzer();
                    var roundTripped = Parser.Parse(sa.Analyze(new UwmfLexer(textReader)));

                    UwmfComparison.AssertEqual(roundTripped, map);
                }
            }
        }

        [Test]
        public void ShouldParseOldDemoMap()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "Uwmf", "Parsing", "TEXTMAP.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var sa = new SyntaxAnalyzer();
                var map = Parser.Parse(sa.Analyze(new UwmfLexer(textReader)));
            }
        }
    }
}