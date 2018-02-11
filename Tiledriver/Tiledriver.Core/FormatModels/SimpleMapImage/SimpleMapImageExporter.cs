// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;
using System.Linq;
using ColorMine.ColorSpaces;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.Utils;

namespace Tiledriver.Core.FormatModels.SimpleMapImage
{
    public sealed class SimpleMapImageExporter
    {
        public static void Export(MetaMap map, MapPalette palette, string outputFilePath, uint scale = 1)
        {
            var image = new ScaledFastImage(map.Width, map.Height, scale);
            for (var tileY = 0; tileY < map.Height; tileY++)
            {
                for (var tileX = 0; tileX < map.Width; tileX++)
                {
                    var tileColor = palette.PickColor(map[tileX, tileY]);
                    image.SetPixel(tileX, tileY, tileColor);
                }
            }
            image.Save(outputFilePath);
        }

        public static void Export(RoomGraph graph, string outputFilePath, uint scale = 1)
        {
            var image = new ScaledFastImage(graph.Width, graph.Height, scale);
            image.Fill(Color.Black);

            var increment = 360.0 / graph.RoomCount;

            foreach (var (room, index) in graph.Select((room, index) => (room, index)))
            {
                var hue = index * increment;
                var hsvColor = new Hsv(hue, 1, 1);
                var rgb = hsvColor.ToRgb();
                var color = Color.FromArgb((int)rgb.R, (int)rgb.G, (int)rgb.B);

                foreach (var spot in room)
                {
                    image.SetPixel(spot, color);
                }
            }

            image.Save(outputFilePath);
        }
    }
}