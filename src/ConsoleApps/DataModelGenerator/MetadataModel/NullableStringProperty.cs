// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    class NullableStringProperty : ScalarProperty
    {
        private readonly string? _formatName;

        public override string FormatName => _formatName ?? Name;
        public string? Default => null;
        public override string PropertyType => "string?";
        public override string? DefaultString => "null";

        public NullableStringProperty(string name, string? formatName = null) : base(name)
        {
            _formatName = formatName;
        }
    }
}