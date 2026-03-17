using System.Collections.Immutable;
using Tiledriver.FormatModels.Common.Reading;
using Tiledriver.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.FormatModels.Xlat.Reading.AbstractSyntaxTree;

public sealed record Statement(ImmutableArray<IdentifierToken> Components) : IExpression;
