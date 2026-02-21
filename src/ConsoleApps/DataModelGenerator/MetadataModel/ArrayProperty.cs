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
