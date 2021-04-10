// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Common;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree
{
    public sealed record Block(Identifier Name, ImmutableArray<Assignment> Fields) : IGlobalExpression;
}