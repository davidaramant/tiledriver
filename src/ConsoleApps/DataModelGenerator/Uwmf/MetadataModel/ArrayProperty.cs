// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Humanizer;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.Uwmf.MetadataModel
{
    sealed class ArrayProperty : CollectionProperty
    {
        public override string PropertyType => $"ImmutableArray<{Name.Pascalize()}>";

        public ArrayProperty(string name) : base(name)
        {
        }
    }
}