// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    sealed class IdentifierProperty : ScalarProperty
    {
        public IdentifierProperty(string name)
            : base(name, "Identifier", isNullable: false, defaultString: null) { }
    }
}
