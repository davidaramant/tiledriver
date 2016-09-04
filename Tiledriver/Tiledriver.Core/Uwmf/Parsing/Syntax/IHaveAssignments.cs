// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Functional.Maybe;

namespace Tiledriver.Core.Uwmf.Parsing.Syntax
{
    public interface IHaveAssignments
    {
        Maybe<Token> GetValueFor(string name);
        Maybe<Token> GetValueFor(Identifier name);
    }
}