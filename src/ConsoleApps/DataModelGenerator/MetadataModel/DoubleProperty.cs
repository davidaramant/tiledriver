namespace Tiledriver.DataModelGenerator.MetadataModel;

sealed class DoubleProperty : ScalarProperty
{
	public double? Default { get; }

	public DoubleProperty(string name, int? defaultValue = null)
		: base(name, "double", isNullable: false, defaultValue?.ToString()) => Default = defaultValue;
}
