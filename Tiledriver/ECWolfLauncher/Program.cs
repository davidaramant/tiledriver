// Copyright (c) 2016, David Aramant
// Copyright (c) 2017, David Aramant and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using Functional.Maybe;
using Moq;
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
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.FormatModels.Wdc31;
using Tiledriver.Core.FormatModels.Xlat;
using Tiledriver.Core.FormatModels.Xlat.Parsing;
using Tiledriver.Core.MapTranslators;
using Tiledriver.Core.Settings;
using Tiledriver.Core.Tests;

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

                BatchConvertGameMaps(
                    baseInputPath: @"C:\Users\david\Desktop\Wolf3D Maps\Wolf3D User Maps\oldschoolspear",
                    outputPath: @"C:\Users\david\Desktop\Wolf3D Maps\Wolf3D User Maps\oldschool - converted");

                //ExportMapsFromPk3(
                //    pk3Path: @"C:\Users\david\Desktop\Wolf3D Maps\Wolf3D User Maps\astrostein_spiff.pk3",
                //    outputBasePath: @"C:\Users\david\Desktop\Wolf3D Maps\Wolf3D User Maps");

                //LoadMapInEcWolf(DemoMap.Create(), Path.GetFullPath("demo.wad"));
                //TranslateGameMapsFormat();
                //Flatten();
                //Pk3Test();
                //ConvertMapsToSimpleFormat(
                //    inputPath: Path.Combine(desktop, "maps"),
                //    outputDirsWithSaveMethods: new List<(string path, Action<MetaMap, string> saveMethod)>
                //    {
                //        (
                //        Path.Combine(desktop,"imagemaps-sparse"),
                //        (metaMap,fileNameWithNoExtension)=>SimpleMapImageExporter.Export(metaMap,MapPalette.HighlightWalls,fileNameWithNoExtension+".png")
                //        ),
                //        (
                //        Path.Combine(desktop,"imagemaps-solid"),
                //        (metaMap,fileNameWithNoExtension)=>SimpleMapImageExporter.Export(metaMap,MapPalette.CarveOutRooms,fileNameWithNoExtension+".png")
                //        ),
                //        (
                //        Path.Combine(desktop,"textmaps-sparse"),
                //        (metaMap,fileNameWithNoExtension)=>SimpleMapTextExporter.Export(metaMap,fileNameWithNoExtension+".txt",unreachableIsSolid:false)
                //        ),
                //        (
                //        Path.Combine(desktop,"textmaps-solid"),
                //        (metaMap,fileNameWithNoExtension)=>SimpleMapTextExporter.Export(metaMap,fileNameWithNoExtension+".txt",unreachableIsSolid:true)
                //        ),
                //    });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        private static void ConvertMapsToSimpleFormat(string inputPath, List<(string path, Action<MetaMap, string> saveMethod)> outputDirsWithSaveMethods)
        {
            foreach (var outputPath in outputDirsWithSaveMethods.Select(p => p.path))
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
                        var outputFilePath = Path.Combine(output.path, Path.GetFileNameWithoutExtension(uwmfFilePath) + ".png");
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
                    Func<string> mapHeadPath = ()=>FindPathOf(levelSetDir,"MAPHEAD");
                    Func<string> gameMapsPath = ()=>FindPathOf(levelSetDir,"GAMEMAPS");

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

        private static void LoadMapInEcWolf(MapData uwmfMap, string wadPath)
        {
            var ecWolfPath = GetECWolfExePath();

            var wad = new WadFile();
            wad.Append(new Marker("MAP01"));
            wad.Append(new UwmfLump("TEXTMAP", uwmfMap));
            wad.Append(new Marker("ENDMAP"));
            wad.SaveTo(wadPath);

            Process.Start(
                ecWolfPath,
                $"--file \"{wadPath}\" --hard --nowait --tedlevel map01");
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
