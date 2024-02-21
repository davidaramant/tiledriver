// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.MetadataModel
{
	abstract class Property
	{
		public string Name { get; }
		public virtual string FormatName => Name;
		public abstract string PropertyName { get; }
		public abstract string PropertyType { get; }
		public virtual string? DefaultString => null;
		public bool HasDefault => DefaultString != null;

		protected Property(string name) => Name = name;
	}
}
