using Humanizer;

namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class BlockProperty : ScalarProperty
{
	public BlockProperty(string name, string? propertyType = null, bool isNullable = false)
		: base(name, propertyType ?? name.Pascalize(), isNullable: isNullable, defaultString: null) { }
}
