// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.Tests.LevelGeometry.Mapping
{
    [TestFixture]
    public class LevelMapperIntegrationTest
    {
        private bool _writeMapDetails = false;
        private bool _writeGraphVizFiles = false;

        private StreamWriter _mapDetailsWriter;

        [OneTimeSetUp]
        public void Start()
        {
            if (_writeMapDetails)
                _mapDetailsWriter = new StreamWriter("C:\\temp\\rooms.txt");
        }

        [OneTimeTearDown]
        public void Stop()
        {
            if (_writeMapDetails)
                _mapDetailsWriter.Dispose();
        }

        [Test]
        [TestCaseSource(nameof(TestDefinitions))]
        public void Test(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var sa = new UwmfSyntaxAnalyzer();
                var map = UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader)));
                var mapper = new LevelMapper();

                var levelMap = mapper.Map(map);
                var room = levelMap.StartingRoom;

                Assert.That(room, Is.Not.Null);
                Assert.That(room.Locations, Is.Not.Empty);

                Console.WriteLine($"Found {room.Locations.Count} locations in the start room");
                Console.WriteLine($"Found {levelMap.AllRooms.Count()} rooms in the level");

                Console.WriteLine();
                foreach (var currentRoom in levelMap.AllRooms)
                {
                    Console.Write($"Room {currentRoom.Name}: ");
                    Console.Write(string.Join(", ", currentRoom.Locations.Select(l => $"({l.X},{l.Y})")));
                    Console.WriteLine();
                }

                if (_writeMapDetails)
                    _mapDetailsWriter.WriteLine($"Map {map.Name}: {levelMap.AllRooms.Count()} rooms");

                if (_writeGraphVizFiles)
                    ProduceGraphs(map, levelMap);
            }
        }

        private void ProduceGraphs(MapData map, LevelMap levelMap)
        {
            var name = $"{map.Name}.gv";

            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var outputPath = Path.Combine(currentDirectory, name);

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

                var roomSize = (double) room.Locations.Count / maximumLocationSize * maximumSize;
                if (roomSize < minimumSize)
                    roomSize = minimumSize;
                contentBuilder.AppendLine($"shape=circle, width={roomSize:F2}, fontsize=22];");
            }

            contentBuilder.AppendLine("}");

            foreach (var room in levelMap.AllRooms)
            {
                foreach (var passageRoomPair in room.AdjacentRooms)
                {
                    contentBuilder.AppendLine($"\"{room.Name}\" -- \"{passageRoomPair.Value.Name}\";");
                }
            }

            contentBuilder.AppendLine("}");

            using (var writer = new StreamWriter(outputPath))
            {
                writer.Write(contentBuilder.ToString());
            }
        }

        public static IEnumerable<string> TestDefinitions()
        {
            var mapDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "LegacyMaps"));
            foreach (var file in mapDirectory.GetFiles("*.uwmf"))
            {
                yield return file.FullName;
            }
        }
    }
}