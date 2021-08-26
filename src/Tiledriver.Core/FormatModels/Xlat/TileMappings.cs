// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed record TileMappings(
        ImmutableArray<AmbushModzone> AmbushModzones,
        ImmutableArray<ChangeTriggerModzone> ChangeTriggerModzones,
        ImmutableArray<TileTemplate> TileTemplates,
        ImmutableArray<TriggerTemplate> TriggerTemplates,
        ImmutableArray<ZoneTemplate> ZoneTemplates);
}
