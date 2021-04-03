// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using Tiledriver.Core.FormatModels.MapMetadata;

namespace Tiledriver.Core.FormatModels.SimpleMapText
{
    public static class SimpleMapTextExporter
    {
        public static void Export(MetaMap map, string outputFilePath, bool unreachableIsSolid = true)
        {
            using (var fs = File.CreateText(outputFilePath))
            {
                Export(map, fs, unreachableIsSolid);
            }
        }

        public static void Export(MetaMap map, TextWriter writer, bool unreachableIsSolid = true)
        {
            string DetermineCharacter(int x, int y)
            {
                switch (map[x, y])
                {
                    case TileType.Empty:
                        return " ";
                    case TileType.Wall:
                        return "█";
                    case TileType.Door:
                    case TileType.PushWall:
                        return "□";
                    case TileType.Unreachable:
                        return unreachableIsSolid ? "█" : " ";
                    default:
                        return "!";
                }
            }

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