// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.Xlat.MetadataModel
{
    sealed class UShortProperty : ScalarProperty
    {
        public UShortProperty(string name) : base(name, "ushort", isNullable: false, defaultString: null)
        {
        }
    }
}