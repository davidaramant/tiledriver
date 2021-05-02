// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    class StringProperty : ScalarProperty
    {
        private readonly string? _formatName;

        public override string FormatName => _formatName ?? Name;
        public string? Default { get; }
        public override string PropertyType => "string";
        public override string? DefaultString => Default == null ? null : $"\"{Default}\"";

        public StringProperty(string name, string? defaultValue = null, string? formatName = null) : base(name)
        {
            _formatName = formatName;
            Default = defaultValue;
        }
    }
}