// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;
using System.Text;
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
    }
}