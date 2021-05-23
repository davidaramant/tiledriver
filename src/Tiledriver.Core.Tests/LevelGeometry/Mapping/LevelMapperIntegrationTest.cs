// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Reading;
using Tiledriver.Core.LevelGeometry.Mapping;
using Tiledriver.Core.MapRanker;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.Mapping
{
    public class LevelMapperIntegrationTest : IDisposable
    {
        private bool _writeMapDetails = false;
        private bool _writeGraphVizFiles = false;

        private StreamWriter _mapDetailsWriter;

        public LevelMapperIntegrationTest()
        {
            _mapDetailsWriter = new StreamWriter("C:\\temp\\rooms.txt");
        }

        public void Dispose()
        {
            _mapDetailsWriter.Dispose();
        }

        [Theory(Skip = "This is a weird pseudo test; should it be a console app?")]
        [MemberData(nameof(TestDefinitions))]
        public void Test(string path)
        {
            using var stream = File.OpenRead(path);
            var map = UwmfReader.Read(stream);
            var mapper = new LevelMapper();

            var levelMap = mapper.Map(map);
            var room = levelMap.StartingRoom;

            room.Should().NotBeNull();
            room.Locations.Should().NotBeEmpty();

            Console.WriteLine($"Found {room.Locations.Count} locations in the start room");
            Console.WriteLine($"Found {levelMap.AllRooms.Count()} rooms in the level");

            var ranker = new Ranker();
            var score = ranker.RankLevel(map);
            Console.WriteLine($"Score: {score}");

            Console.WriteLine();
            foreach (var currentRoom in levelMap.AllRooms)
            {
                Console.Write($"Room {currentRoom.Name}: ");
                Console.Write(string.Join(", ", currentRoom.Locations.Select(l => $"({l.X},{l.Y})")));
                Console.WriteLine();
            }

            if (_writeMapDetails)
                _mapDetailsWriter.WriteLine($"Map {map.Name}: {levelMap.AllRooms.Count()} rooms, score={score}");

            if (_writeGraphVizFiles)
                ProduceGraphs(map, levelMap);
        }

        private void ProduceGraphs(MapData map, LevelMap levelMap)
        {
            var name = $"{map.Name}.gv";

            var content = GraphVizRenderer.BuildGraphDefinition(map, levelMap);

            using var writer = new StreamWriter(name);
            writer.Write(content);
        }

        public static IEnumerable<object[]> TestDefinitions()
        {
            var mapDirectory = new DirectoryInfo("LegacyMaps");
            foreach (var file in mapDirectory.GetFiles("*.uwmf"))
            {
                yield return new[] { file.FullName };
            }
        }
    }
}