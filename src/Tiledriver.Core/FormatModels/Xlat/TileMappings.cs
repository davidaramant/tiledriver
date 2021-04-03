// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.Extensions.Collections;

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

            _condensedTileTemplateLookup = null;
        }

        private Dictionary<ushort, TileTemplate> _condensedTileTemplateLookup;

        private Dictionary<ushort, TileTemplate> GetCondensedTileTemplateLookup()
        {
            return _condensedTileTemplateLookup ??
                   (_condensedTileTemplateLookup = TileTemplates.CondenseToDictionary(t => t.OldNum, t => t));
        }

        public IEnumerable<TileTemplate> GetCondensedTileTemplates() => GetCondensedTileTemplateLookup().Values;

        public ImmutableDictionary<ushort, int> GetTileIndexLookup()
        {
            return
                GetCondensedTileTemplates().
                    Select((template, index) => new { template.OldNum, TileIndex = index }).
                    ToImmutableDictionary(pair => pair.OldNum, pair => pair.TileIndex);
        }
    }
}