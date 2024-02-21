// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.MapInfo.Reading.AbstractSyntaxTree
{
	public sealed record VariableAssignment(IdentifierToken Id, ImmutableArray<Token> Values)
	{
		public bool HasValues => Values.Any();
	}
}
