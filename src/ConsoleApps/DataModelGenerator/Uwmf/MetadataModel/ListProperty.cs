// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel
{
    sealed class ListProperty : Property
    {
        public override string CodeName => Name.ToPluralPascalCase();
        public override string CodeType => $"ImmutableList<{Name.ToPascalCase()}>";

        public ListProperty(string name) : base(name)
        {
        }
    }
}