// Copyright (c) 2016 David Aramant
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
        [TestCase("someproperty = 10;")]
        [TestCase("      someProperty=10;")]
        [TestCase("      SOMEPROPERTY=10;")]
        [TestCase("/* comment */ someProperty = 10;")]
        [TestCase("// comment\nsomeProperty = 10;")]
        public void ShouldReadIdentifier(string input)
        {
            var expectedIdentifier = new Identifier("someproperty");

            using (var inputStream = CreateTestStream(input))
            {
                var lexer = Create(inputStream);

                Assert.That(
                    lexer.ReadIdentifier(), 
                    Is.EqualTo(expectedIdentifier), 
                    "Did not read identifier.");
            }
        }

        private static Stream CreateTestStream(string input)
        {
            return new MemoryStream(Encoding.ASCII.GetBytes(input));
        }

        private static ILexer Create(Stream input)
        {
            return new Lexer(input);
        }
    }
}