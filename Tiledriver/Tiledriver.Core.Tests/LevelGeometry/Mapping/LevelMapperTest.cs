// Copyright (c) 2017, Aaron Alexander
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

        [SetUp]
        public void Setup()
        {
            _data = DemoMap.Create();
            _planeMap = _data.PlaneMaps.Single();
            ClearMap();
            SetupTiles();
        }

        private void ClearMap()
        {
            _planeMap.TileSpaces.ForEach(space => space.Tile = -1);
            _data.Things.Clear();
        }

        private void SetupTiles()
        {
            _tileAllWalls = new Tile("GSTONEA1", "GSTONEA1", "GSTONEA1", "GSTONEA1");
            _data.Tiles.Add(_tileAllWalls);
        }

        [Test]
        public void ShouldFindOneByOneRoom()
        {
            var location = new MapLocation(_data, 2, 2);
            location.Tile = _tileAllWalls;
            location.AddThing(Actor.Player1Start.ClassName);

            var room = LevelMapper.Map(_data);

            Assert.That(room, Is.Not.Null);
        }
    }
}