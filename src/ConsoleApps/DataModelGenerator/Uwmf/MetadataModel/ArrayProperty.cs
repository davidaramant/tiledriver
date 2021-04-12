// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel
{
    sealed class ArrayProperty : CollectionProperty
    {
        public override string CodeType => $"ImmutableArray<{Name.ToPascalCase()}>";

        public ArrayProperty(string name) : base(name)
        {
        }
    }
}