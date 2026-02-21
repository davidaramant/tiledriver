namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class CharProperty : ScalarProperty
{
	public CharProperty(string name)
		: base(name, "char", isNullable: false, defaultString: null) { }
}
