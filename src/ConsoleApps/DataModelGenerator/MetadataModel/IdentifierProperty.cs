namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class IdentifierProperty : ScalarProperty
{
	public IdentifierProperty(string name)
		: base(name, "Identifier", isNullable: false, defaultString: null) { }
}
