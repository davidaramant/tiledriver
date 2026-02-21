using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Xlat.Reading.AbstractSyntaxTree;

public sealed record Section(IdentifierToken Name, ImmutableArray<IExpression> Contents) : IExpression;
