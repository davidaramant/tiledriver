namespace Tiledriver.DataModelGenerator.MetadataModel;

class StringProperty : ScalarProperty
{
	public override string FormatName { get; }

	public string? Default { get; }

	public StringProperty(string name, string? defaultValue = null, string? formatName = null, bool isNullable = false)
		: base(
			name,
			"string",
			isNullable: isNullable,
			defaultString: defaultValue == null ? null : $"\"{defaultValue}\""
		)
	{
		FormatName = formatName ?? name;
		Default = defaultValue;
	}
}
