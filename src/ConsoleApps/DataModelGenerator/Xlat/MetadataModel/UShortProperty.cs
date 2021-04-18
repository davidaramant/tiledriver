// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.Xlat.MetadataModel
{
    sealed class UShortProperty : ScalarProperty
    {
        public override string PropertyType => "ushort";

        public UShortProperty(string name) : base(name)
        {
        }
    }
}