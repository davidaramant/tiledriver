// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.MapTranslators
{
    public sealed class BinaryMapTranslator
    {
        private readonly MapTranslatorInfo _translatorInfo;

        public BinaryMapTranslator(MapTranslatorInfo translatorInfo)
        {
            _translatorInfo = translatorInfo;
        }

        public MapData Translate(BinaryMap binaryMap)
        {


            var map = new MapData
            {
                NameSpace = "Wolf3D",
                TileSize = 64,
                Width = binaryMap.Size.Width,
                Height = binaryMap.Size.Height,
                Name = binaryMap.Name,

                Planes = { new Plane(depth: 64) },

                // Tiles
                // Sectors
                // Zones
                // PlaneMaps
                // Things
                // Triggers
            };

            return map;
        }
    }
}
