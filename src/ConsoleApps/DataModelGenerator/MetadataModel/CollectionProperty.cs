// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Humanizer;

namespace Tiledriver.DataModelGenerator.MetadataModel
{
    abstract class CollectionProperty : Property
    {
        public virtual string ElementTypeName => Name.Pascalize();
        public override string PropertyName => Name.Pluralize().Pascalize();

        protected CollectionProperty(string name) : base(name)
        {
        }
    }
}