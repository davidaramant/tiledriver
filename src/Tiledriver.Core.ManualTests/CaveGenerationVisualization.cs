// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using SkiaSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CaveGeneration;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.LevelGeometry.Lighting;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.ConnectedComponentLabeling.Extensions;
using Tiledriver.Core.Utils.Images;
using NUnit.Framework;
using Tiledriver.Core.Utils.CellularAutomata;

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
                visualizeProcess: true,
                generations: 6);
        }

        [Test, Explicit]
        public void ShowLotsOfSeeds()
        {
            const int generations = 6;
            const bool visualizeProcess = false;
            const string folderName = "Cave Seeds";

            Parallel.ForEach(Enumerable.Range(0, 100),
                seed => { CreateCave(seed, folderName, visualizeProcess, generations); });
        }

        private static void CreateCave(int seed, string folderName, bool visualizeProcess, int generations)
        {
            var stopWatch = Stopwatch.StartNew();

            void Log(string msg) => Console.WriteLine(stopWatch.Elapsed + ": " + msg);
            Log($"Seed {seed} - Start");
            Size dimensions = new(128, 128);

            var random = new Random(seed);
            var board =
                new CellBoard(dimensions)
                    .Fill(random, probabilityAlive: 0.6)
                    .MakeBorderAlive(thickness: 3);

            DirectoryInfo dirInfo = OutputLocation.CreateDirectory(folderName);

            void SaveImage(IFastImage image, int step, string description) =>
                image.Save(Path.Combine(dirInfo.FullName, $"Seed {seed:00} - Step {step:00} - {description}.png"));

            void SaveBoard(CellBoard boardToSave)
            {
                using var img = GenericVisualizer.RenderBinary(
                    dimensions,
                    isTrue: p => boardToSave[p] == CellType.Alive,
                    trueColor: SKColors.DarkSlateBlue,
                    falseColor: SKColors.White,
                    scale:ImageScale);
                SaveImage(img, boardToSave.Generation, $"Cellular Generation {boardToSave.Generation}");
            }

            if (visualizeProcess)
            {
                SaveBoard(board);
            }

            for (int i = 0; i < generations; i++)
            {
                board = board.RunGenerations(1, minAliveNeighborsToLive: 5);
                if (visualizeProcess)
                {
                    SaveBoard(board);
                }
            }

            // Find all components, render picture of all of them

            var step = generations;

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
                scale:ImageScale);

            step++;
            if (visualizeProcess)
            {
                SaveImage(largestComponentImg, step, "Largest Component");
            }

            // Find interior

            var distanceToEdge = largestComponent.DetermineDistanceToEdges(Neighborhood.VonNeumann);

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
                scale:ImageScale);

            step++;
            if (visualizeProcess)
            {
                SaveImage(interiorImg, step, "Interior");
            }

            // Place some lights
            var lightRange = new LightRange(DarkLevels: 15, LightLevels: 15);
            var lights = CaveThingPlacement.RandomlyPlaceLights(
                    largestComponent,
                    random,
                    lightRange,
                    percentAreaToCover: 0.008)
                .ToArray();

            Log($"Seed {seed} - Number of lights: {lights.Length}");

            var (floorLighting, _) =
                LightTracer.Trace(dimensions, p => board[p] == CellType.Alive, lightRange, lights);

            using var lightImg = LightMapVisualizer.Render(floorLighting, lights, largestComponent);

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

            SaveImage(lightImg, ++step, "Lighting & Treasure");

            Log($"Seed {seed} - Complete");
        }
    }
}