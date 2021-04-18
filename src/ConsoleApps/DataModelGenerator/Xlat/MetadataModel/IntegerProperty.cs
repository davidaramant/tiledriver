// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.Xlat.MetadataModel
{
    sealed class IntegerProperty : ScalarProperty
    {
        public int? Default { get; }
        public override string PropertyType => "int";
        public override string? DefaultString => Default?.ToString();

        public IntegerProperty(string name, int? defaultValue = null) : base(name)
        {
            Default = defaultValue;
        }
    }
}