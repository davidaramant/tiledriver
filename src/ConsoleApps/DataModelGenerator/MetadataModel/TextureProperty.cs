// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class TextureProperty : ScalarProperty
    {
        public bool IsOptional { get; }

        public TextureProperty(string name, bool nullable = false, bool optional = false)
            : base(name, type: "Texture", isNullable: false, defaultString: null)
        {
            IsOptional = optional;
        }
    }
}
