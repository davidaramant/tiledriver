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
    }
}