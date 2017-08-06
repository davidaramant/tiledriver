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
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.Tests.LevelGeometry.Mapping
{
    [TestFixture]
    public class LevelMapperIntegrationTest
    {
        [Test]
        [TestCaseSource(nameof(TestDefinitions))]
        public void Test(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var sa = new UwmfSyntaxAnalyzer();
                var map = UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader)));

                var levelMap = LevelMapper.Map(map);
                var room = levelMap.StartingRoom;

                Assert.That(room, Is.Not.Null);
                Assert.That(room.Locations, Is.Not.Empty);

                Console.WriteLine($"Found {room.Locations.Count} locations in the start room");
                Console.WriteLine($"Found {levelMap.AllRooms.Count()} rooms in the level");

                ProduceGraphs(map, levelMap);
            }
        }

        private void ProduceGraphs(MapData map, LevelMap levelMap)
        {
            var name = $"{map.Name}.gv";

            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var outputPath = Path.Combine(currentDirectory, name);

            var contentBuilder = new StringBuilder();
            contentBuilder.AppendLine("graph {");

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