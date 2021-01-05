﻿// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.MapInfos.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.MapInfos.Parsing
{
    [TestFixture]
    public sealed class MapInfoParserTests
    {
        [Test]
        public void ShouldParseMapInfoWithoutInclude()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "MapInfos", "Parsing", "wolfcommon.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(Mock.Of<IResourceProvider>());
                var elements = lexer.Analyze(textReader).ToArray();
                var results = MapInfoParser.Parse(elements);
            }
        }

        [Test]
        public void ShouldParseMapInfoWithInclude()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider.Setup(_ => _.Lookup("mapinfo/wolfcommon.txt"))
                .Returns(File.OpenRead(
                        Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "MapInfos",
                        "Parsing", "wolfcommon.txt")));

            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "MapInfos", "Parsing", "wolf3d.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(mockProvider.Object);
                var elements = lexer.Analyze(textReader).ToArray();
                var results = MapInfoParser.Parse(elements);
            }
        }

        [Test]
        public void ShouldParseSpearMapInfo()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider.Setup(_ => _.Lookup("mapinfo/wolfcommon.txt"))
                .Returns(File.OpenRead(
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "MapInfos",
                        "Parsing", "wolfcommon.txt")));

            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "MapInfos", "Parsing", "spear.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(mockProvider.Object);
                var elements = lexer.Analyze(textReader).ToArray();
                var results = MapInfoParser.Parse(elements);
            }
        }
    }
}