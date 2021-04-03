﻿// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Tests.LevelGeometry.Mapping
{
    [TestFixture]
    public class LevelMapperTest
    {
        private MapData _data;
        private PlaneMap _planeMap;

        private Tile _tileAllWalls;

        private Tile _tileNorthWalls;
        private Tile _tileEastWalls;
        private Tile _tileSouthWalls;
        private Tile _tileWestWalls;

        private Tile _tileNorthWestWalls;
        private Tile _tileNorthEastWalls;
        private Tile _tileSouthWestWalls;
        private Tile _tileSouthEastWalls;

        private Tile _tileNorthSouthWalls;
        private Tile _tileEastWestWalls;

        private Tile _tileNorthEastSouthWalls;
        private Tile _tileEastSouthWestWalls;
        private Tile _tileSouthWestNorthWalls;
        private Tile _tileWestNorthEastWalls;

        [SetUp]
        public void Setup()
        {
            _data = DemoMap.Create();
            _planeMap = _data.PlaneMaps.Single();
            ClearMap();
            SetupTiles();
        }

        /// <remarks>
        /// Room Shape:
        /// X
        /// </remarks>
        [Test]
        public void ShouldFindOneByOneRoom()
        {
            AddStart(2, 2);
            AddSpace(2, 2, _tileAllWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);
            Assert.That(room.Locations.Count, Is.EqualTo(1));
            AssertLocationInRoom(room, 2, 2);
        }

        /// <remarks>
        /// Room Shape:
        /// XX
        /// XX
        /// </remarks>
        [Test]
        public void ShouldFindTwoByTwoRoom()
        {
            AddStart(2, 2);
            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthEastWalls);
            AddSpace(3, 3, _tileSouthEastWalls);
            AddSpace(2, 3, _tileSouthWestWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);
            Assert.That(room.Locations.Count, Is.EqualTo(4));
            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 2, 3);
            AssertLocationInRoom(room, 3, 3);
        }

        /// <remarks>
        /// Room Shape:
        /// XXX
        /// XNX
        /// XXX
        /// </remarks>
        [Test]
        public void ShouldHandleNullSpace()
        {
            AddStart(2, 2);

            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthWalls);
            AddSpace(4, 2, _tileNorthEastWalls);

            AddSpace(2, 3, _tileWestWalls);
            AddSpace(3, 3);
            AddSpace(4, 3, _tileEastWalls);

            AddSpace(2, 4, _tileSouthWestWalls);
            AddSpace(3, 4, _tileSouthWalls);
            AddSpace(4, 4, _tileSouthEastWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);
            Assert.That(room.Locations.Count, Is.EqualTo(9));

            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 4, 2);

            AssertLocationInRoom(room, 2, 3);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 4, 3);

            AssertLocationInRoom(room, 2, 4);
            AssertLocationInRoom(room, 3, 4);
            AssertLocationInRoom(room, 4, 4);
        }

        /// <remarks>
        /// Room Shape:
        /// XXXXX
        /// X   X 
        /// XXX X
        /// </remarks>
        [Test]
        public void ShouldFindPartialDonut()
        {
            AddStart(2, 2);
            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthSouthWalls);
            AddSpace(4, 2, _tileNorthSouthWalls);
            AddSpace(5, 2, _tileNorthSouthWalls);
            AddSpace(6, 2, _tileNorthEastWalls);

            AddSpace(2, 3, _tileEastWestWalls);
            AddSpace(6, 3, _tileEastWestWalls);

            AddSpace(2, 4, _tileSouthWestWalls);
            AddSpace(3, 4, _tileNorthSouthWalls);
            AddSpace(4, 4, _tileNorthEastSouthWalls);
            AddSpace(6, 4, _tileEastSouthWestWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);
            Assert.That(room.Locations.Count, Is.EqualTo(11));

            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 4, 2);
            AssertLocationInRoom(room, 5, 2);
            AssertLocationInRoom(room, 6, 2);

            AssertLocationInRoom(room, 2, 3);
            AssertLocationInRoom(room, 6, 3);

            AssertLocationInRoom(room, 2, 4);
            AssertLocationInRoom(room, 3, 4);
            AssertLocationInRoom(room, 4, 4);
            AssertLocationInRoom(room, 6, 4);
        }

        /// <remarks>
        /// Room Shape:
        /// XX
        /// XB
        /// </remarks>
        [Test]
        public void ShouldExcludeBlockingSpace()
        {
            AddStart(2, 2);
            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthEastWalls);
            AddSpace(3, 3, _tileSouthEastWalls);
            AddSpace(2, 3, _tileSouthWestWalls);
            AddBlocker(3, 3);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);
            Assert.That(room.Locations.Count, Is.EqualTo(3));
            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 2, 3);
        }

        /// <remarks>
        /// Room Shape:
        /// XXW
        /// XBW
        /// WWW
        /// </remarks>
        [Test]
        public void ShouldExcludeBlockingSpaceInNullTile()
        {
            AddStart(2, 2);

            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthWalls);
            AddSpace(4, 2, _tileAllWalls);

            AddSpace(2, 3, _tileWestWalls);
            AddSpace(3, 3);
            AddSpace(4, 3, _tileAllWalls);

            AddSpace(2, 4, _tileAllWalls);
            AddSpace(3, 4, _tileAllWalls);
            AddSpace(4, 4, _tileAllWalls);

            AddBlocker(3, 3);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);
            Assert.That(room.Locations.Count, Is.EqualTo(3));
            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 2, 3);
        }

        /// <remarks>
        /// Room Shape:
        /// WWW
        /// WNW
        /// WWW
        /// </remarks>
        [Test]
        public void ShouldBlockFromOutside()
        {
            AddStart(3, 3);

            AddSpace(2, 2, _tileAllWalls);
            AddSpace(3, 2, _tileAllWalls);
            AddSpace(4, 2, _tileAllWalls);

            AddSpace(2, 3, _tileAllWalls);
            AddSpace(3, 3);
            AddSpace(4, 3, _tileAllWalls);

            AddSpace(2, 4, _tileAllWalls);
            AddSpace(3, 4, _tileAllWalls);
            AddSpace(4, 4, _tileAllWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);
            Assert.That(room.Locations.Count, Is.EqualTo(1));

            AssertLocationInRoom(room, 3, 3);
        }

        /// <remarks>
        /// Room Shape:
        /// XX X
        /// XXDX
        /// </remarks>
        [Test]
        public void ShouldFindTwoConnectedRoomsWithADoor()
        {
            AddStart(2, 2);

            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthEastWalls);
            AddSpace(3, 3, _tileSouthWalls);
            AddSpace(2, 3, _tileSouthWestWalls);

            AddDoor(4, 3, true);

            AddSpace(5, 2, _tileWestNorthEastWalls);
            AddSpace(5, 3, _tileSouthEastWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);

            Assert.That(room.Locations.Count, Is.EqualTo(4));
            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 2, 3);
            AssertLocationInRoom(room, 3, 3);

            Assert.That(room.AdjacentRooms.Count, Is.EqualTo(1));

            var firstTransition = room.AdjacentRooms.First();

            Assert.That(firstTransition.Key.Count, Is.EqualTo(1));
            Assert.That(firstTransition.Value, Is.Not.Null);
            Assert.That(firstTransition.Value.Locations.Count, Is.EqualTo(2));
            AssertLocationInRoom(firstTransition.Value, 5, 2);
            AssertLocationInRoom(firstTransition.Value, 5, 3);
        }

        /// <remarks>
        /// Room Shape:
        /// XX   X
        /// XXDDDX
        /// </remarks>
        [Test]
        public void ShouldFindTwoConnectedRoomsWithMultipleDoors()
        {
            AddStart(2, 2);

            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthEastWalls);
            AddSpace(3, 3, _tileSouthWalls);
            AddSpace(2, 3, _tileSouthWestWalls);

            AddDoor(4, 3, true);
            AddDoor(5, 3, true);
            AddDoor(6, 3, true);

            AddSpace(7, 2, _tileWestNorthEastWalls);
            AddSpace(7, 3, _tileSouthEastWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);

            Assert.That(room.Locations.Count, Is.EqualTo(4));
            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 2, 3);
            AssertLocationInRoom(room, 3, 3);

            Assert.That(room.AdjacentRooms.Count, Is.EqualTo(1));

            var firstTransition = room.AdjacentRooms.First();

            Assert.That(firstTransition.Key.Count, Is.EqualTo(3));
            Assert.That(firstTransition.Value, Is.Not.Null);
            Assert.That(firstTransition.Value.Locations.Count, Is.EqualTo(2));
            AssertLocationInRoom(firstTransition.Value, 7, 2);
            AssertLocationInRoom(firstTransition.Value, 7, 3);
        }

        /// <remarks>
        /// Room Shape:
        /// XX X
        /// XXLX
        /// </remarks>
        [Test]
        public void ShouldFindOnlyOneConnectedRoomsWithALockedDoorAndNoKey()
        {
            AddStart(2, 2);

            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthEastWalls);
            AddSpace(3, 3, _tileSouthWalls);
            AddSpace(2, 3, _tileSouthWestWalls);

            AddDoor(4, 3, true, LockLevel.Silver);

            AddSpace(5, 2, _tileWestNorthEastWalls);
            AddSpace(5, 3, _tileSouthEastWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);

            Assert.That(room.Locations.Count, Is.EqualTo(4));
            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 2, 3);
            AssertLocationInRoom(room, 3, 3);

            Assert.That(room.AdjacentRooms.Count, Is.EqualTo(0));
        }

        /// <remarks>
        /// Room Shape:
        /// XX X
        /// XXLX
        /// </remarks>
        [Test]
        public void ShouldFindTwoConnectedRoomsWithALockedDoorWithKey()
        {
            AddStart(2, 2);

            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthEastWalls);
            AddKey(3, 2, Key.Silver);
            AddSpace(3, 3, _tileSouthWalls);
            AddSpace(2, 3, _tileSouthWestWalls);

            AddDoor(4, 3, true, LockLevel.Silver);

            AddSpace(5, 2, _tileWestNorthEastWalls);
            AddSpace(5, 3, _tileSouthEastWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);

            Assert.That(room.Locations.Count, Is.EqualTo(4));
            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 2, 3);
            AssertLocationInRoom(room, 3, 3);

            Assert.That(room.AdjacentRooms.Count, Is.EqualTo(1));

            var firstTransition = room.AdjacentRooms.First();

            Assert.That(firstTransition.Key.Count, Is.EqualTo(1));
            Assert.That(firstTransition.Value, Is.Not.Null);
            Assert.That(firstTransition.Value.Locations.Count, Is.EqualTo(2));
            AssertLocationInRoom(firstTransition.Value, 5, 2);
            AssertLocationInRoom(firstTransition.Value, 5, 3);
        }

        /// <remarks>
        /// Room Shape:
        /// XX X
        /// XXPX
        /// </remarks>
        [Test]
        public void ShouldFindTwoConnectedRoomsWithAPushwall()
        {
            AddStart(2, 2);

            AddSpace(2, 2, _tileNorthWestWalls);
            AddSpace(3, 2, _tileNorthEastWalls);
            AddSpace(3, 3, _tileSouthWalls);
            AddSpace(2, 3, _tileSouthWestWalls);

            AddPushwall(4, 3);

            AddSpace(5, 2, _tileWestNorthEastWalls);
            AddSpace(5, 3, _tileSouthEastWalls);

            var levelMap = new LevelMapper().Map(_data);
            var room = levelMap.StartingRoom;

            Assert.That(room, Is.Not.Null);

            Assert.That(room.Locations.Count, Is.EqualTo(4));
            AssertLocationInRoom(room, 2, 2);
            AssertLocationInRoom(room, 3, 2);
            AssertLocationInRoom(room, 2, 3);
            AssertLocationInRoom(room, 3, 3);

            Assert.That(room.AdjacentRooms.Count, Is.EqualTo(1));

            var firstTransition = room.AdjacentRooms.First();

            Assert.That(firstTransition.Key.Count, Is.EqualTo(1));
            Assert.That(firstTransition.Value, Is.Not.Null);
            Assert.That(firstTransition.Value.Locations.Count, Is.EqualTo(2));
            AssertLocationInRoom(firstTransition.Value, 5, 2);
            AssertLocationInRoom(firstTransition.Value, 5, 3);
        }

        private void AddStart(int x, int y)
        {
            var location = new MapLocation(_data, x, y);
            location.AddThing(Actor.Player1Start.ClassName);
        }

        private void AddSpace(int x, int y)
        {
            var location = new MapLocation(_data, x, y);
            location.Tile = null;
        }

        private void AddSpace(int x, int y, Tile tile)
        {
            var location = new MapLocation(_data, x, y);
            location.Tile = tile;
        }

        private void AddDoor(int x, int y, bool isEastWest, LockLevel lockLevel = LockLevel.None)
        {
            var location = new MapLocation(_data, x, y);
            location.Tile = _tileAllWalls;

            var doorTrigger = location.AddTrigger("Door_Open");
            doorTrigger.Arg3 = (int)lockLevel;
            if (!isEastWest)
                doorTrigger.Arg4 = 1;
        }

        private void AddKey(int x, int y, Key key)
        {
            var location = new MapLocation(_data, x, y);
            location.AddThing(key==Key.Silver ? Actor.SilverKey.ClassName : Actor.GoldKey.ClassName);
        }

        private void AddPushwall(int x, int y)
        {
            var location = new MapLocation(_data, x, y);
            location.Tile = _tileAllWalls;

            location.AddTrigger("Pushwall_Move");
        }

        private void AddBlocker(int x, int y)
        {
            var location = new MapLocation(_data, x, y);
            location.AddThing(Actor.TableWithChairs.ClassName);
        }

        private void ClearMap()
        {
            _planeMap.TileSpaces.ForEach(space => space.Tile = -1);
            _data.Things.Clear();
            _data.Triggers.Clear();
        }

        private void SetupTiles()
        {
            _tileAllWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1");
            _data.Tiles.Add(_tileAllWalls);

            _tileNorthWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", false, true, false, false);
            _data.Tiles.Add(_tileNorthWalls);

            _tileEastWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", true, false, false, false);
            _data.Tiles.Add(_tileEastWalls);

            _tileSouthWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", false, false, false, true);
            _data.Tiles.Add(_tileSouthWalls);

            _tileWestWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", false, false, true, false);
            _data.Tiles.Add(_tileWestWalls);

            _tileNorthEastWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", true, true, false, false);
            _data.Tiles.Add(_tileNorthEastWalls);

            _tileNorthWestWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", false, true, true, false);
            _data.Tiles.Add(_tileNorthWestWalls);

            _tileSouthEastWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", true, false, false, true);
            _data.Tiles.Add(_tileSouthEastWalls);

            _tileSouthWestWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", false, false, true, true);
            _data.Tiles.Add(_tileSouthWestWalls);

            _tileNorthSouthWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", false, true, false, true);
            _data.Tiles.Add(_tileNorthSouthWalls);

            _tileEastWestWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", true, false, true, false);
            _data.Tiles.Add(_tileEastWestWalls);

            _tileNorthEastSouthWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", true, true, false, true);
            _data.Tiles.Add(_tileNorthEastSouthWalls);

            _tileEastSouthWestWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", true, false, true, true);
            _data.Tiles.Add(_tileEastSouthWestWalls);

            _tileSouthWestNorthWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", false, true, true, true);
            _data.Tiles.Add(_tileSouthWestNorthWalls);

            _tileWestNorthEastWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1", true, true, true, false);
            _data.Tiles.Add(_tileWestNorthEastWalls);
        }

        private void AssertLocationInRoom(IRoom room, int x, int y)
        {
            Assert.That(room.Locations.Any(location => location.X == x && location.Y == y));
        }
    }
}