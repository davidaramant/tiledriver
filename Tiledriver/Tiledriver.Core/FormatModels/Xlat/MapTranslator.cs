// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using JetBrains.Annotations;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed class MapTranslator
    {
        public TileMappings TileMappings { get; }
        public ThingMappings ThingMappings { get; }
        public FlatMappings FlatMappings { get; }
        public bool EnableLightLevels { get; }

        public MapTranslator(
            [NotNull] TileMappings tileMappings,
            [NotNull] ThingMappings thingMappings,
            [NotNull] FlatMappings flatMappings,
            bool enableLightLevels)
        {
            TileMappings = tileMappings;
            ThingMappings = thingMappings;
            FlatMappings = flatMappings;
            EnableLightLevels = enableLightLevels;
        }
    }
}