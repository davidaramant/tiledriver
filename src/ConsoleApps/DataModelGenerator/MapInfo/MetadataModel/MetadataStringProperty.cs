// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 


using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel
{
    sealed class MetadataStringProperty : StringProperty
    {
        public MetadataStringProperty(string name, string? defaultValue = null, string? formatName = null) 
            : base(name, defaultValue, formatName)
        {
        }
    }
}