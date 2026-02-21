using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo.MetadataModel;

sealed record AbstractBlock(string ClassName, ImmutableArray<Property> Metadata, ImmutableArray<Property> Properties)
	: IBlock;
