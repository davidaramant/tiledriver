// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Linq;
using SkiaSharp;
using static HsluvS.Hsluv;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.FormatModels.MapMetadata.Writing
{
    public sealed class MetaMapImageExporter
    {
        public static void Export(MetaMap map, MapPalette palette, string outputFilePath, int scale = 1)
        {
            using var image = new FastImage(map.Width, map.Height, scale);
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

        public static void Export(RoomGraph graph, string outputFilePath, int scale = 1)
        {
            using var image = new FastImage(graph.Width, graph.Height, scale);
            image.Fill(SKColors.Black);

            var increment = 360.0 / graph.RoomCount;

            foreach (var (room, index) in graph.Select((room, index) => (room, index)))
            {
                var hue = index * increment;
                var (r, g, b) = HslToRgb((hue, 100, 50));
                var color = new SKColor((byte) (255 * r), (byte) (255 * g), (byte) (255 * b));

                foreach (var spot in room)
                {
                    image.SetPixel(spot.X, spot.Y, color);
                }
            }

            image.Save(outputFilePath);
        }
    }
}