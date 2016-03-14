// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using NUnit.Framework;
using Tiledriver.Core.Uwmf.Parsing;

namespace Tiledriver.Core.Tests.Uwmf.Parsing
{
    [TestFixture]
    public sealed class LexerTests
    {
        [TestCase("someproperty = 10;")]
        [TestCase("      someProperty=10;")]
        [TestCase("      SOMEPROPERTY=10;")]
        [TestCase("/* comment */ someProperty = 10;")]
        [TestCase("// comment\nsomeProperty = 10;")]
        public void ShouldReadIdentifier(string input)
        {
            var expectedIdentifier = new Identifier("someproperty");

            var lexer = new Lexer(new TestStringReader(input));

            Assert.That(
                lexer.ReadIdentifier(),
                Is.EqualTo(expectedIdentifier),
                "Did not read identifier.");
        }

        [Test]
        public void ShouldDetermineIfAssignmentOrStartOfBlock(
            [Values("", "   ", "//comment\n", "// comment\n\t", "/* comment */", "/* comment */   ", "/*\ncommentn\n*/\n  ")] string prefix,
            [Values(ExpressionType.Assignment, ExpressionType.StartBlock)] ExpressionType expectedType)
        {
            var input = prefix + (expectedType == ExpressionType.Assignment ? "=" : "{");

            var lexer = new Lexer(new TestStringReader(input));

            Assert.That(
                lexer.DetermineIfAssignmentOrStartBlock(),
                Is.EqualTo(expectedType),
                "Did not determine expression type correctly.");
        }

        [TestCase("identifier",ExpressionType.Identifier)]
        [TestCase("}",ExpressionType.EndBlock)]
        public void ShouldDetermineIfIdentifierOrEndBlock(string input, ExpressionType expectedType)
        {
            var lexer = new Lexer(new TestStringReader(input));

            Assert.That(
                lexer.DetermineIfIdentifierOrEndBlock(),
                Is.EqualTo(expectedType),
                "Did not determine if the next thing was an identifier or the end of a block.");
        }

        [TestCase("64;", 64)]
        [TestCase("8;", 8)]
        [TestCase("08;", 8)]
        [TestCase("+16;", 16)]
        [TestCase("-16;", -16)]
        [TestCase("0xFf;", 255)]
        public void ShouldReadIntegerAssignment( string input, int expectedResult )
        {
            var lexer = new Lexer(new TestStringReader(input));

            Assert.That(
                lexer.ReadIntegerAssignment(),
                Is.EqualTo(expectedResult),
                "Did not read integer correctly.");
        }

        [TestCase("64;", 64d)]
        [TestCase("6.4;", 6.4d)]
        [TestCase("+1.6;", 1.6d)]
        [TestCase("-1.6;", -1.6d)]
        [TestCase("1e+9;", 1e+9d)]
        public void ShouldReadFloatingPointAssignment(string input, double expectedResult)
        {
            var lexer = new Lexer(new TestStringReader(input));

            Assert.That(
                lexer.ReadFloatingPointAssignment(),
                Is.EqualTo(expectedResult),
                "Did not read floating point number correctly.");
        }

        [TestCase("true;", true)]
        [TestCase("false;", false)]
        public void ShouldReadBooleanAssignment(string input, bool expectedResult)
        {
            var lexer = new Lexer(new TestStringReader(input));

            Assert.That(
                lexer.ReadBooleanAssignment(),
                Is.EqualTo(expectedResult),
                "Did not read Boolean correctly.");
        }

        [TestCase("\"Test String\";", "Test String")]
        [TestCase("\"0xFB010304\";", "0xFB010304")]
        [TestCase("\"\";", "")]
        public void ShouldReadStringAssignment(string input, string expectedResult)
        {
            var lexer = new Lexer(new TestStringReader(input));

            Assert.That(
                lexer.ReadStringAssignment(),
                Is.EqualTo(expectedResult),
                "Did not read string correctly.");
        }

        [Test]
        public void ShouldVerifyStartOfBlock()
        {
            var lexer = new Lexer(new TestStringReader("{"));

            Assert.DoesNotThrow(
                lexer.VerifyStartOfBlock,
                "Should have verified a start of block.");
        }

        [TestCase("64;")]
        [TestCase("true;")]
        [TestCase("6.4;")]
        [TestCase("\"string\";")]
        public void ShouldMovePastAssignment(string input)
        {
            var lexer = new Lexer(new TestStringReader(input));

            Assert.DoesNotThrow(
                lexer.MovePastAssignment,
                "Should not have thrown moving past assignment,");
        }
    }
}