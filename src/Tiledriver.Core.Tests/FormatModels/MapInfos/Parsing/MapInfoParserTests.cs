// Copyright (c) 2017, David Aramant
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
            using (var stream = TestFile.MapInfo.wolfcommon)
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
                .Returns(TestFile.MapInfo.wolfcommon);

            using (var stream = TestFile.MapInfo.wolf3d)
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
                .Returns(TestFile.MapInfo.wolfcommon);

            using (var stream = TestFile.MapInfo.spear)
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(mockProvider.Object);
                var elements = lexer.Analyze(textReader).ToArray();
                var results = MapInfoParser.Parse(elements);
            }
        }
    }
}