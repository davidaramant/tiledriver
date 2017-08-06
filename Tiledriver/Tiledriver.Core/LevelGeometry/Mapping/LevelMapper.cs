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
        private List<Thing> _silverLocations;
        private List<Thing> _goldLocations;
        private bool _hasSilver;
        private bool _hasGold;
        private IList<IRoom> _discoveredRooms = new List<IRoom>();
        private IList<LockedWay> _lockedWays = new List<LockedWay>();

        public LevelMap Map(MapData data)
        {
            _hasSilver = false;
            _hasGold = false;
            _discoveredRooms.Clear();

            _silverLocations = data.Things.Where(t=>t.Type == Actor.SilverKey.ClassName).ToList();
            _goldLocations = data.Things.Where(t => t.Type == Actor.GoldKey.ClassName).ToList();
            _goldLocations.AddRange(data.Things.Where(t => t.Type == Actor.Hans.ClassName).ToList());
            _goldLocations.AddRange(data.Things.Where(t => t.Type == Actor.Gretel.ClassName).ToList());

            var startPosition = FindStart(data);

            var startingRoom = MapRoom(startPosition);

            int keysFound;

            do
            {
                keysFound = CountKeys();
                AttemptLocks();
            } while (keysFound < CountKeys());

            foreach (var lockedWay in _lockedWays)
            {
                lockedWay.FromRoom.UnopenableDoors++;
            }

            return new LevelMap(startingRoom, _discoveredRooms);
        }

        private int CountKeys()
        {
            return (_hasGold ? 1 : 0) + (_hasSilver ? 1 : 0);
        }

        private MapLocation FindStart(MapData data)
        {
            var startThing = data.Things.Single(thing => thing.Type == Actor.Player1Start.ClassName);

            return new MapLocation(data, (int)Math.Floor(startThing.X), (int)Math.Floor(startThing.Y));
        }

        private IRoom MapRoom(MapLocation firstLocation)
        {
            var newRoom = new Room(_discoveredRooms.Count + 1);
            _discoveredRooms.Add(newRoom);

            newRoom.Locations.Add(firstLocation);

            var locationsToProcess = new List<MapLocation> {firstLocation};

            ExpandRoom(newRoom, locationsToProcess);

            var passages = FindPassages(newRoom);

            ExplorePassages(passages, newRoom);

            return newRoom;
        }

        private void ExpandRoom(IRoom room, List<MapLocation> locationsToProcess)
        {
            while (locationsToProcess.Count > 0)
            {
                var fromLocation = locationsToProcess[0];
                locationsToProcess.RemoveAt(0);

                var n = TryExpand(loc => loc.CanMoveNorth(), loc => loc.North(), room, fromLocation, locationsToProcess);
                var w = TryExpand(loc => loc.CanMoveWest(), loc => loc.West(), room, fromLocation, locationsToProcess);
                var s = TryExpand(loc => loc.CanMoveSouth(), loc => loc.South(), room, fromLocation, locationsToProcess);
                var e= TryExpand(loc => loc.CanMoveEast(), loc => loc.East(), room, fromLocation, locationsToProcess);

                fromLocation.Cooridor = (n && s && !e && !w) || (!n && !s && e && w);
            }

            _hasGold |= room.Locations.Any(loc => _goldLocations.Any(key=>(int)key.X == loc.X && (int)key.Y==loc.Y));
            _hasSilver |= room.Locations.Any(loc => _silverLocations.Any(key => (int)key.X == loc.X && (int)key.Y == loc.Y));
        }

        private bool TryExpand(Func<MapLocation, bool> moveCheck, Func<MapLocation, MapLocation> targetTile, IRoom room, MapLocation fromLocation, List<MapLocation> locationsToProcess)
        {
            if (moveCheck(fromLocation))
            {
                var targetSpace = targetTile(fromLocation);
                if (!room.Locations.Contains(targetSpace))
                {
                    room.Locations.Add(targetSpace);
                    locationsToProcess.Add(targetSpace);
                }
                return true;
            }

            return false;
        }

        private Dictionary<IList<Passage>, MapLocation> FindPassages(IRoom room)
        {
            var passages = new Dictionary<IList<Passage>, MapLocation>();

            var isEWDoor = new Func< Trigger, bool>(t => t.Action == "Door_Open" && t.Arg4 == 0);
            var isNSDoor = new Func<Trigger, bool>(t => t.Action == "Door_Open" && t.Arg4 == 1);
            var isPushWall = new Func<Trigger, bool>(t => t.Action == "Pushwall_Move");

            foreach (var location in room.Locations)
            {
                TryPassage(loc => loc.North(), t => isNSDoor(t) || isPushWall(t),
                    loc => loc.Tile == null || !loc.Tile.BlockingSouth, location, room, passages);
                TryPassage(loc => loc.West(), t => isEWDoor(t) || isPushWall(t),
                    loc => loc.Tile == null || !loc.Tile.BlockingEast, location, room, passages);
                TryPassage(loc => loc.South(), t => isNSDoor(t) || isPushWall(t),
                    loc => loc.Tile == null || !loc.Tile.BlockingNorth, location, room, passages);
                TryPassage(loc => loc.East(), t => isEWDoor(t) || isPushWall(t),
                    loc => loc.Tile == null || !loc.Tile.BlockingWest, location, room, passages);
            }
            return passages;
        }

        private void TryPassage(
            Func<MapLocation, MapLocation> getNext,
            Func<Trigger, bool> hasProperFormattedPassage,
            Func<MapLocation, bool> moveBackCheck,
            MapLocation fromLocation, 
            IRoom fromRoom,
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
                            if (!_hasSilver)
                            {
                                _lockedWays.Add(new LockedWay((LockLevel)targetPassage.Arg3, getNext, hasProperFormattedPassage, moveBackCheck, fromLocation, fromRoom));
                                return;
                            }
                            break;
                        case LockLevel.Gold:
                            if (!_hasGold)
                            {
                                _lockedWays.Add(new LockedWay((LockLevel)targetPassage.Arg3, getNext, hasProperFormattedPassage, moveBackCheck, fromLocation, fromRoom));
                                return;
                            }
                            break;
                        case LockLevel.Both:
                            if (!_hasSilver || !_hasGold)
                            {
                                _lockedWays.Add(new LockedWay((LockLevel)targetPassage.Arg3, getNext, hasProperFormattedPassage, moveBackCheck, fromLocation, fromRoom));
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

        private void ExplorePassages(Dictionary<IList<Passage>, MapLocation> passages, IRoom room)
        {
            foreach (var passage in passages)
            {
                var existingRoom = _discoveredRooms.FirstOrDefault(
                    r => r.Locations.Any(loc => loc.X == passage.Value.X && loc.Y == passage.Value.Y));
                if (existingRoom != null)
                {
                    if (room != existingRoom)
                    {
                        room.AdjacentRooms.Add(passage.Key, existingRoom);
                    }
                }
                else
                {
                    room.AdjacentRooms.Add(passage.Key, MapRoom(passage.Value));
                }
            }
        }

        private void AttemptLocks()
        {
            foreach (var lockedWay in _lockedWays.ToArray())
            {
                switch (lockedWay.LockLevel)
                {
                    case LockLevel.Silver:
                        if (_hasSilver)
                        {
                            _lockedWays.Remove(lockedWay);
                            Dictionary<IList<Passage>, MapLocation> passages = new Dictionary<IList<Passage>, MapLocation>();
                            TryPassage(lockedWay.GetNext, lockedWay.HasProperFormattedPassage, lockedWay.MoveBackCheck, lockedWay.FromLocation, lockedWay.FromRoom, passages);
                            ExplorePassages(passages, lockedWay.FromRoom);
                        }
                        break;
                    case LockLevel.Gold:
                        if (_hasGold)
                        {
                            _lockedWays.Remove(lockedWay);
                            Dictionary<IList<Passage>, MapLocation> passages = new Dictionary<IList<Passage>, MapLocation>();
                            TryPassage(lockedWay.GetNext, lockedWay.HasProperFormattedPassage, lockedWay.MoveBackCheck, lockedWay.FromLocation, lockedWay.FromRoom, passages);
                            ExplorePassages(passages, lockedWay.FromRoom);
                        }
                        break;
                    case LockLevel.Both:
                        if (_hasSilver || _hasGold)
                        {
                            _lockedWays.Remove(lockedWay);
                            Dictionary<IList<Passage>, MapLocation> passages = new Dictionary<IList<Passage>, MapLocation>();
                            TryPassage(lockedWay.GetNext, lockedWay.HasProperFormattedPassage, lockedWay.MoveBackCheck, lockedWay.FromLocation, lockedWay.FromRoom, passages);
                            ExplorePassages(passages, lockedWay.FromRoom);
                        }
                        break;
                }
            }
        }

        private class LockedWay
        {
            public LockLevel LockLevel { get; }
            public Func<MapLocation, MapLocation> GetNext { get; }
            public Func<Trigger, bool> HasProperFormattedPassage { get; }
            public Func<MapLocation, bool> MoveBackCheck { get; }
            public MapLocation FromLocation { get; }
            public IRoom FromRoom { get; }

            public LockedWay(LockLevel lockLevel, Func<MapLocation, MapLocation> getNext, Func<Trigger, bool> hasProperFormattedPassage, Func<MapLocation, bool> moveBackCheck, MapLocation fromLocation, IRoom fromRoom)
            {
                LockLevel = lockLevel;
                GetNext = getNext;
                HasProperFormattedPassage = hasProperFormattedPassage;
                MoveBackCheck = moveBackCheck;
                FromLocation = fromLocation;
                FromRoom = fromRoom;
            }
        }
    }
}
