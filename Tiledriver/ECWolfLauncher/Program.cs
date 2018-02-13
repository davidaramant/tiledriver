﻿// Copyright (c) 2016, David Aramant
// Copyright (c) 2017, David Aramant and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Functional.Maybe;
using Moq;
using ShellProgressBar;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.GameMaps;
using Tiledriver.Core.FormatModels.MapInfos;
using Tiledriver.Core.FormatModels.MapInfos.Parsing;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.Pk3;
using Tiledriver.Core.FormatModels.SimpleMapImage;
using Tiledriver.Core.FormatModels.SimpleMapText;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Extensions;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.FormatModels.Wdc31;
using Tiledriver.Core.FormatModels.Xlat;
using Tiledriver.Core.FormatModels.Xlat.Parsing;
using Tiledriver.Core.LevelGeometry.CellularAutomata;
using Tiledriver.Core.LevelGeometry.Lighting;
using Tiledriver.Core.MapTranslators;
using Tiledriver.Core.Settings;
using Tiledriver.Core.Tests;
using Tiledriver.Core.Wolf3D;

namespace TestRunner
{
    /// <summary>
    /// Convenience program to directly launch the output in ECWolf from inside Visual Studio.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                //BatchConvertGameMaps(
                //    baseInputPath: @"C:\Users\david\Desktop\Wolf3D Maps\Wolf3D User Maps\oldschoolspear",
                //    outputPath: @"C:\Users\david\Desktop\Wolf3D Maps\Wolf3D User Maps\oldschool - converted");

                //ExportMapsFromPk3(
                //    pk3Path: @"C:\Users\david\Desktop\Wolf3D Maps\Wolf3D User Maps\astrostein_spiff.pk3",
                //    outputBasePath: @"C:\Users\david\Desktop\Wolf3D Maps\Wolf3D User Maps");

                //AnalyzeMaps(
                //    inputPath: @"C:\Users\david\Desktop\Wolf3D Maps\UDMF",
                //    outputPath: @"C:\Users\david\Desktop\Wolf3D Maps\metamaps");

                //RotateMaps(inputPath: @"C:\Users\david\Desktop\Wolf3D Maps\metamaps");
                //TestMapNameComparer();
                //RemoveDuplicateMaps(inputPath: @"C:\Users\david\Desktop\Wolf3D Maps\metamaps");

                //var basePath = @"C:\git\tiledriver\ml";

                //void MapToImage(string mapName)
                //{
                //    var mapPath = Path.Combine(basePath, mapName);
                //    var map = MetaMap.Load(mapPath);
                //    SimpleMapImageExporter.Export(map, MapPalette.CarveOutRooms, mapPath + ".png");
                //}

                //MapToImage("test.metamap");
                //MapToImage("roundtripped.metamap");


                //LoadMapInEcWolf(CAGenerator.Generate(), projectPath: Path.GetFullPath("Cave"));

                //// Visualize multiple generations
                //foreach (var generation in Enumerable.Range(0, 7))
                //{
                //    var genMap = CAGenerator.Generate(width: 128, height: 128, generations: generation);

                //    var genMetaMap = MetaMapAnalyzer.Analyze(genMap, includeAllEmptyAreas: true);

                //    SimpleMapImageExporter.Export(genMetaMap, MapPalette.CarveOutRooms, $"{generation:00} - CA Gen {generation}.png", scale: 10);
                //}


                var caMap = CAGenerator.Generate(width: 128, height: 128, stalagmiteProb:0.02,stalactiteProb:0.04, generations:6);
                var metaMap = MetaMapAnalyzer.Analyze(caMap, includeAllEmptyAreas: true);
                var roomGraph = RoomAnalyzer.Analyze(metaMap);
                var trimmedRoomGraph = new RoomGraph(roomGraph.Width, roomGraph.Height,
                    new[] { roomGraph.OrderBy(r => r.Area).Last() });


                //SimpleMapImageExporter.Export(metaMap, MapPalette.CarveOutRooms, $"caMap.png", scale: 10);
                //Process.Start("caMap.png");
                //SimpleMapImageExporter.Export(roomGraph, "07 - Enclosed playable spaces.png", scale: 10);
                //Process.Start("graph.png");
                //SimpleMapImageExporter.Export(trimmedRoomGraph, "08 - Only largest room.png", scale: 10);
                //Process.Start("trimmed-graph.png");

                // ************************************
                // Strip out all but the largest room
                var largeRoom = trimmedRoomGraph.First();
                var clonedMap = caMap.Clone();
                for (int row = 0; row < clonedMap.Height; row++)
                {
                    for (int col = 0; col < clonedMap.Width; col++)
                    {
                        if (!largeRoom.Contains(new Point(col, row)))
                        {
                            var index = row * clonedMap.Width + col;
                            clonedMap.PlaneMaps[0].TileSpaces[index].Tile = 0;
                        }
                    }
                }

                var firstPlayableSpaceIndex = clonedMap.PlaneMaps[0].TileSpaces.Select((ts, index) => (ts, index)).First(_ => !_.Item1.HasTile).Item2;
                var playerStart = clonedMap.Things.First(thing => thing.Type == Actor.Player1Start.ClassName);
                playerStart.X = firstPlayableSpaceIndex % clonedMap.Width + 0.5;
                playerStart.Y = firstPlayableSpaceIndex / clonedMap.Width + 0.5;

                // remove things that are now in walls
                var thingListCopy = clonedMap.Things.ToList();
                clonedMap.Things.Clear();
                clonedMap.Things.AddRange(thingListCopy.Where(t => !clonedMap.TileSpaceAt(t.TilePosition()).HasTile));

                // ***************************************

                LightTracer.AddRandomLightsToMap(clonedMap, largeRoom, lightRadius: 15, percentAreaToCoverWithLights: 0.005);

                LoadMapInEcWolf(clonedMap, "Cave");

                //TranslateGameMapsFormat();
                //Flatten();
                //Pk3Test();
                //ConvertMapsToSimpleFormat(
                //    inputPath: Path.Combine(desktop, "benchmark"),
                //    outputDirsWithSaveMethods: new List<(string path, Action<MetaMap, string> saveMethod)>
                //    {
                //        //(
                //        //"imagemaps-sparse",
                //        //(metaMap,fileNameWithNoExtension)=>SimpleMapImageExporter.Export(metaMap,MapPalette.HighlightWalls,fileNameWithNoExtension+".png")
                //        //),
                //        (
                //        "imagemaps-solid",
                //        (metaMap,fileNameWithNoExtension)=>SimpleMapImageExporter.Export(metaMap,MapPalette.CarveOutRooms,fileNameWithNoExtension+".png")
                //        ),
                //        //(
                //        //"textmaps-sparse",
                //        //(metaMap,fileNameWithNoExtension)=>SimpleMapTextExporter.Export(metaMap,fileNameWithNoExtension+".txt",unreachableIsSolid:false)
                //        //),
                //        //(
                //        //"textmaps-solid",
                //        //(metaMap,fileNameWithNoExtension)=>SimpleMapTextExporter.Export(metaMap,fileNameWithNoExtension+".txt",unreachableIsSolid:true)
                //        //),
                //    });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        private static void RotateMaps(string inputPath)
        {
            string MutateFileName(string path, string postfix)
            {
                var fileName = Path.GetFileNameWithoutExtension(path) + " " + postfix;

                return Path.Combine(Path.GetDirectoryName(path), fileName + ".metamap");
            }

            var filesToGoThrough = Directory.GetFiles(inputPath, "*.metamap", SearchOption.AllDirectories);

            using (var progress = new ProgressBar(filesToGoThrough.Length, "Duplicating maps..."))
            {
                Parallel.ForEach(filesToGoThrough, metaMapPath =>
                {
                    var map = MetaMap.Load(metaMapPath);

                    map.Mirror().Save(MutateFileName(metaMapPath, "m"));

                    var rotated90 = map.Rotate90();
                    rotated90.Save(MutateFileName(metaMapPath, "r1"));
                    rotated90.Mirror().Save(MutateFileName(metaMapPath, "r1m"));

                    var rotated180 = rotated90.Rotate90();
                    rotated180.Save(MutateFileName(metaMapPath, "r2"));
                    rotated180.Mirror().Save(MutateFileName(metaMapPath, "r2m"));

                    var rotated270 = rotated180.Rotate90();
                    rotated270.Save(MutateFileName(metaMapPath, "r3"));
                    rotated270.Mirror().Save(MutateFileName(metaMapPath, "r3m"));

                    progress.Tick();
                });
            }
        }

        public static void TestMapNameComparer()
        {
            void TestTypeDetermination(string name, MapNameComparer.Type expectedType)
            {
                var actualType = MapNameComparer.DetermineType(name);

                Console.WriteLine($"Checking type for ({name})...");
                if (actualType != expectedType)
                {
                    Console.WriteLine($"  FAILED!\n  Expected:\t{expectedType}\n  Actual:\t{actualType}");
                }
                else
                {
                    Console.WriteLine("  Passed!");
                }
            }

            TestTypeDetermination("Wolf3D Map 1", MapNameComparer.Type.Wolf3D);
            TestTypeDetermination("Wolf3D Map 1 r3m", MapNameComparer.Type.Wolf3D | MapNameComparer.Type.Rotated270 | MapNameComparer.Type.Mirrored);
            TestTypeDetermination("Custom Map", MapNameComparer.Type.Custom);
            TestTypeDetermination("Custom Map m", MapNameComparer.Type.Custom | MapNameComparer.Type.Mirrored);
            TestTypeDetermination("Custom Map r1", MapNameComparer.Type.Custom | MapNameComparer.Type.Rotated90);
            TestTypeDetermination("Custom Map r1m", MapNameComparer.Type.Custom | MapNameComparer.Type.Rotated90 | MapNameComparer.Type.Mirrored);
            TestTypeDetermination("Custom Map r2", MapNameComparer.Type.Custom | MapNameComparer.Type.Rotated180);
            TestTypeDetermination("Custom Map r2m", MapNameComparer.Type.Custom | MapNameComparer.Type.Rotated180 | MapNameComparer.Type.Mirrored);
            TestTypeDetermination("Custom Map r3", MapNameComparer.Type.Custom | MapNameComparer.Type.Rotated270);
            TestTypeDetermination("Custom Map r3m", MapNameComparer.Type.Custom | MapNameComparer.Type.Rotated270 | MapNameComparer.Type.Mirrored);

            void TestComparison(string x, string y, int expected)
            {
                Console.WriteLine($"Testing comparison for ({x}) and ({y})...");
                var actual = new MapNameComparer().Compare(x, y);
                if (actual != expected)
                {
                    Console.WriteLine($"  FAILED!\n  Expected {expected} but got {actual}");
                }
                else
                {
                    Console.WriteLine("  Passed!");
                }
            }

            TestComparison("Wolf3D Map 1", "Custom Map", -1);
            TestComparison("Custom Map", "Wolf3D Map 1", 1);
            TestComparison("Wolf3D Map 1", "Wolf3D Map 4", 0);
            TestComparison("Custom Map 1", "Custom Map 4", 0);
            TestComparison("Custom Map 1", "Custom Map 4 m", -1);
            TestComparison("Wolf3D Map r3m", "Custom Map", -1);

            Console.ReadLine();
        }

        sealed class MapNameComparer : Comparer<string>
        {
            [Flags]
            public enum Type
            {
                Wolf3D,
                Mirrored = 1 << 0,
                Rotated90 = 1 << 1,
                Rotated180 = 1 << 2,
                Rotated270 = 1 << 3,
                Custom = 1 << 4,
            }

            public static Type DetermineType(string path)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);

                var type = fileName.Contains("Wolf3D") ? Type.Wolf3D : Type.Custom;

                if (fileName.EndsWith(" m"))
                {
                    type |= Type.Mirrored;
                }
                else if (fileName.EndsWith(" r1"))
                {
                    type |= Type.Rotated90;
                }
                else if (fileName.EndsWith(" r1m"))
                {
                    type |= Type.Mirrored;
                    type |= Type.Rotated90;
                }
                else if (fileName.EndsWith(" r2"))
                {
                    type |= Type.Rotated180;
                }
                else if (fileName.EndsWith(" r2m"))
                {
                    type |= Type.Mirrored;
                    type |= Type.Rotated180;
                }
                else if (fileName.EndsWith(" r3"))
                {
                    type |= Type.Rotated270;
                }
                else if (fileName.EndsWith(" r3m"))
                {
                    type |= Type.Mirrored;
                    type |= Type.Rotated270;
                }

                return type;
            }

            public override int Compare(string x, string y)
            {
                var xType = DetermineType(x);
                var yType = DetermineType(y);

                return xType.CompareTo(yType);
            }
        }

        private static void RemoveDuplicateMaps(string inputPath)
        {
            var duplicateFileGroups =
                Directory.EnumerateFiles(inputPath, "*.metamap", SearchOption.AllDirectories).
                AsParallel().
                Select(filePath =>
                    {
                        using (var md5 = new MD5CryptoServiceProvider())
                        using (var fs = File.OpenRead(filePath))
                        {
                            return (filePath: filePath, hash: BitConverter.ToString(md5.ComputeHash(fs)));
                        }
                    }).
                GroupBy(tuple => tuple.hash).
                Where(group => group.Count() > 1).
                ToArray();

            var comparer = new MapNameComparer();

            var deletedFiles = new List<string>();

            foreach (var dupeGroup in duplicateFileGroups)
            {
                var filePaths = dupeGroup.Select(tuple => tuple.filePath).OrderBy(filePath => filePath, comparer).ToArray();

                string fileToKeep = filePaths.First();

                var filesToRemove = filePaths.Except(new[] { fileToKeep });
                deletedFiles.Add($"{Path.GetFileName(fileToKeep)} - Duplicates: " + string.Join(", ", filesToRemove.Select(Path.GetFileName)));
                foreach (var file in filesToRemove)
                {
                    File.Delete(file);
                }
            }

            File.WriteAllLines("deleted maps.txt", deletedFiles);
        }

        private static void AnalyzeMaps(string inputPath, string outputPath)
        {
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, recursive: true);
            }
            Directory.CreateDirectory(outputPath);

            var failures = new ConcurrentBag<string>();

            var sa = new UwmfSyntaxAnalyzer();
            var filesToGoThrough = Directory.GetFiles(inputPath, "*.uwmf", SearchOption.AllDirectories);
            Parallel.ForEach(filesToGoThrough, uwmfFilePath =>
            {
                var filename = Path.GetFileNameWithoutExtension(uwmfFilePath);
                try
                {
                    using (var stream = File.OpenRead(uwmfFilePath))
                    using (var textReader = new StreamReader(stream, Encoding.ASCII))
                    {
                        var mapData = UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader)));
                        var metaMap = MetaMapAnalyzer.Analyze(mapData);
                        metaMap.Save(Path.Combine(outputPath, filename + ".metamap"));
                    }
                }
                catch (Exception e)
                {
                    failures.Add(filename + "\t" + e.Message);
                }
            });

            File.WriteAllLines(Path.Combine(outputPath, "errors.txt"), failures);
        }

        private static void ConvertMapsToSimpleFormat(string inputPath, List<(string path, Action<MetaMap, string> saveMethod)> outputDirsWithSaveMethods)
        {
            foreach (var outputPath in outputDirsWithSaveMethods.Select(p => Path.Combine(inputPath, p.path)))
            {
                if (Directory.Exists(outputPath))
                {
                    Directory.Delete(outputPath, recursive: true);
                }
                Directory.CreateDirectory(outputPath);
            }

            var sa = new UwmfSyntaxAnalyzer();
            var filesToGoThrough = Directory.GetFiles(inputPath, "*.uwmf", SearchOption.AllDirectories);
            int current = 0;
            foreach (var uwmfFilePath in filesToGoThrough)
            {
                current++;
                Console.WriteLine($"File: {current}/{filesToGoThrough.Length}");

                using (var stream = File.OpenRead(uwmfFilePath))
                using (var textReader = new StreamReader(stream, Encoding.ASCII))
                {
                    var mapData = UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader)));
                    var metaMap = MetaMapAnalyzer.Analyze(mapData);

                    foreach (var output in outputDirsWithSaveMethods)
                    {
                        var outputFilePath = Path.Combine(inputPath, output.path, Path.GetFileNameWithoutExtension(uwmfFilePath) + ".png");
                        output.saveMethod(metaMap, outputFilePath);
                    }
                }
            }
        }

        private static void ExportMapsFromPk3(string pk3Path, string outputBasePath = ".")
        {
            var ecWolfPk3Path = Path.ChangeExtension(GetECWolfExePath(), "pk3");

            var levelSetName = Path.GetFileNameWithoutExtension(pk3Path);

            var outputPath = Path.Combine(outputBasePath, $"{levelSetName} Translated");
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, recursive: true);
            }
            Directory.CreateDirectory(outputPath);

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TriggerTemplate, Trigger>();
                cfg.CreateMap<ZoneTemplate, Zone>();
                cfg.CreateMap<TileTemplate, Tile>();
            });

            var autoMapper = autoMapperConfig.CreateMapper();

            using (var resources = new CompoundResourceProvider())
            using (var basePk3 = Pk3File.Open(ecWolfPk3Path))
            using (var pk3 = Pk3File.Open(pk3Path))
            {
                resources.AddProvider(basePk3);
                resources.AddProvider(pk3);

                var mapInfos = LoadMapInfo(resources, mapInfoStream: resources.TryLookup("MAPINFO.txt").Or(() => resources.TryLookup("MAPINFO")).Value);
                var xlat = LoadXlat(resources, resources.Lookup(mapInfos.GameInfo.Value.Translator.Value));
                var translator = new BinaryMapTranslator(translatorInfo: xlat, autoMapper: autoMapper);

                for (int mapIndex = 0; mapIndex < mapInfos.Maps.Count; mapIndex++)
                {
                    var mapInfo = mapInfos.Maps[mapIndex];

                    using (var wadStream = new MemoryStream())
                    {
                        resources.Lookup($"maps/{mapInfo.MapLump}.wad").CopyTo(wadStream);
                        wadStream.Position = 0;

                        var wad = WadFile.Read(wadStream);
                        var mapFileBytes = wad[1].GetData();
                        using (var ms = new MemoryStream(mapFileBytes))
                        {
                            var binaryMap = Wdc31Bundle.ReadMaps(ms).Single();

                            var uwmfMap = translator.Translate(binaryMap, mapInfo);

                            var fileSafeName = uwmfMap.Name;
                            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                            {
                                fileSafeName = fileSafeName.Replace(invalidChar.ToString(), string.Empty);
                            }
                            fileSafeName = fileSafeName.Trim();

                            using (var outStream =
                                File.OpenWrite(Path.Combine(outputPath, $"{levelSetName} {mapIndex + 1} - {fileSafeName}.uwmf")))
                            {
                                uwmfMap.WriteTo(outStream);
                            }
                        }
                    }
                }
            }
        }

        private static MapTranslatorInfo LoadXlat(IResourceProvider provider, Stream xlatStream)
        {
            using (var textReader = new StreamReader(xlatStream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                var syntaxAnalzer = new XlatSyntaxAnalyzer(provider);
                var result = syntaxAnalzer.Analyze(lexer);
                return XlatParser.Parse(result);
            }
        }

        private static MapInfo LoadMapInfo(IResourceProvider provider, Stream mapInfoStream)
        {
            using (mapInfoStream)
            using (var textReader = new StreamReader(mapInfoStream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(provider);
                var elements = lexer.Analyze(textReader).ToArray();
                return MapInfoParser.Parse(elements);
            }
        }

        private static void Flatten()
        {
            var input = @"C:\Users\david\Desktop\maps\translated.wad";

            var map = OpenWadFile(input);
            map.Name = "Flatland";

            for (int tileIndex = 0; tileIndex < map.Tiles.Count; tileIndex++)
            {
                var tile = map.Tiles[tileIndex];

                var texture = PickTexture(tile);

                //tile.TextureNorth = tile.TextureSouth = tile.TextureEast = tile.TextureWest = "-";

                var sectorIndex = map.Sectors.Count;
                map.Sectors.Add(new Sector(textureCeiling: "#383838", textureFloor: texture));

                var spaces = map.PlaneMaps[0].TileSpaces.Where(ts => ts.Tile == tileIndex);

                foreach (var space in spaces)
                {
                    space.Sector = sectorIndex;
                    space.Tile = -1;
                }
            }

            var index = map.Tiles.Count;
            map.Tiles.Add(new Tile
            {
                TextureEast = "#383838",
                TextureNorth = "#383838",
                TextureSouth = "#383838",
                TextureWest = "#383838",
                BlockingEast = true,
                BlockingNorth = true,
                BlockingSouth = true,
                BlockingWest = true,
            });

            // shift all things + triggers
            foreach (var thing in map.Things)
            {
                thing.X++;
                thing.Y++;
            }
            foreach (var trigger in map.Triggers)
            {
                trigger.X++;
                trigger.Y++;
            }

            map.Width = map.Height = 66;

            // Add sides
            var solidTile = new TileSpace(index, 0, -1);
            var ts2 = map.PlaneMaps[0].TileSpaces;
            for (int row = 0; row < 64; row++)
            {
                ts2.Insert(row * 64 + 2 * row, solidTile);
                ts2.Insert(row * 64 + 2 * row + 65, solidTile);
            }

            // Add top/bottom
            ts2.InsertRange(0, Enumerable.Repeat(solidTile, 66));
            ts2.InsertRange(4290, Enumerable.Repeat(solidTile, 66));


            LoadMapInEcWolf(map, "flatland.wad");
        }

        private static string PickTexture(Tile tile)
        {
            if (tile.OffsetVertical)
            {
                if (tile.TextureEast.StartsWith("DOOR1"))
                    return "DOOR1_1";
                return "DOOR2_1";
            }



            return tile.TextureNorth;
        }

        private static MapData OpenWadFile(string filePath)
        {
            var wad = WadFile.Read(filePath);

            var mapBytes = wad[1].GetData();
            using (var ms = new MemoryStream(mapBytes))
            using (var textReader = new StreamReader(ms, Encoding.ASCII))
            {
                var sa = new UwmfSyntaxAnalyzer();
                var map = UwmfParser.Parse(sa.Analyze(new UwmfLexer(textReader)));

                return map;
            }
        }

        private static void TranslateAllWolf3DMaps()
        {
            var paths = SteamGameSearcher.GetGamePaths();

            var basePath = Path.Combine(paths.Wolf3D.Value, "base");
            var mapHeadPath = Path.Combine(basePath, "MAPHEAD.WL6");
            var gameMapsPath = Path.Combine(basePath, "GAMEMAPS.WL6");

            if (Directory.Exists("Wolf3D Maps"))
            {
                Directory.Delete("Wolf3D Maps", recursive: true);
            }

            TranslateGameMapsFormat(
                mapHeadPath,
                gameMapsPath,
                outputPath: "Wolf3D Maps",
                levelSetName: "Wolf3D");
        }

        private static void BatchConvertGameMaps(
            string baseInputPath,
            string outputPath)
        {
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, recursive: true);
            }
            Directory.CreateDirectory(outputPath);

            string FindPathOf(string path, string name) =>
                Directory.EnumerateFiles(path, name + ".*", SearchOption.AllDirectories).
                Single(f => Path.GetExtension(f).ToLowerInvariant() != "bak");

            TranslateGameMapsFormat(
                Directory.GetDirectories(baseInputPath).Select(levelSetDir =>
                {
                    var name = Path.GetFileName(levelSetDir);
                    Func<string> mapHeadPath = () => FindPathOf(levelSetDir, "MAPHEAD");
                    Func<string> gameMapsPath = () => FindPathOf(levelSetDir, "GAMEMAPS");

                    return (mapHeadPath, gameMapsPath, name);
                }),
                outputPath);

            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        private static void TranslateGameMapsFormat(
            IEnumerable<(
            Func<string> mapHeadPath,
            Func<string> gameMapsPath,
            string name)> levelSets,
            string outputPath)
        {
            var ecWolfPk3Path = Path.ChangeExtension(GetECWolfExePath(), "pk3");

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TriggerTemplate, Trigger>();
                cfg.CreateMap<ZoneTemplate, Zone>();
                cfg.CreateMap<TileTemplate, Tile>();
            });

            var autoMapper = autoMapperConfig.CreateMapper();

            using (var resources = Pk3File.Open(ecWolfPk3Path))
            {
                var mapInfos = LoadMapInfo(resources, mapInfoStream: resources.Lookup("mapinfo/spear.txt"));
                var xlat = LoadXlat(resources, resources.Lookup(mapInfos.GameInfo.Value.Translator.Value));
                var translator = new BinaryMapTranslator(translatorInfo: xlat, autoMapper: autoMapper);

                foreach (var levelSet in levelSets)
                {
                    Console.WriteLine(levelSet.name);
                    try
                    {
                        using (var headerStream = File.OpenRead(levelSet.mapHeadPath()))
                        using (var mapsStream = File.OpenRead(levelSet.gameMapsPath()))
                        {
                            var gameMaps = GameMapsBundle.Load(headerStream, mapsStream);
                            for (int mapIndex = 0; mapIndex < gameMaps.Maps.Length; mapIndex++)
                            {
                                var bMap = gameMaps.LoadMap(mapIndex, mapsStream);

                                var uwmfMap = translator.Translate(bMap, mapInfos.Maps[mapIndex]);

                                var fileName = $"{levelSet.name} {mapIndex + 1:00} - {uwmfMap.Name}.uwmf";

                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    fileName = fileName.Replace(c.ToString(), "");
                                }

                                using (var outStream = File.OpenWrite(Path.Combine(outputPath, fileName)))
                                {
                                    uwmfMap.WriteTo(outStream);
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"\n\t{levelSet.name} FAILED:\n\t" + e + "\n");
                    }
                }
            }
        }

        private static void TranslateGameMapsFormat(
            string mapHeadPath,
            string gameMapsPath,
            string outputPath,
            string levelSetName)
        {
            var ecWolfPk3Path = Path.ChangeExtension(GetECWolfExePath(), "pk3");

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TriggerTemplate, Trigger>();
                cfg.CreateMap<ZoneTemplate, Zone>();
                cfg.CreateMap<TileTemplate, Tile>();
            });

            var autoMapper = autoMapperConfig.CreateMapper();

            using (var resources = Pk3File.Open(ecWolfPk3Path))
            {
                var mapInfos = LoadMapInfo(resources, mapInfoStream: resources.Lookup("mapinfo/spear.txt"));
                var xlat = LoadXlat(resources, resources.Lookup(mapInfos.GameInfo.Value.Translator.Value));
                var translator = new BinaryMapTranslator(translatorInfo: xlat, autoMapper: autoMapper);

                using (var headerStream = File.OpenRead(mapHeadPath))
                using (var mapsStream = File.OpenRead(gameMapsPath))
                {
                    var gameMaps = GameMapsBundle.Load(headerStream, mapsStream);
                    for (int mapIndex = 0; mapIndex < gameMaps.Maps.Length; mapIndex++)
                    {
                        var bMap = gameMaps.LoadMap(mapIndex, mapsStream);

                        var uwmfMap = translator.Translate(bMap, mapInfos.Maps[mapIndex]);

                        var fileName = $"{levelSetName} {mapIndex + 1:00} - {uwmfMap.Name}.uwmf";

                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            fileName = fileName.Replace(c.ToString(), "");
                        }

                        using (var outStream = File.OpenWrite(Path.Combine(outputPath, fileName)))
                        {
                            uwmfMap.WriteTo(outStream);
                        }
                    }
                }
            }
        }

        private static void LoadMapInEcWolf(MapData uwmfMap, string projectPath = "")
        {
            string wadFilePath = "demo.wad";
            string pathToLoad = Path.GetFullPath(wadFilePath);

            if (!string.IsNullOrWhiteSpace(projectPath))
            {
                var mapsSubDir = Path.Combine(projectPath, "maps");

                if (!Directory.Exists(mapsSubDir))
                {
                    Directory.CreateDirectory(mapsSubDir);
                }

                wadFilePath = Path.Combine(mapsSubDir, "Map01.wad");
                pathToLoad = Path.GetFullPath(projectPath);
            }

            var ecWolfPath = GetECWolfExePath();

            var wad = new WadFile();
            wad.Append(new Marker("MAP01"));
            wad.Append(new UwmfLump("TEXTMAP", uwmfMap));
            wad.Append(new Marker("ENDMAP"));
            wad.SaveTo(wadFilePath);

            Process.Start(
                ecWolfPath,
                $"--file \"{pathToLoad}\" --data wl6 --hard --nowait --tedlevel map01");
        }

        private static string GetECWolfExePath()
        {
            const string inputFile = "ECWolfPath.txt";

            if (!File.Exists(inputFile))
            {
                throw new Exception(
                    $"Could not find {inputFile}.  " +
                    "Create this file in the output directory containing a single line with the full path to ECWolf.exe.  " +
                    "Do not quote the path.");
            }

            var ecWolfPath = File.ReadAllLines(inputFile).Single();

            if (Path.GetFileName(ecWolfPath).ToLowerInvariant() != "ecwolf.exe")
            {
                ecWolfPath = Path.Combine(ecWolfPath, "ecwolf.exe");
            }
            return ecWolfPath;
        }
    }
}
