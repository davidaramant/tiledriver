// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.Extensions;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class TileMappings
    {
        public void Add(TileMappings newMappings)
        {
            AmbushModzones.Merge(newMappings.AmbushModzones);
            ChangeTriggerModzones.Merge(newMappings.ChangeTriggerModzones);
            Tiles.Merge(newMappings.Tiles);
            PositionlessTriggers.Merge(newMappings.PositionlessTriggers);
            Zones.Merge(newMappings.Zones);
        }
    }
}