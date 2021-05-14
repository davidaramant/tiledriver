// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class TextureProperty : ScalarProperty
    {
        public TextureProperty(string name, bool nullable = false) 
            : base(name, "Texture", isNullable: false, defaultString: null)
        {
        }
    }
}