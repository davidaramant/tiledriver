// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed class MapTranslation
    {
        private readonly Dictionary<ushort, IThingMapping> _thingMappingLookup = new();

        public TileMappings TileMappings { get; }
        public FlatMappings? FlatMappings { get; }

        public MapTranslation(
            TileMappings tileMappings,
            IEnumerable<IThingMapping> thingMappings,
            FlatMappings? flatMappings)
        {
            TileMappings = tileMappings;
            FlatMappings = flatMappings;

            foreach (var mapping in thingMappings)
            {
                switch (mapping)
                {
                    case ThingTemplate { Angles: > 0 } thing:
                        for (var i = thing.OldNum; i < thing.OldNum + thing.Angles; i++)
                        {
                            _thingMappingLookup[i] = thing;
                        }
                        break;

                    default:
                        _thingMappingLookup[mapping.OldNum] = mapping;
                        break;
                }
            }
        }

        public IThingMapping LookupThingMapping(ushort oldNum) => _thingMappingLookup[oldNum];
        public IThingMapping? TryLookupThingMapping(ushort oldNum) => _thingMappingLookup.TryGetValue(oldNum, out var value) ? value : null;
    }
}