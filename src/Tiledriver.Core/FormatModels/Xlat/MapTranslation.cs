// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed class MapTranslation
    {
        public IReadOnlyDictionary<ushort, IMapping> ThingMappingLookup { get; }
        public TileMappings TileMappings { get; }
        public FlatMappings? FlatMappings { get; }

        public MapTranslation(
            TileMappings tileMappings,
            IEnumerable<IMapping> thingMappings,
            FlatMappings? flatMappings)
        {
            TileMappings = tileMappings;
            FlatMappings = flatMappings;

            var thingMappingLookup = new Dictionary<ushort, IMapping>();

            foreach (var mapping in thingMappings)
            {
                switch (mapping)
                {
                    case ThingTemplate { Angles: > 0 } thing:
                        for (var i = thing.OldNum; i < thing.OldNum + thing.Angles; i++)
                        {
                            thingMappingLookup[i] = thing;
                        }
                        break;

                    default:
                        thingMappingLookup[mapping.OldNum] = mapping;
                        break;
                }
            }

            ThingMappingLookup = thingMappingLookup;
        }
    }
}