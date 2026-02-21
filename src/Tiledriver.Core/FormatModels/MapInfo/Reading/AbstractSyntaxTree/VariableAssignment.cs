using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.MapInfo.Reading.AbstractSyntaxTree;

public sealed record VariableAssignment(IdentifierToken Id, ImmutableArray<Token> Values)
{
	public bool HasValues => Values.Any();
}
