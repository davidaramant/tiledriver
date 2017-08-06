// Copyright (c) 2016, David Aramant
// Copyright (c) 2017, David Aramant and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using Moq;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.GameMaps;
using Tiledriver.Core.FormatModels.MapInfos;
using Tiledriver.Core.FormatModels.MapInfos.Parsing;
using Tiledriver.Core.FormatModels.Pk3;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Parsing;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.FormatModels.Wdc31;
using Tiledriver.Core.FormatModels.Xlat;
using Tiledriver.Core.FormatModels.Xlat.Parsing;
using Tiledriver.Core.MapTranslators;
using Tiledriver.Core.Settings;

namespace TestRunner
{
    /// <summary>
    /// Convenience program to directly launch the output in ECWolf from inside Visual Studio.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //LoadMapInEcWolf(DemoMap.Create(), Path.GetFullPath("demo.wad"));
            //TranslateAllWolf3DMaps();
            //Flatten();
            //Pk3Test();
        }



        private static void ExportMapsFromPk3(string pk3Path, string outputBasePath)
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

                var mapInfos = LoadMapInfo(resources);
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

        private static MapInfo LoadMapInfo(IResourceProvider provider)
        {
            using (var stream = provider.Lookup("MAPINFO.txt"))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
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
            var mapInfos = LoadMapInfo();
            var xlat = LoadXlat();

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TriggerTemplate, Trigger>();
                cfg.CreateMap<ZoneTemplate, Zone>();
                cfg.CreateMap<TileTemplate, Tile>();
            });

            var autoMapper = autoMapperConfig.CreateMapper();
            var translator = new BinaryMapTranslator(translatorInfo: xlat, autoMapper: autoMapper);

            var paths = SteamGameSearcher.GetGamePaths();

            var basePath = Path.Combine(paths.Wolf3D.Value, "base");
            var mapHeadPath = Path.Combine(basePath, "MAPHEAD.WL6");
            var gameMapsPath = Path.Combine(basePath, "GAMEMAPS.WL6");

            var outputPath = "Translated";

            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, recursive: true);
            }

            using (var headerStream = File.OpenRead(mapHeadPath))
            using (var mapsStream = File.OpenRead(gameMapsPath))
            {
                var gameMaps = GameMapsBundle.Load(headerStream, mapsStream);
                for (int mapIndex = 0; mapIndex < mapInfos.Maps.Count; mapIndex++)
                {
                    var bMap = gameMaps.LoadMap(mapIndex, mapsStream);

                    var uwmfMap = translator.Translate(bMap, mapInfos.Maps[mapIndex]);

                    using (var outStream = File.OpenWrite(Path.Combine(outputPath, $"Wolf3D {mapIndex + 1:00} - {uwmfMap.Name}.uwmf")))
                    {
                        uwmfMap.WriteTo(outStream);
                    }
                }
            }
        }

        private static BinaryMap LoadBinaryMap()
        {
            var paths = SteamGameSearcher.GetGamePaths();

            var basePath = Path.Combine(paths.Wolf3D.Value, "base");
            var mapHeadPath = Path.Combine(basePath, "MAPHEAD.WL6");
            var gameMapsPath = Path.Combine(basePath, "GAMEMAPS.WL6");

            using (var headerStream = File.OpenRead(mapHeadPath))
            using (var mapsStream = File.OpenRead(gameMapsPath))
            {
                var gameMaps = GameMapsBundle.Load(headerStream, mapsStream);

                return gameMaps.LoadMap(0, mapsStream);
            }
        }

        private static MapTranslatorInfo LoadXlat()
        {
            using (var stream = File.OpenRead(Path.Combine("..", "..", "..", "Tiledriver.Core.Tests", "FormatModels", "Xlat", "Parsing", "wolf3d.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                var syntaxAnalzer = new XlatSyntaxAnalyzer(Mock.Of<IResourceProvider>());
                var result = syntaxAnalzer.Analyze(lexer);
                return XlatParser.Parse(result);
            }
        }

        private static MapInfo LoadMapInfo()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider.Setup(_ => _.Lookup("mapinfo/wolfcommon.txt"))
                .Returns(File.OpenRead(
                        Path.Combine("..", "..", "..", "Tiledriver.Core.Tests", "FormatModels", "MapInfos",
                        "Parsing", "wolfcommon.txt")));

            using (var stream = File.OpenRead(Path.Combine("..", "..", "..", "Tiledriver.Core.Tests", "FormatModels", "MapInfos", "Parsing", "wolf3d.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new MapInfoLexer(mockProvider.Object);
                var elements = lexer.Analyze(textReader).ToArray();
                return MapInfoParser.Parse(elements);
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
