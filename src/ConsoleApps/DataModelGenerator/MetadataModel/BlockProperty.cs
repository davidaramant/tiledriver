// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class BlockProperty : ScalarProperty
    {
        public override string PropertyType { get; }
        public override string? DefaultString { get; }

        public BlockProperty(string name, string? propertyType = null, bool nullable = false) : base(name)
        {
            PropertyType = (propertyType ?? PropertyName) + (nullable ? "?" : string.Empty);
            DefaultString = nullable ? "null" : null;
        }
    }
}