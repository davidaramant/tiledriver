// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class DoubleProperty : ScalarProperty
    {
        public double? Default { get; }
        public override string PropertyType => "double";
        public override string? DefaultString => Default?.ToString();

        public DoubleProperty(string name, int? defaultValue = null) : base(name) => Default = defaultValue;
    }
}