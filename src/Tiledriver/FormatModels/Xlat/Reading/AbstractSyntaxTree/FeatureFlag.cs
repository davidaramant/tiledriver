using Tiledriver.FormatModels.Common.Reading;
using Tiledriver.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.FormatModels.Xlat.Reading.AbstractSyntaxTree;

public sealed record FeatureFlag(IdentifierToken FlagName) : IExpression;
