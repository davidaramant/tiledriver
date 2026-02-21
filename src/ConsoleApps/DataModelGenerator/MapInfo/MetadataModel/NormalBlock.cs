using System.Collections.Immutable;
using Humanizer;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel;

sealed record NormalBlock(
	string FormatName,
	string ClassName,
	ImmutableArray<Property> Metadata,
	ImmutableArray<Property> Properties,
	SerializationType Serialization = SerializationType.Normal
) : IBlock
{
	public string Name => FormatName;

	public NormalBlock(
		string FormatName,
		ImmutableArray<Property> Metadata,
		ImmutableArray<Property> Properties,
		SerializationType Serialization = SerializationType.Normal
	)
		: this(FormatName, FormatName.Pascalize(), Metadata, Properties, Serialization) { }
}
