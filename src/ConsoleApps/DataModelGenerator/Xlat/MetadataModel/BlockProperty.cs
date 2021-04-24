// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.Xlat.MetadataModel
{
    sealed class BlockProperty : ScalarProperty
    {
        public BlockProperty(string name) : base(name)
        {
        }

        public override string PropertyType => PropertyName;
    }
}