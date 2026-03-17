using System.Collections.Immutable;
using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.MapInfo.Reading.AbstractSyntaxTree;

public sealed record VariableAssignment(IdentifierToken Id, ImmutableArray<Token> Values)
{
	public bool HasValues => Values.Any();
}
