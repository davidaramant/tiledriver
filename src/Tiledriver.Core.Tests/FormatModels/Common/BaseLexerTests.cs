// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Xunit;
using FluentAssertions;
using Piglet.Lexer;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.Tests.FormatModels.Common
{
    public abstract class BaseLexerTests
    {
        [Theory]
        [InlineData("someProperty = 10;")]
        [InlineData("      someProperty=10;")]
        [InlineData("      someProperty  = 10 ;")]
        public void ShouldIgnoreWhitespace(string input)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Integer(10),
                Token.Semicolon);
        }

        [Theory]
        [InlineData("someProperty = 0;", 0)]
        [InlineData("someProperty = 10;", 10)]
        [InlineData("someProperty = 0010;", 10)]
        [InlineData("someProperty = 0xa;", 10)]
        [InlineData("someProperty = +10;", 10)]
        [InlineData("someProperty = -10;", -10)]
        public void ShouldLexIntegers(string input, int value)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Integer(value),
                Token.Semicolon);
        }

        [Theory]
        [InlineData("someProperty = 10.5;", 10.5)]
        [InlineData("someProperty = +10.5;", 10.5)]
        [InlineData("someProperty = 1.05e1;", 10.5)]
        [InlineData("someProperty = 1e10;", 1e10)]
        public void ShouldLexDoubles(string input, double value)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Double(value),
                Token.Semicolon);
        }

        [Theory]
        [InlineData("someProperty = true;", true)]
        [InlineData("someProperty = false;", false)]
        public void ShouldLexBooleans(string input, bool boolValue)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                boolValue ? Token.BooleanTrue : Token.BooleanFalse,
                Token.Semicolon);
        }

        [Theory]
        [InlineData("someProperty = \"true\";", "true")]
        [InlineData("someProperty = \"0xFB010304\";", "0xFB010304")]
        [InlineData("someProperty = \"\";", "")]
        public void ShouldLexStrings(string input, string stringValue)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.String(stringValue),
                Token.Semicolon);
        }

        [Fact]
        public void ShouldLexEmptyBlock()
        {
            VerifyLexing("block { }",
                Token.Identifier("block"),
                Token.OpenParen,
                Token.CloseParen);
        }

        [Fact]
        public void ShouldLexBlockWithAssignments()
        {
            VerifyLexing("block { id1 = 1; id2 = false; }",
                Token.Identifier("block"),
                Token.OpenParen,
                Token.Identifier("id1"),
                Token.Equal,
                Token.Integer(1),
                Token.Semicolon,
                Token.Identifier("id2"),
                Token.Equal,
                Token.BooleanFalse,
                Token.Semicolon,
                Token.CloseParen);
        }

        [Fact]
        public void ShouldLexBlockWithArrays()
        {
            VerifyLexing("block { {1,2},{3,4} }",
                Token.Identifier("block"),
                Token.OpenParen,

                Token.OpenParen,
                Token.Integer(1),
                Token.Comma,
                Token.Integer(2),
                Token.CloseParen,

                Token.Comma,

                Token.OpenParen,
                Token.Integer(3),
                Token.Comma,
                Token.Integer(4),
                Token.CloseParen,

                Token.CloseParen);
        }

        [InlineData("// Comment\r\nsomeProperty = 10;")]
        [InlineData("// Comment\nsomeProperty = 10;")]
        [InlineData("someProperty = 10; // Comment")]
        public void ShouldIgnoreComments(string input)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Integer(10),
                Token.Semicolon);
        }

        protected void VerifyLexing(string input, params Token[] expectedTokens)
        {
            var actualTokens = GetDefinition().Tokenize(input).Select(t => t.Item2).ToArray();

            actualTokens.Should().BeEquivalentTo(expectedTokens, $"{input} should have been tokenized");
        }

        protected abstract ILexer<Token> GetDefinition();
    }
}