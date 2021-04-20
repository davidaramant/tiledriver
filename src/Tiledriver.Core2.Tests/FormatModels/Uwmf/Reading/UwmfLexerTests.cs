﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 
using Xunit;
using FluentAssertions;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf.Reading;
using Tiledriver.Core.FormatModels.Uwmf.Writing;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Reading
{
    public sealed class UwmfLexerTests
    {
        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData("0000045", 45)]
        [InlineData("123", 123)]
        [InlineData("-123", -123)]
        [InlineData("+123", 123)]
        [InlineData("0x1234", 0x1234)]
        public void ShouldLexInteger(string input, int expected)
        {
            var tokens = Scan(input);
            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<IntegerToken>().Which.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData("0.", 0d)]
        [InlineData("1.", 1d)]
        [InlineData("1.23", 1.23)]
        [InlineData("-1.23", -1.23)]
        [InlineData("+1.23", 1.23)]
        public void ShouldLexFloat(string input, double expected)
        {
            var tokens = Scan(input);
            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<FloatToken>().Which.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void ShouldLexBoolean(string input, bool expected)
        {
            var tokens = Scan(input);
            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<BooleanToken>().Which.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData("\"\"", "")]
        [InlineData("\"Some value 123 _\"", "Some value 123 _")]
        public void ShouldLexString(string input, string expected)
        {
            var tokens = Scan(input);
            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<StringToken>().Which.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("A")]
        [InlineData("_")]
        [InlineData("someName_123")]
        [InlineData("t")]
        [InlineData("true_1")]
        public void ShouldLexIdentifier(string id)
        {
            var tokens = Scan(id);
            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<IdentifierToken>().Which.Id.Should().Be(new Identifier(id));
        }

        [Theory]
        [InlineData("id=1;")]
        [InlineData(" id = 1 ; ")]
        [InlineData("// Some comment\n id = 1 ; ")]
        [InlineData("/*\nSome comment\n*/id = 1 ; ")]
        [InlineData("id=1;//ending comment")]
        [InlineData("id=1;/*ending block comment*/")]
        public void ShouldLexAssignment(string input)
        {
            var tokens = Scan(input);
            tokens.Should().HaveCount(4);
            tokens[0].Should().BeOfType<IdentifierToken>();
            tokens[1].Should().BeOfType<EqualsToken>();
            tokens[2].Should().BeOfType<IntegerToken>();
            tokens[3].Should().BeOfType<SemicolonToken>();
        }

        [Theory]
        [InlineData("blockName{}")]
        [InlineData("blockName\n{\n}\n")]
        public void ShouldLexBlock(string input)
        {
            var tokens = Scan(input);
            tokens.Should().HaveCount(3);
            tokens[0].Should().BeOfType<IdentifierToken>();
            tokens[1].Should().BeOfType<OpenBraceToken>();
            tokens[2].Should().BeOfType<CloseBraceToken>();
        }

        [Fact]
        public void ShouldLexTuple()
        {
            var tokens = Scan("{1,2,-3}");
            tokens.Should().HaveCount(7);
            tokens[0].Should().BeOfType<OpenBraceToken>();
            tokens[1].Should().BeOfType<IntegerToken>().Which.Value.Should().Be(1);
            tokens[2].Should().BeOfType<CommaToken>();
            tokens[3].Should().BeOfType<IntegerToken>().Which.Value.Should().Be(2);
            tokens[4].Should().BeOfType<CommaToken>();
            tokens[5].Should().BeOfType<IntegerToken>().Which.Value.Should().Be(-3);
            tokens[6].Should().BeOfType<CloseBraceToken>();
        }

        private static Token[] Scan(string input)
        {
            using var stringReader = new StringReader(input);
            var lexer = new UwmfLexer(stringReader);
            return lexer.Scan().ToArray();
        }

        [Fact]
        public void ShouldHandleLexingDemoMap()
        {
            var map = ThingDemoMap.Create();

            using var stream = new MemoryStream();
            map.WriteTo(stream);

            stream.Position = 0;

            using var textReader = new StreamReader(stream, Encoding.ASCII);
            var lexer = new UwmfLexer(textReader);
            var result = lexer.Scan().ToArray();
        }

        [Fact]
        public void ShouldLexTestFile()
        {
            using var stream = TestFile.Xlat.spear;
            using var textReader = new StreamReader(stream, Encoding.ASCII);
            var lexer = new UwmfLexer(textReader);
            var result = lexer.Scan().ToArray();
        }
    }
}