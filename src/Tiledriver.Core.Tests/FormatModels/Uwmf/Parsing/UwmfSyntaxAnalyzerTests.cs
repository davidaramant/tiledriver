// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Parsing
{
    public sealed class UwmfSyntaxAnalyzerTests
    {
        [Fact]
        public void ShouldParseGlobalAssignment()
        {
            var input = @"prop = 1;";

            var syntaxAnalzer = new UwmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UwmfLexer(new StringReader(input)));

            result.GetGlobalAssignments().Should()
                .BeEquivalentTo(new[] { new Assignment(new Identifier("prop"), Token.Integer(1)) });
        }

        [Fact]
        public void ShouldParseMultipleGlobalAssignments()
        {
            var input = @"prop = 1;prop2 = ""string"";";

            var syntaxAnalzer = new UwmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UwmfLexer(new StringReader(input)));


            result.GetGlobalAssignments().Should().BeEquivalentTo(new[]
            {
                new Assignment(new Identifier("prop"), Token.Integer(1)),
                new Assignment(new Identifier("prop2"), Token.String("string"))
            });
        }

        [Fact]
        public void ShouldParseEmptyBlock()
        {
            var input = @"block {}";

            var syntaxAnalzer = new UwmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UwmfLexer(new StringReader(input)));

            result.Blocks.Should().HaveCount(1, "a block should have been parsed out.");

            var block = result.Blocks.First();

            block.Name.Should().Be(new Identifier("block"));
        }

        [Fact]
        public void ShouldParseFullBlock()
        {
            var input = @"block {prop = 1;prop2 = ""string"";}";

            var syntaxAnalzer = new UwmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UwmfLexer(new StringReader(input)));

            result.Blocks.Should().HaveCount(1, "a block should have been parsed out.");

            var block = result.Blocks.First();

            block.Name.Should().Be(new Identifier("block"));
            block.Should().BeEquivalentTo(new[]
            {
                new Assignment(new Identifier("prop"), Token.Integer(1)),
                new Assignment(new Identifier("prop2"), Token.String("string"))
            });
        }

        [Fact]
        public void ShouldParseFullBlockMixedWithGlobalAssignments()
        {
            var input = @"gProp =1;block {prop = 1;prop2 = ""string"";}gProp2 = 5;";

            var syntaxAnalzer = new UwmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UwmfLexer(new StringReader(input)));

            result.Blocks.Should().HaveCount(1, "a block should have been parsed out.");

            var block = result.Blocks.First();

            block.Name.Should().Be(new Identifier("block"));
            block.Should().BeEquivalentTo(new[]
                {
                    new Assignment(new Identifier("prop"), Token.Integer(1)),
                    new Assignment(new Identifier("prop2"), Token.String("string"))
                });

            result.GetGlobalAssignments().Should().BeEquivalentTo(new[]
                {
                    new Assignment(new Identifier("gProp"), Token.Integer(1)),
                    new Assignment(new Identifier("gProp2"), Token.Integer(5))
                });
        }

        [Fact]
        public void ShouldParseArrayBlockWithOneTuple()
        {
            var input = @"block {{1,2,3}}";

            var syntaxAnalzer = new UwmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UwmfLexer(new StringReader(input)));

            result.ArrayBlocks.Should().HaveCount(1, "an array block should have been parsed out.");

            var block = result.ArrayBlocks.First();

            block.Name.Should().Be(new Identifier("block"));
            block.First().Should().BeEquivalentTo(new[] { 1, 2, 3 });
        }

        [Fact]
        public void ShouldParseArrayBlockWitMultipleTuples()
        {
            var input = @"block {{1,2,3},{4,5,6},{7,8,9,10}}";

            var syntaxAnalzer = new UwmfSyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new UwmfLexer(new StringReader(input)));

            result.ArrayBlocks.Should().HaveCount(1, "an array block should have been parsed out.");

            var block = result.ArrayBlocks.First();

            block.Name.Should().Be(new Identifier("block"));
            block[0].Should().BeEquivalentTo(new[] { 1, 2, 3 });
            block[1].Should().BeEquivalentTo(new[] { 4, 5, 6 });
            block[2].Should().BeEquivalentTo(new[] { 7, 8, 9, 10 });
        }
    }
}