// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using System.Drawing;
using System.IO;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.FormatModels.MapText
{
    public static class MapTextExporter
    {
        public static void Export(MapData map, string outputFilePath)
        {
            var allDoors =
                map.Triggers
                .Where(t => t.Action == ActionSpecial.DoorOpen || t.Action == ActionSpecial.PushwallMove)
                .Select(t => new Point(t.X, t.Y))
                .ToImmutableHashSet();

            string DetermineCharacter(int row, int col)
            {
                var index = row * map.Width + col;

                var space = map.PlaneMaps[0].TileSpaces[index];
                if (!space.HasTile)
                    return " ";

                return allDoors.Contains(new Point(col, row)) ? "□" : "█";
            }

            using (var writer = File.CreateText(outputFilePath))
            {
                for (int rowIndex = 0; rowIndex < map.Height; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < map.Width; colIndex++)
                    {
                        writer.Write(DetermineCharacter(rowIndex, colIndex));
                    }
                    if (rowIndex < map.Height - 1)
                        writer.WriteLine();
                }

                writer.Flush();
            }
        }
    }
}