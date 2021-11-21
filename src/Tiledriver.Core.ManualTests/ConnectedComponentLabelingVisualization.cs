// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SkiaSharp;
using Tiledriver.Core.Extensions.Enumerable;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests
{
    [TestFixture]
    public class ConnectedComponentLabelingVisualization
    {
        private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Connected Component Labeling");

        private static CellBoard MakeBoard()
        {
            const int generations = 6;
            const int seed = 3;
            Size dimensions = new(128, 128);

            var random = new Random(seed);
            return
                new CellBoard(dimensions)
                    .Fill(random, probabilityAlive: 0.6)
                    .MakeBorderAlive(thickness: 3)
                    .RunGenerations(generations);
        }

        void SaveImage(IFastImage image, string description) =>
            image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

        [Test, Explicit]
        public void VisualizeComponents()
        {
            var board = MakeBoard();

            var tests = new ( Func<Size, Func<Position, bool>, IEnumerable<ConnectedArea>> Finder, string Description)[]
            {
                (ConnectedAreaAnalyzer.FindForegroundAreas, "Original"),
                //(ConnectedAreaAnalyzer.FindEmptyAreas2, "New"),
            };

            List<ConnectedArea[]> results = new(2);

            foreach (var (Finder, Description) in tests)
            {
                var components =
                    Finder(board.Dimensions, p => board[p] == CellType.Dead)
                        .OrderByDescending(component => component.Area)
                        .ToArray();

                results.Add(components);

                Console.Out.WriteLine($"{components.Length} connected areas found");

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

                SaveImage(componentsImg, Description);
            }
        }

        [Test, Explicit]
        public void DistanceToEdges()
        {
            var board = MakeBoard();
            var largestComponent =
                ConnectedAreaAnalyzer
                    .FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
                    .MaxBy(component => component.Area) ??
                throw new InvalidOperationException("This can't happen");

            foreach (var neighborhood in new[] { Neighborhood.Moore, Neighborhood.VonNeumann })
            {
                var interiorInfo = largestComponent.DetermineInteriorEdgeDistance(neighborhood);
                var maxDistance = interiorInfo.Values.Max();

                Console.Out.WriteLine($"Max Distance for {neighborhood} Neighborhood: {maxDistance}");

                var palette =
                    Enumerable
                        .Range(0, maxDistance + 1)
                        .Select(d => SKColor.FromHsv(0, 0, 100f * ((float)d / maxDistance)))
                        .ToArray();

                var image = GenericVisualizer.RenderPalette(
                    board.Dimensions,
                    p =>
                        interiorInfo.TryGetValue(p, out var distance)
                        ? palette[distance]
                        : SKColors.DarkSlateGray,
                    scale: 5);

                SaveImage(image, $"Distance - {neighborhood} Neighborhood");
            }
        }
    }
}