﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using Tiledriver.Core.Extensions.Colors;
using Tiledriver.Core.Extensions.Enumerable;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CaveGeneration;
using Tiledriver.Core.LevelGeometry.CellularAutomata;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.LevelGeometry.Lighting;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.ConnectedComponentLabeling.Extensions;
using Tiledriver.Core.Utils.Images;
using Xunit;
using Xunit.Abstractions;

namespace Tiledriver.Core.Tests.LevelGeometry
{
    public sealed class CaveGenerationManualVisualizationTests
    {
        private readonly ITestOutputHelper _output;

        public CaveGenerationManualVisualizationTests(ITestOutputHelper output) => _output = output;

        [Fact]
        public void VisualizeGenerations()
        {
            const int generations = 6;
            const int seed = 3;
            Size dimensions = new(128, 128);

            var random = new Random(seed);
            var board =
                new CellBoard(dimensions)
                    .Fill(random, probabilityAlive: 0.6)
                    .MakeBorderAlive(thickness: 3);

            DirectoryInfo dirInfo = Directory.CreateDirectory(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Cellular Automata Generation Visualizations")) ?? throw new Exception("Could not create directory");

            void SaveImage(IFastImage image, int step, string description) =>
                image.Save(Path.Combine(dirInfo.FullName, $"Step {step:00} - {description}.png"));

            void SaveBoard(CellBoard boardToSave)
            {
                using var img = GenericVisualizer.RenderBinary(
                    dimensions,
                    isTrue: p => boardToSave[p] == CellType.Alive,
                    trueColor: SKColors.DarkSlateBlue,
                    falseColor: SKColors.White);
                SaveImage(img, boardToSave.Generation, $"Cellular Generation {boardToSave.Generation}");
            }

            SaveBoard(board);

            for (int i = 0; i < generations; i++)
            {
                board = board.RunGenerations(1, minAliveNeighborsToLive: 5);
                SaveBoard(board);
            }

            // Find all components, render picture of all of them

            var step = generations;

            var components =
                ConnectedComponentAnalyzer
                    .FindEmptyAreas(board.Dimensions, p => board[p] == CellType.Dead)
                    .ToArray();

            _output.WriteLine($"{components.Length} connected areas found");

            using var componentsImg = new FastImage(board.Width, board.Height, scale: 10);
            componentsImg.Fill(SKColors.Black);

            var hueShift = 360d / components.Length;

            double hue = 0;
            foreach (ConnectedArea c in components)
            {
                var color = (hue, 100d, 50d).ToSKColor();

                foreach (var p in c)
                {
                    componentsImg.SetPixel(p.X, p.Y, color);
                }

                hue += hueShift;
            }

            SaveImage(componentsImg, ++step, "All Components");

            // Show just the largest component

            var largestComponent = components.MaxElement(c => c.Area) ?? throw new Exception("No components??");
            _output.WriteLine($"Area of largest component: {largestComponent.Area}");

            using var largestComponentImg = GenericVisualizer.RenderBinary(
                dimensions,
                isTrue: largestComponent.Contains,
                trueColor: SKColors.White,
                falseColor: SKColors.DarkSlateBlue);

            SaveImage(largestComponentImg, ++step, "Largest Component");

            // Place some lights
            var lightRange = new LightRange(DarkLevels: 15, LightLevels: 15);
            var lights = CaveThingPlacement.RandomlyPlaceLights(
                largestComponent,
                random,
                lightRange,
                percentAreaToCover: 0.008)
                .ToArray();

            _output.WriteLine($"Number of lights: {lights.Length}");

            var (floorLighting, _) = LightTracer.Trace(dimensions, p => board[p] == CellType.Alive, lightRange, lights);

            using var lightImg = LightMapVisualizer.Render(floorLighting, lights, largestComponent);

            SaveImage(lightImg, ++step, "Lighting");

            // Place treasure
            var treasures = CaveThingPlacement.RandomlyPlaceTreasure(largestComponent, floorLighting, lightRange, random);

            foreach (var t in treasures)
            {
                lightImg.SetPixel(t.Location.X, t.Location.Y, SKColors.Gold);
            }

            SaveImage(lightImg, ++step, "Treasure");

            // Find interior

            var edge = largestComponent.Where(p => largestComponent.CountAdjacentWalls(p) > 0).ToHashSet();
            var edge2 = largestComponent.Where(p => p.GetMooreNeighbors().Any(edge.Contains)).ToHashSet();

            using var interiorImg = GenericVisualizer.RenderPalette(
                dimensions,
                getColor: p =>
                {
                    if (!largestComponent.Contains(p))
                        return SKColors.DarkSlateBlue;
                    if (edge2.Contains(p))
                        return SKColors.Gray;
                    return SKColors.White;
                });

            SaveImage(interiorImg, ++step, "Interior");
        }
    }
}
