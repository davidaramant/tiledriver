// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel
{
    enum SerializationType
    {
        Normal,
        Custom,
        TopLevel
    }

    record Block(
        string Name,
        ImmutableArray<Property> Properties,
        SerializationType Serialization = SerializationType.Normal)
    {
        public string ClassName => Name.ToPascalCase();

        public IEnumerable<Property> OrderedProperties => 
            Properties.Where(p => p.DefaultString == null)
            .Concat(Properties.Where(p => p.DefaultString != null));
    }
}