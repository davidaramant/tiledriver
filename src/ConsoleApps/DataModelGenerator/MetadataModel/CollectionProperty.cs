// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Humanizer;

namespace Tiledriver.DataModelGenerator.MetadataModel
{
	abstract class CollectionProperty : Property
	{
		private readonly string? _explicitElementType;
		public virtual string ElementTypeName => (_explicitElementType ?? Name).Pascalize();
		public override string PropertyName => Name.Pluralize().Pascalize();

		protected CollectionProperty(string name, string? elementTypeName = null)
			: base(name)
		{
			_explicitElementType = elementTypeName;
		}
	}
}
