using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Xlat.Reading.AbstractSyntaxTree;

public sealed record FeatureFlag(IdentifierToken FlagName) : IExpression;
