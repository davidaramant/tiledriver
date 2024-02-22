// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class ArrayProperty : CollectionProperty
{
	public ArrayProperty(string name, string elementType)
		: base(name)
	{
		ElementType = elementType;
	}

	public override string PropertyType => $"ImmutableArray<{ElementType}>";
	public string ElementType { get; }
}
