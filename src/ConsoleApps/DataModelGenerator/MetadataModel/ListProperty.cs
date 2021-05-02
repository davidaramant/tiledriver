// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class ListProperty : CollectionProperty
    {
        public ListProperty(string name, string elementType) : base(name)
        {
            ElementType = elementType;
        }

        public override string PropertyType => $"ImmutableList<{ElementType}>";
        public string ElementType { get; }
    }
}