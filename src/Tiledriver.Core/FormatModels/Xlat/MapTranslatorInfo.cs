// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using Functional.Maybe;
using JetBrains.Annotations;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed class MapTranslatorInfo
    {
        private readonly Dictionary<ushort, IThingMapping> _thingMappingLookup = new Dictionary<ushort, IThingMapping>();

        [NotNull]
        public TileMappings TileMappings { get; }
        [NotNull]
        public IEnumerable<IThingMapping> ThingMappings { get; }

        [NotNull]
        public FlatMappings FlatMappings { get; }
        public bool EnableLightLevels { get; }

        public MapTranslatorInfo(
            [NotNull] TileMappings tileMappings,
            [NotNull] IEnumerable<IThingMapping> thingMappings,
            [NotNull] FlatMappings flatMappings,
            bool enableLightLevels)
        {
            TileMappings = tileMappings;
            ThingMappings = thingMappings;
            FlatMappings = flatMappings;
            EnableLightLevels = enableLightLevels;

            foreach (var mapping in thingMappings)
            {
                switch (mapping)
                {
                    case ThingTemplate thing when thing.Angles > 0:
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

        public IThingMapping LookupThingMapping(ushort oldNum)
        {
            return _thingMappingLookup[oldNum];
        }

        public Maybe<IThingMapping> TryLookupThingMapping(ushort oldNum)
        {
            return _thingMappingLookup.Lookup(oldNum);
        }
    }
}