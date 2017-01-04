// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using NUnit.Framework;
using Piglet.Lexer;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat.Parsing;
using Tiledriver.Core.Tests.FormatModels.Common;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Parsing
{
    [TestFixture]
    public sealed class XlatLexerTests : BaseLexerTests
    {
        [Test]
        public void ShouldLexRealXlat()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "Xlat", "Parsing", "wolf3d.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                while (lexer.ReadToken().Type != TokenType.EndOfFile)
                {
                    // should not throw here
                }
            }
        }

        protected override ILexer<Token> GetDefinition()
        {
            return XlatLexer.Definition;
        }
    }
}