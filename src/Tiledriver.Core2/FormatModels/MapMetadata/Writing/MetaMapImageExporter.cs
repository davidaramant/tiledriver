// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;
using System.Linq;
using static HsluvS.Hsluv;
using Tiledriver.Core.Utils;

namespace Tiledriver.Core.FormatModels.MapMetadata.Writing
{
    public sealed class MetaMapImageExporter
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
                var (r,g,b) = HslToRgb((hue,100,50));
                var color = Color.FromArgb((int)(255*r),(int)(255*g),(int)(255*b));

                foreach (var spot in room)
                {
                    image.SetPixel(spot, color);
                }
            }

            image.Save(outputFilePath);
        }
    }
}