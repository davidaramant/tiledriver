// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree
{
    public sealed record IntTuple(FilePosition StartLocation, ImmutableList<IntegerToken> Values);
}