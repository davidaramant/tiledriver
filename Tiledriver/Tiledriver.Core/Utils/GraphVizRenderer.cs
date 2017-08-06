// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using System.Text;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Utils
{
    public static class GraphVizRenderer
    {
        public static string BuildGraphDefinition(MapData data)
        {
            var mapper = new LevelMapper();
            var levelMap = mapper.Map(data);

            return BuildGraphDefinition(data, levelMap);
        }

        public static string BuildGraphDefinition(MapData data, LevelMap levelMap)
        {
            var graphDefinition = ProduceGraphs(data, levelMap);

            return graphDefinition;
        }

        private static string ProduceGraphs(MapData map, LevelMap levelMap)
        {
            var maximumLocationSize = levelMap.AllRooms.Max(room => room.Locations.Count);
            var minimumSize = 1.0;
            var maximumSize = 15.0;

            var contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("graph {");
            contentBuilder.AppendLine($"label=\"{map.Name}\";");

            contentBuilder.AppendLine("{");

            foreach (var room in levelMap.AllRooms)
            {
                contentBuilder.Append($"\"{room.Name}\" [");
                if (room == levelMap.StartingRoom)
                {
                    contentBuilder.Append("color=yellow, style=filled, ");
                }
                else if (levelMap.EndingRooms.Contains(room))
                {
                    contentBuilder.Append("color=orange, style=filled, ");
                }

                var roomSize = (double)room.Locations.Count / maximumLocationSize * maximumSize;
                if (roomSize < minimumSize)
                    roomSize = minimumSize;
                contentBuilder.AppendLine($"shape=circle, width={roomSize:F2}, fontsize=22];");
            }

            contentBuilder.AppendLine("}");

            foreach (var room in levelMap.AllRooms)
            {
                foreach (var passageRoomPair in room.AdjacentRooms)
                {
                    contentBuilder.Append($"\"{room.Name}\" -- \"{passageRoomPair.Value.Name}\"");

                    if (passageRoomPair.Key.Any(passage => passage.LockLevel == LockLevel.Gold))
                        contentBuilder.Append(" [color=gold]");
                    if (passageRoomPair.Key.Any(passage => passage.LockLevel == LockLevel.Silver))
                        contentBuilder.Append(" [color=silver]");
                    if (passageRoomPair.Key.Any(passage => passage.LockLevel == LockLevel.Both))
                        contentBuilder.Append(" [color=cyan]");

                    contentBuilder.AppendLine(";");
                }
            }

            contentBuilder.AppendLine("}");

            return contentBuilder.ToString();
        }
    }
}