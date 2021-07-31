// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using Tiledriver.Core.Extensions.Colors;
using Tiledriver.Core.Extensions.Enumerable;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;
using Tiledriver.Core.LevelGeometry.Lighting;
using Xunit;
using Xunit.Abstractions;

namespace Tiledriver.Core.Tests.LevelGeometry.CellularAutomata
{
    public sealed class CellBoardVisualizerManualTests
    {
        private readonly ITestOutputHelper _output;

        public CellBoardVisualizerManualTests(ITestOutputHelper output) => _output = output;

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
                using var img = CellBoardVisualizer.Render(boardToSave);
                SaveImage(img, boardToSave.Generation, $"Cellular Generation {boardToSave.Generation}");
            }

            SaveBoard(board);

            for (int i = 0; i < generations; i++)
            {
                board = board.RunGenerations(1, minAliveNeighborsToLive: 5);
                SaveBoard(board);
            }

            // Find all components, render picture of all of them

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

            SaveImage(componentsImg, generations + 1, "All Components");

            // Show just the largest component

            var largestComponent = components.MaxElement(c => c.Area) ?? throw new Exception("No components??");
            _output.WriteLine($"Area of largest component: {largestComponent.Area}");

            componentsImg.Fill(SKColors.Black);
            foreach (var p in largestComponent)
            {
                componentsImg.SetPixel(p.X, p.Y, SKColors.White);
            }

            SaveImage(componentsImg, generations + 2, "Largest Component");

            // Place some lights
            var lightRange = new LightRange(DarkLevels: 15, LightLevels: 15);

            const double percentAreaToCoverWithLights = 0.008;

            var numLights = (int)(largestComponent.Area * percentAreaToCoverWithLights);
            _output.WriteLine($"Number of lights: {numLights}");

            var lights =
                Enumerable
                    .Range(0, numLights)
                    .Select(_ =>
                    {
                        var posIndex = random.Next(0, largestComponent.Area);
                        var position = largestComponent.ElementAt(posIndex);
                        return new LightDefinition(
                            position,
                            Brightness: (int)(lightRange.LightLevels * 1.25),
                            Radius: 15);
                    })
                    .ToArray();

            var (floorLighting, _) = LightTracer.Trace(dimensions, p => board[p] == CellType.Alive, lightRange, lights);

            var lightImg = LightMapVisualizer.Render(floorLighting, lights);

            SaveImage(lightImg, generations + 3, $"Lighting");
        }
    }
}
