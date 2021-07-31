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

            var random = new Random(seed);
            var board =
                new CellBoard(new Size(128, 128))
                    .Fill(random, probabilityAlive: 0.6)
                    .MakeBorderAlive(thickness: 3);

            var dirInfo = Directory.CreateDirectory(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Cellular Automata Generation Visualizations"));

            void SaveBoard(CellBoard boardToSave, string prefix = "Step")
            {
                using var img = CellBoardVisualizer.Render(boardToSave);
                img.Save(Path.Combine(dirInfo.FullName, $"{prefix} {boardToSave.Generation:00}.png"));
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

            componentsImg.Save(Path.Combine(dirInfo.FullName, $"Step {board.Generation + 1:00} - All Components.png"));

            // Show just the largest component

            var largestComponent = components.MaxElement(c => c.Area) ?? throw new Exception("No components??");

            componentsImg.Fill(SKColors.Black);
            foreach (var p in largestComponent)
            {
                componentsImg.SetPixel(p.X, p.Y, SKColors.White);
            }

            componentsImg.Save(Path.Combine(dirInfo.FullName, $"Step {board.Generation + 2:00} - Largest Components.png"));
        }
    }
}
