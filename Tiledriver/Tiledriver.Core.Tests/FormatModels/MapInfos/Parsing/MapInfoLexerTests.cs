// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.MapInfos.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.MapInfos.Parsing
{
    [TestFixture]
    public sealed class MapInfoLexerTests
    {
        [Test]
        public void ShouldHandlePropertyWithValue()
        {
            VerifyLexing("a = 3",
                new MapInfoProperty(new Identifier("a"), new[] { "3" }));
        }

        [Test]
        public void ShouldHandlePropertyWithoutValue()
        {
            VerifyLexing("a",
                new MapInfoProperty(new Identifier("a")));
        }

        [Test]
        public void ShouldHandleMultiplePropertiesWithoutValues()
        {
            VerifyLexing("a\nb",
                new MapInfoProperty(new Identifier("a")),
                new MapInfoProperty(new Identifier("b")));
        }

        [Test]
        public void ShouldHandlePropertyWithValues()
        {
            VerifyLexing("a = 1, 2, 3",
                new MapInfoProperty(new Identifier("a"), new[] { "1", "2", "3" }));
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void ShouldTokenizeMapInfoWithoutInclude()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "MapInfos", "Parsing", "wolfcommon.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(Mock.Of<IResourceProvider>());
                var result = lexer.Analyze(textReader).ToArray();
            }
        }

        [Test]
        public void ShouldTokenizeMapInfoWithInclude()
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
            Assert.That(actual.Count(), Is.EqualTo(expected.Count()), "Did not lex correct number of elements.");

            foreach (var pair in actual.Zip(expected, (a, e) => new { Actual = a, Expected = e }))
            {
                Assert.That(pair.Actual.IsBlock, Is.EqualTo(pair.Actual.IsBlock), "Lexed wrong type of element.");

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
            Assert.That(actual.Name, Is.EqualTo(expected.Name), "Property name did not match.");
            Assert.That(actual.Values, Is.EquivalentTo(expected.Values), "Property values did not match.");
        }

        private static void VerifyBlocksAreIdentical(MapInfoBlock actual, MapInfoBlock expected)
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name), "Block name did not match.");
            Assert.That(actual.Metadata, Is.EquivalentTo(expected.Metadata), "Block metadata did not match.");


        }
    }
}