﻿// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
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
            var wad = WadFile.Read(path);

            var mapBytes = wad[1].GetData();
            using (var ms = new MemoryStream(mapBytes))
            using (var textReader = new StreamReader(ms, Encoding.ASCII))
            {
                var sa = new UwmfSyntaxAnalyzer();
                var map = UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader)));

                var levelMap = LevelMapper.Map(map);
                var room = levelMap.StartingRoom;

                Assert.That(room, Is.Not.Null);
                Assert.That(room.Locations, Is.Not.Empty);

                Console.WriteLine($"Found {room.Locations.Count} locations in the start room");
                Console.WriteLine($"Found {levelMap.AllRooms.Count()} rooms in the level");
            }
        }

        public static IEnumerable<string> TestDefinitions()
        {
            var mapDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "LegacyMaps"));
            foreach (var file in mapDirectory.GetFiles("*.wad"))
            {
                yield return file.FullName;
            }
        }
    }
}