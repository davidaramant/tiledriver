// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using SkiaSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.LevelGeometry.Lighting;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;
using NUnit.Framework;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Wolf;

namespace Tiledriver.Core.ManualTests
{
    [TestFixture]
    public sealed class CaveGenerationVisualization
    {
        private const int ImageScale = 10;

        [Test, Explicit]
        public void ShowEntireProcess()
        {
            CreateCave(
                seed: 13,
                folderName: "Cave Generation Process",
                visualizeProcess: true);
        }

        [Test, Explicit]
        public void ShowLotsOfSeeds()
        {
            const bool visualizeProcess = false;
            const string folderName = "Cave Seeds";

            Parallel.ForEach(Enumerable.Range(0, 100),
                seed => { CreateCave(seed, folderName, visualizeProcess); });
        }

        [Test, Explicit]
        public void MakeDetailedCave()
        {
            var totalTime = Stopwatch.StartNew();
            var stepTime = Stopwatch.StartNew();
            var dir = OutputLocation.CreateDirectory("Detailed Cave");

            static IFastImage Visualize(CellBoard board, int scale = 1) =>
                GenericVisualizer.RenderBinary(board.Dimensions,
                        isTrue: p => board[p] == CellType.Dead,
                        trueColor: SKColors.White,
                        falseColor: SKColors.Black,
                        scale: scale);

            foreach (var file in dir.GetFiles())
            {
                file.Delete();
            }

            var path = dir.FullName;

            void LogProgress(string msg)
            {
                TestContext.Progress.WriteLine($"{totalTime.Elapsed}: {msg} (step time {stepTime.Elapsed})");
                stepTime.Restart();
            }

            void SaveImage(IFastImage image, string name)
            {
                image.Save(Path.Combine(path, $"{name}.png"));
                LogProgress(name);
            }

            void Save(CellBoard board, string name, int scale)
            {
                using var img = Visualize(board, scale);
                SaveImage(img, name);
            }

            var random = new Random(1);

            var board =
                new CellBoard(new Size(128, 128))
                    .Fill(random, probabilityAlive: 0.5)
                    .MakeBorderAlive(thickness: 1)
                    .GenerateStandardCave();

            int step = 1;

            Save(board, $"{step++}. board", 8);

            const int scalingIterations = 2;
            const double noise = 0.2;

            CellBoard scaled = board;

            foreach (int scalingIteration in Enumerable.Range(1, scalingIterations))
            {
                scaled = scaled.Quadruple().AddNoise(random, noise).RunGenerations(1);

                Save(scaled, $"{step++}. board {1 << scalingIteration}x - noise {noise:F2}", 8 / (1 << scalingIteration));
            }

            var trimmed = scaled.TrimToLargestDeadArea();

            Save(trimmed, $"{step++}. trimmed", 1);

            // remove noise
            var aliveAreas =
                ConnectedAreaAnalyzer
                    .FindForegroundAreas(trimmed.Dimensions, p => trimmed[p] == CellType.Alive)
                    .Where(area => area.Area > 64)
                    .ToArray();

            var denoised = new CellBoard(trimmed.Dimensions, pos => aliveAreas.Any(a => a.Contains(pos)) ? CellType.Alive : CellType.Dead);

            Save(denoised, $"{step++}. denoised", 1);

            var fullyDenoised = denoised.ScaleAndSmooth();

            Save(fullyDenoised, $"{step++}. smoothed and scaled", 1);

            // Very silly!!! Need to turn CellBoard into ConnectedArea
            var (playArea, dimensions) = ConnectedAreaAnalyzer
                    .FindForegroundAreas(fullyDenoised.Dimensions, p => fullyDenoised[p] == CellType.Dead)
                    .First()
                    .TrimExcess(border: 1); // border would not be useful during actual Doom level generation

            LogProgress("Converted CellBoard into ConnectedArea");

            var interiorDistances = playArea.DetermineInteriorEdgeDistance(Neighborhood.Moore);

            LogProgress("Found interior distances");

            var largestDistance = interiorDistances.Values.Max();
            var numLevels = 6;
            var levelSize = largestDistance / numLevels;

            using var interiorImg = GenericVisualizer.RenderPalette(
                dimensions,
                getColor: p =>
                {
                    if (interiorDistances.TryGetValue(p, out int d))
                    {
                        return SKColor.FromHsv(0, (1 - ((d / levelSize) / (float)numLevels)) * 100, 100);
                    }
                    return SKColors.Black;
                },
                scale: 1);

            SaveImage(interiorImg, $"{step++}. interior");
        }

        private static void CreateCave(int seed, string folderName, bool visualizeProcess)
        {
            var stopWatch = Stopwatch.StartNew();

            void Log(string msg) => Console.WriteLine(stopWatch.Elapsed + ": " + msg);
            Log($"Seed {seed} - Start");
            Size dimensions = new(128, 128);

            var random = new Random(seed);
            var board =
                new CellBoard(dimensions)
                    .Fill(random, probabilityAlive: 0.5)
                    .MakeBorderAlive(thickness: 3);

            DirectoryInfo dirInfo = OutputLocation.CreateDirectory(folderName);

            void SaveImage(IFastImage image, int step, string description) =>
                image.Save(Path.Combine(dirInfo.FullName, $"Seed {seed:00} - Step {step:00} - {description}.png"));

            void SaveBoard(CellBoard boardToSave, int generation)
            {
                using var img = GenericVisualizer.RenderBinary(
                    dimensions,
                    isTrue: p => boardToSave[p] == CellType.Alive,
                    trueColor: SKColors.DarkSlateBlue,
                    falseColor: SKColors.White,
                    scale: ImageScale);
                SaveImage(img, generation, $"Cellular Generation {generation}");
            }

            int generation = 0;

            if (visualizeProcess)
            {
                SaveBoard(board, generation);
            }

            if (visualizeProcess)
            {
                for (int i = 0; i < 4; i++)
                {
                    generation++;
                    board = board.RunGenerations(1, CellRule.FiveNeighborsOrInEmptyArea);
                    SaveBoard(board, generation);
                }
                for (int i = 0; i < 3; i++)
                {
                    generation++;
                    board = board.RunGenerations(1, CellRule.FiveOrMoreNeighbors);
                    SaveBoard(board, generation);
                }
            }
            else
            {
                board = board.GenerateStandardCave();
            }

            // Find all components, render picture of all of them

            var step = 7;

            var components =
                ConnectedAreaAnalyzer
                    .FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
                    .OrderByDescending(component => component.Area)
                    .ToArray();

            Log($"Seed {seed} - {components.Length} connected areas found");

            using var componentsImg = new FastImage(board.Width, board.Height, scale: 10);
            componentsImg.Fill(SKColors.DarkSlateBlue);

            var largestComponent = components.First();
            foreach (var p in largestComponent)
            {
                componentsImg.SetPixel(p.X, p.Y, SKColors.White);
            }

            var hueShift = 360d / (components.Length - 1);

            double hue = 0;
            foreach (ConnectedArea c in components.Skip(1))
            {
                foreach (var p in c)
                {
                    componentsImg.SetPixel(p.X, p.Y, SKColor.FromHsl((float)hue, 100, 50));
                }

                hue += hueShift;
            }

            step++;
            if (visualizeProcess)
            {
                SaveImage(componentsImg, step, "All Components");
            }

            // Show just the largest component

            Log($"Seed {seed} - Area of largest component: {largestComponent.Area}");

            using var largestComponentImg = GenericVisualizer.RenderBinary(
                dimensions,
                isTrue: largestComponent.Contains,
                trueColor: SKColors.White,
                falseColor: SKColors.DarkSlateBlue,
                scale: ImageScale);

            step++;
            if (visualizeProcess)
            {
                SaveImage(largestComponentImg, step, "Largest Component");
            }

            // Find interior

            var distanceToEdge = largestComponent.DetermineInteriorEdgeDistance(Neighborhood.VonNeumann);

            using var interiorImg = GenericVisualizer.RenderPalette(
                dimensions,
                getColor: p =>
                {
                    if (!largestComponent.Contains(p))
                        return SKColors.DarkSlateBlue;
                    if (distanceToEdge.TryGetValue(p, out int d) && d == 0)
                        return SKColors.Gray;
                    return SKColors.White;
                },
                scale: ImageScale);

            step++;
            if (visualizeProcess)
            {
                SaveImage(interiorImg, step, "Interior");
            }

            // Place some lights
            var lightRange = new LightRange(DarkLevels: 15, LightLevels: 5);
            var lights = CaveThingPlacement.RandomlyPlaceLights(
                    distanceToEdge.Where(pair => pair.Value == 2).Select(pair => pair.Key).ToList(),
                    random,
                    lightRange,
                    percentAreaToCover: 0.05)
                .ToArray();

            Log($"Seed {seed} - Number of lights: {lights.Length}");

            foreach (var light in lights)
            {
                largestComponentImg.SetPixel(light.Center.X, light.Center.Y, light.Height switch
                {
                    LightHeight.Ceiling => SKColors.Orange,
                    LightHeight.Middle => SKColors.HotPink,
                    LightHeight.Floor => SKColors.Red,
                    _ => throw new Exception("Impossible")
                });
            }
            if(visualizeProcess)
            {
                step++;
                SaveImage(largestComponentImg, step, "Light Positions");
            }

            var (floorLighting, _) =
                LightTracer.Trace(dimensions, p => board[p] == CellType.Alive, lightRange, lights);

            using var lightImg = LightMapVisualizer.Render(floorLighting, lights, largestComponent);

            if (visualizeProcess)
            {
                step++;
                SaveImage(lightImg, step, "Lighting");
            } 

            // Place treasure
            var treasures =
                CaveThingPlacement.RandomlyPlaceTreasure(
                    area: largestComponent,
                    edge: distanceToEdge.Where(p => p.Value == 0).Select(p => p.Key),
                    floorLighting: floorLighting,
                    lightRange: lightRange,
                    random: random);

            foreach (var t in treasures)
            {
                lightImg.SetPixel(t.Location.X, t.Location.Y, SKColors.Gold);
            }

            SaveImage(lightImg, ++step, "Treasure");

            Log($"Seed {seed} - Complete");
        }
    }
}