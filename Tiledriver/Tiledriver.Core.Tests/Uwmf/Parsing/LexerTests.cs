/*
** LexerTests.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Tiledriver.Core.Uwmf;
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

        [TestCase("id", TokenType.Identifier, 1, 1)]
        [TestCase("=", TokenType.Assignment, 1, 1)]
        [TestCase(";", TokenType.EndOfAssignment, 1, 1)]
        [TestCase("{", TokenType.StartBlock, 1, 1)]
        [TestCase("}", TokenType.EndBlock, 1, 1)]
        [TestCase(",", TokenType.Comma, 1, 1)]
        [TestCase("", TokenType.EndOfFile, 1, 1)]
        [TestCase("1", TokenType.Unknown, 1, 1)]
        public void ShouldDetermineTokenType(
            string input,
            TokenType expectedType,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.DetermineNextToken(),
                    Is.EqualTo(expectedType),
                    "Did not determine token type."),
                expectedEndingLine,
                expectedEndingColumn);
        }

        [TestCase("64;", 64, 1, 3)]
        [TestCase("8;", 8, 1, 2)]
        [TestCase("08;", 8, 1, 3)]
        [TestCase("+16;", 16, 1, 4)]
        [TestCase("-16;", -16, 1, 4)]
        [TestCase("0xFf;", 255, 1, 5)]
        public void ShouldReadInteger(
            string input,
            int expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadIntegerNumber(),
                    Is.EqualTo(expectedResult),
                    "Did not read integer correctly."),
                expectedEndingLine,
                expectedEndingColumn);
        }

        [TestCase("64;", 64d, 1, 3)]
        [TestCase("6.4;", 6.4d, 1, 4)]
        [TestCase("+1.6;", 1.6d, 1, 5)]
        [TestCase("-1.6;", -1.6d, 1, 5)]
        [TestCase("1e+9;", 1e+9d, 1, 5)]
        public void ShouldReadFloatingPointNumber(
            string input,
            double expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadFloatingPointNumber(),
                    Is.EqualTo(expectedResult),
                    "Did not read floating point number correctly."),
                expectedEndingLine,
                expectedEndingColumn);
        }

        [TestCase("true;", true, 1, 5)]
        [TestCase("false;", false, 1, 6)]
        public void ShouldReadBoolean(
            string input,
            bool expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadBoolean(),
                    Is.EqualTo(expectedResult),
                    "Did not read Boolean correctly."),
                expectedEndingLine,
                expectedEndingColumn);
        }

        [TestCase("\"Test String\";", "Test String", 1, 14)]
        [TestCase("\"0xFB010304\";", "0xFB010304", 1, 13)]
        [TestCase("\"\";", "", 1, 3)]
        public void ShouldReadString(
            string input,
            string expectedResult,
            int expectedEndingLine,
            int expectedEndingColumn)
        {
            RunTestAndVerifyResultingPosition(input, lexer =>
                Assert.That(
                    lexer.ReadString(),
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
}
block2
{
    {1,2,3},
    {4,5}
}";

            var reader = CreateReader(input);
            var lexer = new Lexer(reader);

            // Don't bother with assertion messages since the result will have to be inspected anyway.

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("string")));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Assignment));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.ReadString(), Is.EqualTo("String"));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndOfAssignment));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("int")));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Assignment));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.ReadIntegerNumber(), Is.EqualTo(1));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndOfAssignment));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("float")));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Assignment));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.ReadFloatingPointNumber(), Is.EqualTo(1.5));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndOfAssignment));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("flag")));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Assignment));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.ReadBoolean(), Is.EqualTo(false));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndOfAssignment));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("emptyBlock1")));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.StartBlock));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndBlock));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("emptyBlock2")));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.StartBlock));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndBlock));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("block")));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.StartBlock));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Identifier));
            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("unknownProperty")));
            Assert.DoesNotThrow(lexer.MovePastAssignment);
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndBlock));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.ReadIdentifier(), Is.EqualTo(new Identifier("block2")));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.StartBlock));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.StartBlock));
            lexer.AdvanceOneCharacter();
            Assert.That( lexer.DetermineNextToken(), Is.EqualTo(TokenType.Unknown));
            Assert.That( lexer.ReadIntegerNumber(), Is.EqualTo(1));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Comma));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.ReadIntegerNumber(), Is.EqualTo(2));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Comma));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.ReadIntegerNumber(), Is.EqualTo(3));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndBlock));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Comma));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.StartBlock));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.ReadIntegerNumber(), Is.EqualTo(4));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.Comma));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.ReadIntegerNumber(), Is.EqualTo(5));
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndBlock));
            lexer.AdvanceOneCharacter();
            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndBlock));
            lexer.AdvanceOneCharacter();

            Assert.That(lexer.DetermineNextToken(), Is.EqualTo(TokenType.EndOfFile));
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