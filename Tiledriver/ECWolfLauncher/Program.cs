﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.GameMaps;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.FormatModels.Xlat;
using Tiledriver.Core.FormatModels.Xlat.Parsing;
using Tiledriver.Core.MapTranslators;

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
            TranslatorTest();
        }

        private static void TranslatorTest()
        {
            var bMap = LoadBinaryMap();

            var xlat = LoadXlat();

            var translator = new BinaryMapTranslator(translatorInfo: xlat);
            var uwmfMap = translator.Translate(bMap);

            LoadMapInEcWolf(uwmfMap, Path.GetFullPath("translated.wad"));
        }

        private static BinaryMap LoadBinaryMap()
        {
            var basePath = @"C:\Program Files (x86)\Steam\steamapps\common\Wolfenstein 3D\base";
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

        private static void LoadMapInEcWolf(Map uwmfMap, string wadPath)
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

            var wad = new WadFile();
            wad.Append(new Marker("MAP01"));
            wad.Append(new UwmfLump("TEXTMAP", uwmfMap));
            wad.Append(new Marker("ENDMAP"));
            wad.SaveTo(wadPath);

            Process.Start(
                ecWolfPath,
                $"--file \"{wadPath}\" --normal --nowait --tedlevel map01");
        }
    }
}
