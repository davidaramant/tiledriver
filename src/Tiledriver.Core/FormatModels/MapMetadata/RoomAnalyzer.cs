// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.MapMetadata.Extensions;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.MapMetadata
{
    public static class RoomAnalyzer
    {
        public static RoomGraph Analyze(MetaMap metaMap)
        {
            var roomMap = new int[metaMap.Height, metaMap.Width];
            int roomCount = 0;

            // real ugly, but whatever
            var playableSpaces = GetAllPlayableSpaces(metaMap).ToArray();

            foreach (var emptySpot in playableSpaces)
            {
                roomMap[emptySpot.Y, emptySpot.X] = ++roomCount;
            }

            void ReplaceAll(int oldId, int newId)
            {
                for (int row = 0; row < metaMap.Height; row++)
                {
                    for (int col = 0; col < metaMap.Width; col++)
                    {
                        if (roomMap[row, col] == oldId)
                        {
                            roomMap[row, col] = newId;
                        }
                    }
                }
            }

            foreach (var spot in playableSpaces)
            {
                var currentRoomId = roomMap[spot.Y, spot.X];

                int JoinSpots(Position p1, Position p2)
                {
                    var id1 = roomMap[p1.Y, p1.X];
                    var id2 = roomMap[p2.Y, p2.X];

                    ReplaceAll(Math.Max(id1, id2), Math.Min(id1, id2));

                    return Math.Min(id1, id2);
                }

                // left
                if (spot.X > 0 && roomMap[spot.Y, spot.X - 1] != currentRoomId && roomMap[spot.Y, spot.X - 1] != 0)
                {
                    currentRoomId = JoinSpots(spot, spot.Left());
                }
                // right
                if (spot.X < metaMap.Width - 1 && roomMap[spot.Y, spot.X + 1] != currentRoomId && roomMap[spot.Y, spot.X + 1] != 0)
                {
                    currentRoomId = JoinSpots(spot, spot.Right());
                }
                // top
                if (spot.Y > 0 && roomMap[spot.Y - 1, spot.X] != currentRoomId && roomMap[spot.Y - 1, spot.X] != 0)
                {
                    currentRoomId = JoinSpots(spot, spot.Above());
                }
                // bottom
                if (spot.Y < metaMap.Height - 1 && roomMap[spot.Y + 1, spot.X] != currentRoomId && roomMap[spot.Y + 1, spot.X] != 0)
                {
                    currentRoomId = JoinSpots(spot, spot.Below());
                }
            }

            var roomIds = new HashSet<int>();
            for (int row = 0; row < metaMap.Height; row++)
            {
                for (int col = 0; col < metaMap.Width; col++)
                {
                    var id = roomMap[row, col];
                    if (id > 0)
                    {
                        roomIds.Add(id);
                    }
                }
            }

            return new RoomGraph(
                metaMap.Width,
                metaMap.Height,
                roomIds.Select(id => new Room(GetAllSpacesWithId(roomMap, id))));
        }

        private static IEnumerable<Position> GetAllPlayableSpaces(MetaMap metaMap)
        {
            for (int row = 0; row < metaMap.Height; row++)
            {
                for (int col = 0; col < metaMap.Width; col++)
                {
                    if (metaMap[col, row] == TileType.Empty)
                    {
                        yield return new Position(col, row);
                    }
                }
            }
        }

        private static IEnumerable<Position> GetAllSpacesWithId(int[,] roomMap, int id)
        {
            for (int row = 0; row < roomMap.GetLength(0); row++)
            {
                for (int col = 0; col < roomMap.GetLength(1); col++)
                {
                    if (roomMap[row, col] == id)
                    {
                        yield return new Position(col, row);
                    }
                }
            }
        }
    }
}