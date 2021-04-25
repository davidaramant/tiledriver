// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed record TileMappings(
        IReadOnlyDictionary<ushort,AmbushModzone> AmbushModzones,
        IReadOnlyDictionary<ushort,ChangeTriggerModzone> ChangeTriggerModzones,
        IReadOnlyDictionary<ushort,TileTemplate> TileTemplates,
        IReadOnlyDictionary<ushort,TriggerTemplate> TriggerTemplates,
        IReadOnlyDictionary<ushort,ZoneTemplate> ZoneTemplates
    );
}
