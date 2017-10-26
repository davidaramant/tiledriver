// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.Utils;

namespace Tiledriver.Core.FormatModels.SimpleMapImage
{
    public sealed class SimpleMapImageExporter
    {
        public static void Export(MetaMap map, MapPalette palette, string outputFilePath)
        {
            var image = new FastImage(map.Width, map.Height);
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    image.SetPixel(x, y, palette.PickColor(map[x, y]));
                }
            }
            image.Save(outputFilePath);
        }
    }
}