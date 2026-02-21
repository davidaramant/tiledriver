namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class IntegerProperty : ScalarProperty
{
	public int? Default { get; }

	public IntegerProperty(string name, int? defaultValue = null, bool isNullable = false)
		: base(name, "int", isNullable: isNullable, defaultString: defaultValue?.ToString())
	{
		Default = defaultValue;
	}
}
