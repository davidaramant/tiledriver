// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class BlockListProperty : CollectionProperty
    {
        public override string PropertyType => $"ImmutableList<{Name.ToPascalCase()}>";

        public BlockListProperty(string name) : base(name)
        {
        }
    }
}