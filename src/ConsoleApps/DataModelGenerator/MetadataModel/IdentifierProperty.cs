// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    class IdentifierProperty : ScalarProperty
    {
        public IdentifierProperty(string name) : base(name)
        {
        }

        public override string PropertyType => "Identifier";
    }
}