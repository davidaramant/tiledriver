// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    abstract class ScalarProperty : Property
    {
        public override string PropertyName => Name.ToPascalCase();

        protected ScalarProperty(string name) : base(name)
        {
        }
    }
}