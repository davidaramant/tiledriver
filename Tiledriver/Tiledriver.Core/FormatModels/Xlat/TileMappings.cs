// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class TileMappings
    {
        public void Add(TileMappings newMappings)
        {
            AmbushModzones.AddRange(newMappings.AmbushModzones);
            ChangeTriggerModzones.AddRange(newMappings.ChangeTriggerModzones);
            TileTemplates.AddRange(newMappings.TileTemplates);
            TriggerTemplates.AddRange(newMappings.TriggerTemplates);
            ZoneTemplates.AddRange(newMappings.ZoneTemplates);
        }
    }
}