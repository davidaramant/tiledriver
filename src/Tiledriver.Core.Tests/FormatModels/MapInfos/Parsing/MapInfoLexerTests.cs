// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.MapInfos.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.MapInfos.Parsing
{
    public sealed class MapInfoLexerTests
    {
        [Fact]
        public void ShouldHandlePropertyWithValue()
        {
            VerifyLexing("a = 3",
                new MapInfoProperty(new Identifier("a"), new[] { "3" }));
        }

        [Fact]
        public void ShouldHandlePropertyWithoutValue()
        {
            VerifyLexing("a",
                new MapInfoProperty(new Identifier("a")));
        }

        [Fact]
        public void ShouldHandleMultiplePropertiesWithoutValues()
        {
            VerifyLexing("a\nb",
                new MapInfoProperty(new Identifier("a")),
                new MapInfoProperty(new Identifier("b")));
        }

        [Fact]
        public void ShouldHandlePropertyWithValues()
        {
            VerifyLexing("a = 1, 2, 3",
                new MapInfoProperty(new Identifier("a"), new[] { "1", "2", "3" }));
        }

        [InlineData("NoWhiteSpace")]
        [InlineData("White Space")]
        [InlineData("With,Comma")]
        public void ShouldHandlePropertyWithStringValues(string s)
        {
            VerifyLexing($"a = \"{s}\"",
                new MapInfoProperty(new Identifier("a"), new[] { $"\"{s}\"" }));
        }

        #region Paramter parsing

        [InlineData("\"\"\"", "\"\"\"")]
        [InlineData("1", "1")]
        [InlineData("\"string\"", "\"string\"")]
        [InlineData("\"string with whitespace\"", "\"string with whitespace\"")]
        [InlineData("\"string,with,commas\"", "\"string,with,commas\"")]
        [InlineData("   \"string needing trimming\"     ", "\"string needing trimming\"")]
        public void ShouldParseSingleParameters(string input, string expected)
        {
            AssertParsing(input, expected);
        }

        [Fact]
        public void ShouldParseMultipleParameters()
        {
            AssertParsing(
                "   1   ,  \"a string\",  \"comma,string\"  ",
                "1",
                "\"a string\"",
                "\"comma,string\"");
        }

        private static void AssertParsing(string input, params string[] expectedResults)
        {
            var actualResults = MapInfoLexer.ParseCommaSeparatedParameters(input);
            actualResults.Should().BeEquivalentTo(expectedResults, "parameters should have been parsed correctly.");
        }

        #endregion

        [Fact]
        public void ShouldHandleBlock()
        {
            VerifyLexing(
                "block 1 2 \"3\"\n" +
                "{\n" +
                    "a = \"dog\"\n" +
                    "b = 4\n" +
                "}",
                new MapInfoBlock(
                    new Identifier("block"),
                    new[] { "1", "2", "\"3\"" },
                    new[]
                    {
                        new MapInfoProperty(new Identifier("a"), new [] { "\"dog\""}),
                        new MapInfoProperty(new Identifier("b"), new [] {"4"}),
                    }));
        }

        [Fact]
        public void ShouldHandleNestedBlocks()
        {
            VerifyLexing(
                "block 1 2 \"3\"\n" +
                "{\n" +
                    "nested\n" +
                    "{\n" +
                        "a = \"dog\"\n" +
                    "}\n" +
                "}",
                new MapInfoBlock(
                    new Identifier("block"),
                    new[] { "1", "2", "\"3\"" },
                    new[]
                    {
                        new MapInfoBlock(new Identifier("nested"),Enumerable.Empty<string>(),new[]
                        {
                            new MapInfoProperty(new Identifier("a"), new [] {"\"dog\""}),
                        }),

                    }));
        }

        [Fact]
        public void ShouldHandleBlockContainingPropertyWithNoAssignment()
        {
            VerifyLexing(
                "block\n" +
                "{\n" +
                    "a\n" +
                "}",
                new MapInfoBlock(
                    new Identifier("block"),
                    Enumerable.Empty<string>(),
                    new[]
                    {
                        new MapInfoProperty(new Identifier("a")),
                    }));
        }

        [Fact]
        public void ShouldHandleEmptyBlock()
        {
            VerifyLexing(
                "block {}",
                new MapInfoBlock(
                    new Identifier("block"),
                    Enumerable.Empty<string>(),
                    Enumerable.Empty<IMapInfoElement>()));
        }

        [Fact]
        public void ShouldTokenizeMapInfoWithoutInclude()
        {
            using (var stream = TestFile.MapInfo.wolfcommon)
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(Mock.Of<IResourceProvider>());
                var result = lexer.Analyze(textReader).ToArray();
            }
        }

        [Fact]
        public void ShouldTokenizeMapInfoWithInclude()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider.Setup(_ => _.Lookup("mapinfo/wolfcommon.txt"))
                .Returns(TestFile.MapInfo.wolfcommon);

            using (var stream = TestFile.MapInfo.wolf3d)
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(mockProvider.Object);
                var result = lexer.Analyze(textReader).ToArray();
            }
        }

        [Fact]
        public void ShouldTokenizeSpearMapInfo()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider.Setup(_ => _.Lookup("mapinfo/wolfcommon.txt"))
                .Returns(TestFile.MapInfo.wolfcommon);

            using (var stream = TestFile.MapInfo.spear)
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(mockProvider.Object);
                var result = lexer.Analyze(textReader).ToArray();
            }
        }


        private static void VerifyLexing(string input, params IMapInfoElement[] expectedElements)
        {
            var lexer = new MapInfoLexer(Mock.Of<IResourceProvider>());

            var parsed = lexer.Analyze(new StringReader(input)).ToArray();

            VerifyElementsAreIdentical(
                actual: parsed,
                expected: expectedElements);

        }

        private static void VerifyElementsAreIdentical(
            IEnumerable<IMapInfoElement> actual,
            IEnumerable<IMapInfoElement> expected)
        {
            actual.Should().HaveSameCount(expected, "correct number of elements should have been lexed");

            foreach (var pair in actual.Zip(expected, (a, e) => new { Actual = a, Expected = e }))
            {
                pair.Actual.IsBlock.Should()
                    .Be(pair.Expected.IsBlock, "correct type of element should have been lexed.");

                if (pair.Actual.IsBlock)
                {
                    VerifyBlocksAreIdentical(
                        actual: pair.Actual.AsBlock(),
                        expected: pair.Expected.AsBlock());
                }
                else
                {
                    VerifyPropertiesAreIdentical(
                        actual: pair.Actual.AsProperty(),
                        expected: pair.Expected.AsProperty());
                }
            }
        }

        private static void VerifyPropertiesAreIdentical(MapInfoProperty actual, MapInfoProperty expected)
        {
            actual.Name.Should().Be(expected.Name, "property name should match.");
            actual.Values.Should().BeEquivalentTo(expected.Values, "property values should match.");
        }

        private static void VerifyBlocksAreIdentical(MapInfoBlock actual, MapInfoBlock expected)
        {
            actual.Name.Should().Be(expected.Name, "block name should match.");
            actual.Metadata.Should().BeEquivalentTo(expected.Metadata, "block metadata should match.");
        }
    }
}