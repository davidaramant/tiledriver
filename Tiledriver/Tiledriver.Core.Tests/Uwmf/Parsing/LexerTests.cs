// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.Uwmf.Parsing;

namespace Tiledriver.Core.Tests.Uwmf.Parsing
{
    [TestFixture]
    public sealed class LexerTests
    {
        [TestCase("someProperty = 10;")]
        [TestCase("      someProperty=10;")]
        [TestCase("      someProperty  = 10 ;")]
        public void ShouldIgnoreWhitespace(string input)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Integer(10),
                Token.Semicolon);
        }

        [TestCase("someProperty = 10;")]
        [TestCase("someProperty = 0010;")]
        [TestCase("someProperty = 0xa;")]
        [TestCase("someProperty = +10;")]
        public void ShouldLexIntegers(string input)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Integer(10),
                Token.Semicolon);
        }

        [TestCase("someProperty = 10.5;")]
        [TestCase("someProperty = +10.5;")]
        [TestCase("someProperty = 1.05e1;")]
        public void ShouldLexDoubles(string input)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.Double(10.5),
                Token.Semicolon);
        }

        [TestCase("someProperty = true;", true)]
        [TestCase("someProperty = false;", false)]
        public void ShouldLexBooleans(string input, bool boolValue)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                boolValue ? Token.BooleanTrue : Token.BooleanFalse,
                Token.Semicolon);
        }

        [TestCase("someProperty = \"true\";", "true")]
        [TestCase("someProperty = \"0xFB010304\";", "0xFB010304")]
        [TestCase("someProperty = \"\";", "")]
        public void ShouldLexStrings(string input, string stringValue)
        {
            VerifyLexing(input,
                Token.Identifier("someProperty"),
                Token.Equal,
                Token.String(stringValue),
                Token.Semicolon);
        }

        [Test]
        public void ShouldLexEmptyBlock()
        {
            VerifyLexing("block { }",
                Token.Identifier("block"),
                Token.OpenParen,
                Token.CloseParen);
        }

        [Test]
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

        [Test]
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

        private static void VerifyLexing(string input, params Token[] expectedTokens)
        {
            var actualTokens = UwmfLexer.BuildLexer().Tokenize(input).Select(t => t.Item2).ToArray();

            Assert.That(actualTokens, Is.EqualTo(expectedTokens), $"Did not correct tokenize {input}");
        }
    }
}