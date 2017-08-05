// Copyright (c) 2017, Leon Organ and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    public static class LevelMapper
    {
        public static IRoom Map(MapData data)
        {
            var discoveredRooms = new List<IRoom>();

            var startPosition = FindStart(data);

            return MapRoom(discoveredRooms, startPosition);
        }

        private static MapLocation FindStart(MapData data)
        {
            var startThing = data.Things.Single(thing => thing.Type == Actor.Player1Start.ClassName);

            return new MapLocation(data, (int)Math.Floor(startThing.X), (int)Math.Floor(startThing.Y));
        }

        private static IRoom MapRoom(IList<IRoom> discoveredRooms, MapLocation firstLocation)
        {
            var newRoom = new Room();

            newRoom.Locations.Add(firstLocation);

            var passages = ExpandRoom(newRoom, firstLocation);

            
            foreach (var passage in passages)
            {
                var existingRoom = discoveredRooms.FirstOrDefault(
                    r => r.Locations.Any(loc => loc.X == passage.Value.X && loc.Y == passage.Value.Y));
                if ( existingRoom != null )
                {
                    if (newRoom != existingRoom)
                    {
                        newRoom.AdjacentRooms.Add(passage.Key, existingRoom);
                    }
                }
                else
                {
                    newRoom.AdjacentRooms.Add(passage.Key, MapRoom(discoveredRooms, passage.Value));
                }
            }

            return newRoom;
        }

        private static IDictionary<IPassage, MapLocation> ExpandRoom(IRoom room, MapLocation fromLocation)
        {
            if (fromLocation.CanMoveNorth())
            {
                var targetSpace = fromLocation.North();
                if (!room.Locations.Contains(targetSpace))
                {
                    room.Locations.Add(targetSpace);
                    ExpandRoom(room, targetSpace);
                }
            }
            if (fromLocation.CanMoveWest())
            {
                var targetSpace = fromLocation.West();
                if (!room.Locations.Contains(targetSpace))
                {
                    room.Locations.Add(targetSpace);
                    ExpandRoom(room, targetSpace);
                }
            }
            if (fromLocation.CanMoveSouth())
            {
                var targetSpace = fromLocation.South();
                if (!room.Locations.Contains(targetSpace))
                {
                    room.Locations.Add(targetSpace);
                    ExpandRoom(room, targetSpace);
                }
            }
            if (fromLocation.CanMoveEast())
            {
                var targetSpace = fromLocation.East();
                if (!room.Locations.Contains(targetSpace))
                {
                    room.Locations.Add(targetSpace);
                    ExpandRoom(room, targetSpace);
                }
            }

            return new Dictionary<IPassage, MapLocation>();
        }

        
    }
}
