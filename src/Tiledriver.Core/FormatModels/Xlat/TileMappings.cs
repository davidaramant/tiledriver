using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Xlat;

public sealed record TileMappings(
	ImmutableArray<AmbushModzone> AmbushModzones,
	ImmutableArray<ChangeTriggerModzone> ChangeTriggerModzones,
	ImmutableArray<TileTemplate> TileTemplates,
	ImmutableArray<TriggerTemplate> TriggerTemplates,
	ImmutableArray<ZoneTemplate> ZoneTemplates
);
