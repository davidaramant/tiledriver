// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Piglet.Lexer;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public interface IUwmfLexer
    {
        ILexer<Token> Builder { get; }
    }
}