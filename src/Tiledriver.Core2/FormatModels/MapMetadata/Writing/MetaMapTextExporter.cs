// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;

namespace Tiledriver.Core.FormatModels.MapMetadata.Writing
{
    public static class MetaMapTextExporter
    {
        public static void Export(MetaMap map, string outputFilePath, bool unreachableIsSolid = true)
        {
            using var fs = File.CreateText(outputFilePath);
            Export(map, fs, unreachableIsSolid);
        }

        public static void Export(MetaMap map, TextWriter writer, bool unreachableIsSolid = true)
        {
            string DetermineCharacter(int x, int y) =>
                map[x, y] switch
                {
                    TileType.Empty => " ",
                    TileType.Wall => "█",
                    TileType.Door => "□",
                    TileType.PushWall => "□",
                    TileType.Unreachable => unreachableIsSolid ? "█" : " ",
                    _ => "!"
                };

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    writer.Write(DetermineCharacter(x, y));
                }
                if (y < map.Height - 1)
                    writer.WriteLine();
            }

            writer.Flush();
        }
    }
}