﻿// Copyright (c) 2017, Leon Organ and Aaron Alexander
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

        private static IDictionary<IList<Passage>, MapLocation> ExpandRoom(IRoom room, MapLocation fromLocation)
        {
            var passages = new Dictionary<IList<Passage>, MapLocation>();
           
            TryExpand(loc => loc.CanMoveNorth(), loc => loc.North(), room, fromLocation);
            TryExpand(loc => loc.CanMoveWest(), loc => loc.West(), room, fromLocation);
            TryExpand(loc => loc.CanMoveSouth(), loc => loc.South(), room, fromLocation);
            TryExpand(loc => loc.CanMoveEast(), loc => loc.East(), room, fromLocation);
            

            return passages;
        }

        private static void TryPassage(
            Func<MapLocation, MapLocation> getNext,
            Func<Trigger, bool> hasProperFacingDoor,
            Func<MapLocation, bool> moveBackCheck,
            MapLocation fromLocation, 
            Dictionary<IList<Passage>, MapLocation> passages)
        {
            var passageTrail = new List<Passage>();

            do
            { 
                var targetPassageSpace = getNext(fromLocation);

                if (targetPassageSpace == null)
                    return;

                var targetPassage = targetPassageSpace.Actions.FirstOrDefault(hasProperFacingDoor);
           
                if (targetPassage == null)
                    return;

                passageTrail.Add(new Passage(targetPassageSpace));

                var targetLocation = getNext(targetPassageSpace);

                if (targetLocation == null)
                    return;

                if (targetLocation.HasBlocker())
                    return;

                if (moveBackCheck(targetLocation))
                {
                    passages.Add(passageTrail, targetLocation);
                    return;
                }

                fromLocation = targetPassageSpace;

            } while (true);

        }

        private static void TryExpand(Func<MapLocation, bool> moveCheck, Func<MapLocation, MapLocation> targetTile, IRoom room, MapLocation fromLocation)
        {
            if (moveCheck(fromLocation))
            {
                var targetSpace = targetTile(fromLocation);
                if (!room.Locations.Contains(targetSpace))
                {
                    room.Locations.Add(targetSpace);
                    ExpandRoom(room, targetSpace);
                }
            }
        }


    }
}