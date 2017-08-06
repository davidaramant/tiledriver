// Copyright (c) 2017, Leon Organ and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    public class LevelMapper
    {
        private IList<Thing> silverLocations;
        private IList<Thing> goldLocations;
        private bool hasSilver;
        private bool hasGold;
        private IList<IRoom> discoveredRooms = new List<IRoom>();
        private IList<IList<Passage>> lockedWays = new List<IList<Passage>>();

        public LevelMap Map(MapData data)
        {
            hasSilver = false;
            hasGold = false;
            discoveredRooms.Clear();

            silverLocations = data.Things.Where(t=>t.Type == Actor.SilverKey.ClassName).ToList();
            goldLocations = data.Things.Where(t => t.Type == Actor.GoldKey.ClassName).ToList();

            var startPosition = FindStart(data);

            var startingRoom = MapRoom(startPosition);

            return new LevelMap(startingRoom, discoveredRooms);
        }

        private MapLocation FindStart(MapData data)
        {
            var startThing = data.Things.Single(thing => thing.Type == Actor.Player1Start.ClassName);

            return new MapLocation(data, (int)Math.Floor(startThing.X), (int)Math.Floor(startThing.Y));
        }

        private IRoom MapRoom(MapLocation firstLocation)
        {
            var newRoom = new Room(discoveredRooms.Count + 1);
            discoveredRooms.Add(newRoom);

            newRoom.Locations.Add(firstLocation);

            ExpandRoom(newRoom, firstLocation);

            var passages = FindPassages(newRoom);

            ExplorePassages(passages, newRoom);

            return newRoom;
        }

        private void ExpandRoom(IRoom room, MapLocation fromLocation)
        {
            TryExpand(loc => loc.CanMoveNorth(), loc => loc.North(), room, fromLocation);
            TryExpand(loc => loc.CanMoveWest(), loc => loc.West(), room, fromLocation);
            TryExpand(loc => loc.CanMoveSouth(), loc => loc.South(), room, fromLocation);
            TryExpand(loc => loc.CanMoveEast(), loc => loc.East(), room, fromLocation);

            hasGold |= room.Locations.Any(loc => goldLocations.Any(key=>(int)key.X == loc.X && (int)key.Y==loc.Y));
            hasSilver |= room.Locations.Any(loc => silverLocations.Any(key => (int)key.X == loc.X && (int)key.Y == loc.Y));
        }

        private void TryExpand(Func<MapLocation, bool> moveCheck, Func<MapLocation, MapLocation> targetTile, IRoom room, MapLocation fromLocation)
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

        private Dictionary<IList<Passage>, MapLocation> FindPassages(Room newRoom)
        {
            var passages = new Dictionary<IList<Passage>, MapLocation>();

            var isEWDoor = new Func< Trigger, bool>(t => t.Action == "Door_Open" && t.Arg4 == 0);
            var isNSDoor = new Func<Trigger, bool>(t => t.Action == "Door_Open" && t.Arg4 == 1);
            var isPushWall = new Func<Trigger, bool>(t => t.Action == "Pushwall_Move");

            foreach (var location in newRoom.Locations)
            {
                TryPassage(loc => loc.North(), t => isNSDoor(t) || isPushWall(t),
                    loc => loc.Tile == null || !loc.Tile.BlockingSouth, location, passages);
                TryPassage(loc => loc.West(), t => isEWDoor(t) || isPushWall(t),
                    loc => loc.Tile == null || !loc.Tile.BlockingEast, location, passages);
                TryPassage(loc => loc.South(), t => isNSDoor(t) || isPushWall(t),
                    loc => loc.Tile == null || !loc.Tile.BlockingNorth, location, passages);
                TryPassage(loc => loc.East(), t => isEWDoor(t) || isPushWall(t),
                    loc => loc.Tile == null || !loc.Tile.BlockingWest, location, passages);
            }
            return passages;
        }

        private void TryPassage(
            Func<MapLocation, MapLocation> getNext,
            Func<Trigger, bool> hasProperFormattedPassage,
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

                var targetPassage = targetPassageSpace.Triggers.FirstOrDefault(hasProperFormattedPassage);
           
                if (targetPassage == null)
                    return;

                if (targetPassage.Action == "Door_Open")
                {
                    switch ((LockLevel) targetPassage.Arg3)
                    {
                        case LockLevel.Silver:
                            if (!hasSilver)
                            {
                                lockedWays.Add(passageTrail);
                                return;
                            }
                            break;
                        case LockLevel.Gold:
                            if (!hasGold)
                            {
                                lockedWays.Add(passageTrail);
                                return;
                            }
                            break;
                        case LockLevel.Both:
                            if (!hasSilver || !hasGold)
                            {
                                lockedWays.Add(passageTrail);
                                return;
                            }
                            break;
                    }
                }

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

        private void ExplorePassages(Dictionary<IList<Passage>, MapLocation> passages, Room newRoom)
        {
            foreach (var passage in passages)
            {
                var existingRoom = discoveredRooms.FirstOrDefault(
                    r => r.Locations.Any(loc => loc.X == passage.Value.X && loc.Y == passage.Value.Y));
                if (existingRoom != null)
                {
                    if (newRoom != existingRoom)
                    {
                        newRoom.AdjacentRooms.Add(passage.Key, existingRoom);
                    }
                }
                else
                {
                    newRoom.AdjacentRooms.Add(passage.Key, MapRoom(passage.Value));
                }
            }
        }

    }
}
