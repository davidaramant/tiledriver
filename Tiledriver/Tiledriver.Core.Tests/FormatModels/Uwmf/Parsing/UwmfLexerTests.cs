// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using NUnit.Framework;
using Piglet.Lexer;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Core.Tests.FormatModels.Common;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Parsing
{
    [TestFixture]
    public sealed class UwmfLexerTests : BaseLexerTests
    {
        protected override ILexer<Token> GetDefinition()
        {
            return UwmfLexer.Definition;
        }
    }
}