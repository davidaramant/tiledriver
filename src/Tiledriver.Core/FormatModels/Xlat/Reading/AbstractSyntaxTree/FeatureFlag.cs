// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Xlat.Reading.AbstractSyntaxTree
{
	public sealed record FeatureFlag(IdentifierToken FlagName) : IExpression;
}
