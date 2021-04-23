// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common.UnifiedReading;
using Tiledriver.Core.FormatModels.Common.UnifiedReading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Xlat.Reading.AbstractSyntaxTree
{
    public sealed record Section(IdentifierToken Name, ImmutableArray<IExpression> Contents) : IExpression;
}