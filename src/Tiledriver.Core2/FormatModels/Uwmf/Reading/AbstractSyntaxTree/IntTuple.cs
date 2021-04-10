// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Uwmf.Reading;

namespace Tiledriver.Core2.FormatModels.Uwmf.Reading.AbstractSyntaxTree
{
    public sealed record IntTuple(ImmutableList<IntegerToken> Values);
}