// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel
{
    abstract class CollectionProperty : Property
    {
        public string GenericTypeName => Name.ToPascalCase();
        public override string CodeName => Name.ToPluralPascalCase();

        protected CollectionProperty(string name) : base(name)
        {
        }
    }
}