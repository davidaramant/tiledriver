// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Xlat.Reading.AbstractSyntaxTree;

public sealed record Block(
	IdentifierToken Name,
	ushort OldName,
	ImmutableArray<IdentifierToken> Attributes,
	ImmutableArray<Assignment> Fields
) : IExpression
{
	public IReadOnlyDictionary<Identifier, Token> GetFieldAssignments()
	{
		var assignments = new Dictionary<Identifier, Token>();

		foreach (var field in Fields)
		{
			if (assignments.ContainsKey(field.Name.Id))
			{
				throw new ParsingException(
					$"Duplicate field definition found: {field.Name.Id} on {field.Name.Location}"
				);
			}

			assignments.Add(field.Name.Id, field.Value);
		}

		return assignments;
	}
}
