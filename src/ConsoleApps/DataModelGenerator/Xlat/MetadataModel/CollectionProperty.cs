﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.DataModelGenerator.Utilities;

namespace Tiledriver.DataModelGenerator.Xlat.MetadataModel
{
    abstract class CollectionProperty : Property
    {
        public virtual string ElementTypeName => Name.ToPascalCase();
        public override string PropertyName => Name.ToPluralPascalCase();

        protected CollectionProperty(string name) : base(name)
        {
        }
    }
}