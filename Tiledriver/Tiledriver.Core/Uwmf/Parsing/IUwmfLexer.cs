// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.Uwmf.Parsing
{
    public interface IUwmfLexer
    {
        Token MustReadTokenOfTypes(params TokenType[] types);
        Token MustReadValueToken();
    }
}