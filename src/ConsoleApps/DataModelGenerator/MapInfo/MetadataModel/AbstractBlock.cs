// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel
{
    sealed record AbstractBlock(
        string ClassName,
        ImmutableArray<Property> Metadata,
        ImmutableArray<Property> Properties) : IBlock;
}