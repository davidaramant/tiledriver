// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Humanizer;

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class BlockProperty : ScalarProperty
    {
        public BlockProperty(string name, string? propertyType = null, bool isNullable = false)
            : base(name, propertyType ?? name.Pascalize(), isNullable: isNullable, defaultString: null)
        {
        }
    }
}