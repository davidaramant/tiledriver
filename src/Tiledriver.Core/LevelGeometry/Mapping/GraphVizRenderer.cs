// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.GameInfo.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping;

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

		var roomNameMap = new Dictionary<IRoom, string>();

		foreach (var room in levelMap.AllRooms)
		{
			var isStartingRoom = room == levelMap.StartingRoom;
			var isEndingRoom = levelMap.EndingRooms.Contains(room);

			var roomName = NameRoom(room, isStartingRoom, isEndingRoom, roomNameMap);

			contentBuilder.Append($"\"{roomName}\" [");
			if (isStartingRoom)
			{
				contentBuilder.Append("color=palegreen, style=filled, ");
			}
			else if (isEndingRoom)
			{
				contentBuilder.Append("color=tomato, style=filled, ");
			}
			else if (room.HasGoldKey && room.HasSilverKey)
			{
				contentBuilder.Append("color=cyan, style=filled, ");
			}
			else if (room.HasGoldKey)
			{
				contentBuilder.Append("color=gold, style=filled, ");
			}
			else if (room.HasSilverKey)
			{
				contentBuilder.Append("color=azure3, style=filled, ");
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
				var leftRoomName = roomNameMap[room];
				var rightRoomName = roomNameMap[passageRoomPair.Value];

				contentBuilder.Append($"\"{leftRoomName}\" -- \"{rightRoomName}\"");

				if (passageRoomPair.Key.Any(passage => passage.LockLevel == LockLevel.Gold))
					contentBuilder.Append(" [color=gold, penwidth=5]");
				if (passageRoomPair.Key.Any(passage => passage.LockLevel == LockLevel.Silver))
					contentBuilder.Append(" [color=azure3, penwidth=5]");
				if (passageRoomPair.Key.Any(passage => passage.LockLevel == LockLevel.Both))
					contentBuilder.Append(" [color=cyan, penwidth=5]");

				contentBuilder.AppendLine(";");
			}
		}

		contentBuilder.AppendLine("}");

		return contentBuilder.ToString();
	}

	private static string NameRoom(
		IRoom room,
		bool isStartingRoom,
		bool isEndingRoom,
		Dictionary<IRoom, string> roomNameMap
	)
	{
		var roomName = room.Name;

		var specialFlags = new List<string>();
		if (isStartingRoom)
			specialFlags.Add("Start");
		if (isEndingRoom)
			specialFlags.Add("End");
		if (room.HasGoldKey)
			specialFlags.Add("Gold");
		if (room.HasSilverKey)
			specialFlags.Add("Silver");

		if (specialFlags.Any())
		{
			roomName += "\n(";
			roomName += String.Join(",\n", specialFlags);
			roomName += ")";
		}

		roomNameMap.Add(room, roomName);
		return roomName;
	}
}
