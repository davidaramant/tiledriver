// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel
{
    sealed class MetadataIntegerProperty : IntegerProperty
    {
        public MetadataIntegerProperty(string name, int? defaultValue = null) 
            : base(name, defaultValue)
        {
        }
    }
}