// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax
{
    public interface IExpression : IEnumerable<Assignment>, IHaveAssignments
    {
        Maybe<Identifier> Name { get; }
        Maybe<ushort> Oldnum { get; }
        bool HasQualifiers { get; }
        IEnumerable<Identifier> Qualifiers { get; }

        bool HasValues { get; }
        IEnumerable<Token> Values { get; }
    }
}