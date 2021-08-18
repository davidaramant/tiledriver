// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SkiaSharp;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests
{
    [TestFixture]
    public class ConnectedComponentLabeling
    {
        [Test, Explicit]
        public void VisualizeComponents()
        {
            const int generations = 6;
            const int seed = 3;
            Size dimensions = new(128, 128);

            var random = new Random(seed);
            var board =
                new CellBoard(dimensions)
                    .Fill(random, probabilityAlive: 0.6)
                    .MakeBorderAlive(thickness: 3)
                    .RunGenerations(generations, minAliveNeighborsToLive: 5);

            DirectoryInfo dirInfo = Directory.CreateDirectory(
                                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                            "CCL Visualizations")) ??
                                    throw new Exception("Could not create directory");

            void SaveImage(IFastImage image, string description) =>
                image.Save(Path.Combine(dirInfo.FullName, $"{description}.png"));


            var tests = new ( Func<Size, Func<Position, bool>, IEnumerable<ConnectedArea>> Finder, string Description)[]
            {
                (ConnectedComponentAnalyzer.FindEmptyAreas, "Original"),
                (ConnectedComponentAnalyzer.FindEmptyAreas2, "New"),
            };

            List<ConnectedArea[]> results = new(2);

            foreach (var test in tests)
            {

                var components =
                    test.Finder(board.Dimensions, p => board[p] == CellType.Dead)
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

                SaveImage(componentsImg, test.Description);
            }

            for (int areaIndex = 0; areaIndex < 25; areaIndex++)
            {
                var oldArea = results[0][areaIndex];
                var newArea = results[1][areaIndex];

                var oldNotInNew = oldArea.Except(newArea).ToArray();
                var newNotInOld = newArea.Except(oldArea).ToArray();

                if (oldNotInNew.Any() || newNotInOld.Any())
                {
                    Console.Out.WriteLine($"Area {areaIndex} differs");
                    Console.Out.WriteLine("Old not in new: " + string.Join(", ", oldNotInNew.Select(p=>p.ToString())));
                    Console.Out.WriteLine("New not in old: " + string.Join(", ", newNotInOld.Select(p=>p.ToString())));
                }
            }

        }
    }
}