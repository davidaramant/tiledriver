// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class BlockProperty : ScalarProperty
    {
        public override string PropertyType { get; }

        public BlockProperty(string name, string? propertyType = null) : base(name)
        {
            PropertyType = propertyType ?? PropertyName;
        }
    }
}