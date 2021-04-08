// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
using Xunit;
using Piglet.Lexer;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat.Parsing;
using Tiledriver.Core.Tests.FormatModels.Common;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Parsing
{
    public sealed class XlatLexerTests : BaseLexerTests
    {
        [Fact]
        public void ShouldLexRealXlat()
        {
            using (var stream = TestFile.Xlat.wolf3d)
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