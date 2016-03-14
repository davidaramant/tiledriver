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
    }
}