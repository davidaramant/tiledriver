// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Humanizer;

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    abstract class ScalarProperty : Property
    {
        public override string PropertyName => Name.Pascalize();
        public override string PropertyType { get; }
        public override string? DefaultString { get; }
        public bool IsNullable { get; }
        public string BasePropertyType { get; }

        protected ScalarProperty(string name, string type, bool isNullable, string? defaultString)
            : base(name)
        {
            BasePropertyType = type;
            PropertyType = type + (isNullable ? "?" : string.Empty);
            IsNullable = isNullable;
            DefaultString = isNullable ? "null" : defaultString;
        }
    }
}
