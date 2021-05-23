// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed record TileMappings(
            ImmutableList<AmbushModzone> AmbushModzones,
            ImmutableList<ChangeTriggerModzone> ChangeTriggerModzones,
            ImmutableList<TileTemplate> TileTemplates,
            ImmutableList<TriggerTemplate> TriggerTemplates,
            ImmutableList<ZoneTemplate> ZoneTemplates);
}
