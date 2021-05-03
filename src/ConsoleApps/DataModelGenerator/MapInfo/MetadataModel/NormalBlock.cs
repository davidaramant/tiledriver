// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 


using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel
{
    sealed record NormalBlock(
        string FormatName,
        string ClassName,
        ImmutableArray<Property> Metadata,
        ImmutableArray<Property> Properties,
        SerializationType Serialization = SerializationType.Normal) : IBlock
    {
        public string Name => FormatName;

        public NormalBlock(
            string FormatName,
            ImmutableArray<Property> Metadata,
            ImmutableArray<Property> Properties,
            SerializationType Serialization = SerializationType.Normal) 
            : this(
                FormatName, 
                FormatName.ToPascalCase(), 
                Metadata,
                Properties, 
                Serialization)
        {
        }
    }
}