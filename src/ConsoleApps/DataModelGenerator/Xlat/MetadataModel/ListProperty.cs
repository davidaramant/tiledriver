// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Xlat.MetadataModel
{
    sealed class ListProperty : CollectionProperty
    {
        public override string PropertyType => $"ImmutableList<{Name.ToPascalCase()}>";

        public ListProperty(string name) : base(name)
        {
        }
    }
}