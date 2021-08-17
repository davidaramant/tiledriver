// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class BlockListProperty : CollectionProperty
    {
        public override string PropertyType => $"ImmutableList<{ElementTypeName}>";

        public BlockListProperty(string name, string? elementType = null) : base(name, elementType)
        {
        }
    }
}