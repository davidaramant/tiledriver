// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Humanizer;

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    enum SerializationType
    {
        Normal,
        Custom,
        TopLevel,
        OrderedProperties
    }

    sealed record Block(
        string FormatName,
        string ClassName,
        ImmutableArray<Property> Properties,
        SerializationType Serialization = SerializationType.Normal
    )
    {
        public string Name => FormatName;

        public Block(
            string FormatName,
            ImmutableArray<Property> Properties,
            SerializationType Serialization = SerializationType.Normal
        )
            : this(FormatName, FormatName.Pascalize(), Properties, Serialization) { }

        public IEnumerable<Property> OrderedProperties =>
            Properties.Where(p => !p.HasDefault).Concat(Properties.Where(p => p.HasDefault));
    }
}
