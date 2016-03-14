﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
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

            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadIdentifier(),
                    Is.EqualTo(expectedIdentifier),
                    "Did not read identifier."),
                expectedEndingLine,
                expectedEndingColumn);
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
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.DetermineIfAssignmentOrStartBlock(),
                    Is.EqualTo(expectedType),
                    "Did not determine expression type correctly."),
                expectedEndingLine,
                expectedEndingColumn);
        }

        [TestCase("id ", ExpressionType.Identifier, 1, 1)]
        [TestCase("}", ExpressionType.EndBlock, 1, 2)]
        public void ShouldDetermineIfIdentifierOrEndBlock(
            string input,
            ExpressionType expectedType,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.DetermineIfIdentifierOrEndBlock(),
                    Is.EqualTo(expectedType),
                    "Did not determine if the next thing was an identifier or the end of a block."),
                expectedEndingLine,
                expectedEndingColumn);
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
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadIntegerAssignment(),
                    Is.EqualTo(expectedResult),
                    "Did not read integer correctly."),
                expectedEndingLine,
                expectedEndingColumn);
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
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadFloatingPointAssignment(),
                    Is.EqualTo(expectedResult),
                    "Did not read floating point number correctly."),
                expectedEndingLine,
                expectedEndingColumn);
        }

        [TestCase("true;", true, 1, 6)]
        [TestCase("false;", false, 1, 7)]
        public void ShouldReadBooleanAssignment(
            string input,
            bool expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadBooleanAssignment(),
                    Is.EqualTo(expectedResult),
                    "Did not read Boolean correctly."),
                expectedEndingLine,
                expectedEndingColumn);
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
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadStringAssignment(),
                    Is.EqualTo(expectedResult),
                    "Did not read string correctly."),
                expectedEndingLine,
                expectedEndingColumn);
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
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.DoesNotThrow(
                    lexer.MovePastAssignment,
                    "Should not have thrown moving past assignment,"),
                expectedEndingLine,
                expectedEndingColumn);
        }

        [Test]
        public void ShouldTokenizeRealisticScenario()
        {
            var input = @"string = ""String"";
int = 1;
float = 1.5;
flag = false;
emptyBlock1{}
emptyBlock2
{
}
block
{
    unknownProperty = 5;
}";

            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            // Don't bother with assertion messages since the result will have to be inspected anyway.

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("string")));
            Assert.That(lexer.DetermineIfAssignmentOrStartBlock(), Is.EqualTo(ExpressionType.Assignment));
            Assert.That(lexer.ReadStringAssignment(), Is.EqualTo("String"));

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("int")));
            Assert.That(lexer.DetermineIfAssignmentOrStartBlock(), Is.EqualTo(ExpressionType.Assignment));
            Assert.That(lexer.ReadIntegerAssignment(), Is.EqualTo(1));

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("float")));
            Assert.That(lexer.DetermineIfAssignmentOrStartBlock(), Is.EqualTo(ExpressionType.Assignment));
            Assert.That(lexer.ReadFloatingPointAssignment(), Is.EqualTo(1.5));

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("flag")));
            Assert.That(lexer.DetermineIfAssignmentOrStartBlock(), Is.EqualTo(ExpressionType.Assignment));
            Assert.That(lexer.ReadBooleanAssignment(), Is.EqualTo(false));

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("emptyBlock1")));
            Assert.That(lexer.DetermineIfAssignmentOrStartBlock(), Is.EqualTo(ExpressionType.StartBlock));
            Assert.That(lexer.DetermineIfIdentifierOrEndBlock(), Is.EqualTo(ExpressionType.EndBlock));

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("emptyBlock2")));
            Assert.That(lexer.DetermineIfAssignmentOrStartBlock(), Is.EqualTo(ExpressionType.StartBlock));
            Assert.That(lexer.DetermineIfIdentifierOrEndBlock(), Is.EqualTo(ExpressionType.EndBlock));

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("block")));
            Assert.That(lexer.DetermineIfAssignmentOrStartBlock(), Is.EqualTo(ExpressionType.StartBlock));
            Assert.That(lexer.DetermineIfIdentifierOrEndBlock(), Is.EqualTo(ExpressionType.Identifier));
            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("unknownProperty")));
            Assert.DoesNotThrow(lexer.MovePastAssignment);
            Assert.That(lexer.DetermineIfIdentifierOrEndBlock(), Is.EqualTo(ExpressionType.EndBlock));
        }

        private static IUwmfCharReader CreateReader(string input)
        {
            return new UwmfCharReader(new MemoryStream(Encoding.ASCII.GetBytes(input)));
        }

        private static void RunTestAndVerifyResultingPosition(
            string input,
            Action<Lexer> assertion,
            int expectedLine,
            int expectedColumn)
        {
            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            assertion(lexer);

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