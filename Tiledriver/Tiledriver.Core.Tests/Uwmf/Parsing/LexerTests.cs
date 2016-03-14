// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;
using System.Text;
using NUnit.Framework;
using Tiledriver.Core.Uwmf.Parsing;

namespace Tiledriver.Core.Tests.Uwmf.Parsing
{
    [TestFixture]
    public sealed class LexerTests
    {
        [TestCase("someproperty = 10;", 1, 13)]
        [TestCase("      someProperty=10;", 1, 19)]
        [TestCase("      SOMEPROPERTY=10;", 1, 19)]
        [TestCase("/* comment */ someProperty = 10;", 1, 27)]
        [TestCase("// comment\nsomeProperty = 10;", 2, 13)]
        public void ShouldReadIdentifier(string input, int expectedEndingLine, int expectedEndingColumn)
        {
            var expectedIdentifier = new Identifier("someproperty");

            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.That(
                lexer.ReadIdentifier(),
                Is.EqualTo(expectedIdentifier),
                "Did not read identifier.");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        // TODO: This is stupid.  I don't think comments can really appear in all these places anyway.
        [TestCase("= ", ExpressionType.Assignment, 1, 2)]
        [TestCase(" = ", ExpressionType.Assignment, 1, 3)]
        [TestCase("\n= ", ExpressionType.Assignment, 2, 2)]
        [TestCase("//comment\n= ", ExpressionType.Assignment, 2, 2)]
        [TestCase("/* comment */= ", ExpressionType.Assignment, 1, 15)]
        [TestCase("/* comment */   = ", ExpressionType.Assignment, 1, 18)]
        [TestCase("/*\ncommentn\n*/\n  = ", ExpressionType.Assignment, 4, 4)]
        [TestCase("{ ", ExpressionType.StartBlock, 1, 2)]
        [TestCase(" { ", ExpressionType.StartBlock, 1, 3)]
        [TestCase("\n{ ", ExpressionType.StartBlock, 2, 2)]
        [TestCase("//comment\n{ ", ExpressionType.StartBlock, 2, 2)]
        [TestCase("/* comment */{ ", ExpressionType.StartBlock, 1, 15)]
        [TestCase("/* comment */   { ", ExpressionType.StartBlock, 1, 18)]
        [TestCase("/*\ncommentn\n*/\n  { ", ExpressionType.StartBlock, 4, 4)]
        public void ShouldDetermineIfAssignmentOrStartOfBlock(
            string input,
            ExpressionType expectedType,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.That(
                lexer.DetermineIfAssignmentOrStartBlock(),
                Is.EqualTo(expectedType),
                "Did not determine expression type correctly.");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        [TestCase("id ", ExpressionType.Identifier, 1, 1)]
        [TestCase("}", ExpressionType.EndBlock, 1, 2)]
        public void ShouldDetermineIfIdentifierOrEndBlock(
            string input,
            ExpressionType expectedType,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.That(
                lexer.DetermineIfIdentifierOrEndBlock(),
                Is.EqualTo(expectedType),
                "Did not determine if the next thing was an identifier or the end of a block.");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        [TestCase("64;", 64, 1, 4)]
        [TestCase("8;", 8, 1, 3)]
        [TestCase("08;", 8, 1, 4)]
        [TestCase("+16;", 16, 1, 5)]
        [TestCase("-16;", -16, 1, 5)]
        [TestCase("0xFf;", 255, 1, 6)]
        public void ShouldReadIntegerAssignment(
            string input,
            int expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.That(
                lexer.ReadIntegerAssignment(),
                Is.EqualTo(expectedResult),
                "Did not read integer correctly.");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        [TestCase("64;", 64d, 1, 4)]
        [TestCase("6.4;", 6.4d, 1, 5)]
        [TestCase("+1.6;", 1.6d, 1, 6)]
        [TestCase("-1.6;", -1.6d, 1, 6)]
        [TestCase("1e+9;", 1e+9d, 1, 6)]
        public void ShouldReadFloatingPointAssignment(
            string input,
            double expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.That(
                lexer.ReadFloatingPointAssignment(),
                Is.EqualTo(expectedResult),
                "Did not read floating point number correctly.");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        [TestCase("true;", true, 1, 6)]
        [TestCase("false;", false, 1, 7)]
        public void ShouldReadBooleanAssignment(
            string input,
            bool expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.That(
                lexer.ReadBooleanAssignment(),
                Is.EqualTo(expectedResult),
                "Did not read Boolean correctly.");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        [TestCase("\"Test String\";", "Test String", 1, 15)]
        [TestCase("\"0xFB010304\";", "0xFB010304", 1, 14)]
        [TestCase("\"\";", "", 1, 4)]
        public void ShouldReadStringAssignment(
            string input,
            string expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.That(
                lexer.ReadStringAssignment(),
                Is.EqualTo(expectedResult),
                "Did not read string correctly.");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        [TestCase("{ ", 1, 2)]
        [TestCase("     { ", 1, 7)]
        public void ShouldVerifyStartOfBlock(
            string input,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.DoesNotThrow(
                lexer.VerifyStartOfBlock,
                "Should have verified a start of block.");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        [TestCase("64;", 1, 4)]
        [TestCase("true;", 1, 6)]
        [TestCase("6.4;", 1, 5)]
        [TestCase("\"string\";", 1, 10)]
        public void ShouldMovePastAssignment(
            string input,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            Assert.DoesNotThrow(
                lexer.MovePastAssignment,
                "Should not have thrown moving past assignment,");

            VerifyReaderPosition(reader, expectedEndingLine, expectedEndingColumn);
        }

        private static IUwmfCharReader CreateReader(string input)
        {
            return new UwmfCharReader(new MemoryStream(Encoding.ASCII.GetBytes(input)));
        }

        private static void VerifyReaderPosition(IUwmfCharReader reader, int expectedLine, int expectedColumn)
        {
            Assert.That(
               reader.Current.Position.Line,
               Is.EqualTo(expectedLine),
               "Unexpected resulting line position of reader.");

            Assert.That(
                reader.Current.Position.Column,
                Is.EqualTo(expectedColumn),
                "Unexpected resulting column position of reader.");
        }
    }
}