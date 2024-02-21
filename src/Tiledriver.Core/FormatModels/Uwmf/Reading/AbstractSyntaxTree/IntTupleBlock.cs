// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree
{
	public sealed record IntTupleBlock(IdentifierToken Name, ImmutableArray<IntTuple> Tuples) : IExpression;
}
