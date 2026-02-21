namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class TextureProperty : ScalarProperty
{
	public bool IsOptional { get; }

	public TextureProperty(string name, bool nullable = false, bool optional = false)
		: base(name, type: "Texture", isNullable: false, defaultString: null)
	{
		IsOptional = optional;
	}
}
