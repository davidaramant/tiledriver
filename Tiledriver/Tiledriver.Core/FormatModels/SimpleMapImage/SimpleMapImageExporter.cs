// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.Utils;

namespace Tiledriver.Core.FormatModels.SimpleMapImage
{
    public sealed class SimpleMapImageExporter
    {
        public static void Export(MetaMap map, MapPalette palette, string outputFilePath, uint scale = 1)
        {
            var width = scale * map.Width;
            var height = scale * map.Height;

            var image = new FastImage((int)width, (int)height);
            for (var tileY = 0; tileY < map.Height; tileY++)
            {
                for (var tileX = 0; tileX < map.Width; tileX++)
                {
                    var tileColor = palette.PickColor(map[tileX, tileY]);
                    for (int pixelY = tileY * (int)scale; pixelY < (tileY + 1) * scale; pixelY++)
                    {
                        for (int pixelX = tileX * (int)scale; pixelX < (tileX + 1) * scale; pixelX++)
                        {
                            image.SetPixel(pixelX, pixelY, tileColor);
                        }
                    }
                }
            }
            image.Save(outputFilePath);
        }
    }
}