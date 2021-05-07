// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class CharProperty : ScalarProperty
    {
        public CharProperty(string name) : base(name, "char", isNullable: false, defaultString: null)
        {
        }
    }
}