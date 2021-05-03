// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel
{
    sealed record InheritedBlock(
        string FormatName,
        string BaseClass,
        ImmutableArray<Property> Metadata,
        ImmutableArray<Property> Properties) : IBlock
    {
        public string ClassName => FormatName.ToPascalCase();
    }
}