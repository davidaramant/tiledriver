namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class FlagProperty : ScalarProperty
{
	public bool Default => true;

	public FlagProperty(string name, bool isNullable = false)
		: base(name, "bool", isNullable: isNullable, defaultString: "true") { }
}
