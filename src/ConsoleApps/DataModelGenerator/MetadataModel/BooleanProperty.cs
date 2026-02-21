namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class BooleanProperty : ScalarProperty
{
	public bool? Default { get; }

	public BooleanProperty(string name, bool? defaultValue = null, bool isNullable = false)
		: base(name, "bool", isNullable: isNullable, defaultValue?.ToString().ToLowerInvariant()) =>
		Default = defaultValue;
}
