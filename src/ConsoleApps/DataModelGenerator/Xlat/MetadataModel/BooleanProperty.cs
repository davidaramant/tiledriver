// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.Xlat.MetadataModel
{
    sealed class BooleanProperty : ScalarProperty
    {
        public bool? Default { get; }
        public override string PropertyType => "bool";
        public override string? DefaultString => Default?.ToString()?.ToLowerInvariant();

        public BooleanProperty(string name, bool? defaultValue = null) : base(name) => Default = defaultValue;
    }
}