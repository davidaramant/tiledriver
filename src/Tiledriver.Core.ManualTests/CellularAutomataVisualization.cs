// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using NUnit.Framework;
using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tiledriver.Core.Extensions.Enumerable;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests
{
    public class CellularAutomataVisualization
    {
        [Test, Explicit]
        public void RenderABunchOfOptions()
        {
            var dir = OutputLocation.CreateDirectory("Cellular Automata Trials");

            foreach (var file in dir.GetFiles())
            {
                file.Delete();
            }

            var path = dir.FullName;

            var trials =
                (from seed in Enumerable.Range(0, 3)
                 from size in new[] { 160 }
                 from probAlive in new[] { 0.5 }
                 from flipPercent in new[] { 0, 0.05, 0.075 }
                 select (seed, size, probAlive, flipPercent)).ToArray();

            Parallel.ForEach(trials, trial =>
            {
                var random = new Random(trial.seed);

                var board =
                    new CellBoard(new Size(trial.size, trial.size))
                        .Fill(random, probabilityAlive: trial.probAlive)
                        .MakeBorderAlive(thickness: 10)
                        .GenerateStandardCave()
                        .Quadruple();

                var (area, trimmedSize) =
                    ConnectedAreaAnalyzer
                        .FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
                        .MaxElement(component => component.Area)
                        ?.TrimExcess(1) ??
                    throw new InvalidOperationException("This can't happen");

                var interior = area.DetermineDistanceToEdges(Neighborhood.VonNeumann);
                var edge = interior.Where(pair => pair.Value == 0).Select(pair => pair.Key).OrderBy(x => random.Next()).ToList();

                var numToFlip = (int)(edge.Count * trial.flipPercent);
                var jaggedArea = new ConnectedArea(area.Except(edge.Take(numToFlip)));

                using var img = Visualize(trimmedSize, jaggedArea);

                img.Save(Path.Combine(path, $"Size {trial.size}" +
                                            $" - ProbAlive {trial.probAlive:F2}" +
                                            $" - Seed {trial.seed}" +
                                            $" - Flip {trial.flipPercent:P}" +
                                            $".png"));
            });
        }

        [Test, Explicit]
        public void ScaleBoard()
        {
            var dir = OutputLocation.CreateDirectory("Cellular Automata Scaling");

            foreach (var file in dir.GetFiles())
            {
                file.Delete();
            }

            var path = dir.FullName;

            void Save(CellBoard board, string name, int scale)
            {
                using var img = Visualize(board, false, scale);
                img.Save(Path.Combine(path, $"{name}.png"));
            }

            var random = new Random();

            var board =
                new CellBoard(new Size(128, 128))
                    .Fill(random, probabilityAlive: 0.5)
                    .MakeBorderAlive(thickness: 1)
                    .GenerateStandardCave();

            Save(board, "1. board", 8);

            var (largestArea, newSize) =
                ConnectedAreaAnalyzer
                    .FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
                    .MaxElement(component => component.Area)
                    ?.TrimExcess(1) ??
                throw new InvalidOperationException("This can't happen");

            var board2 = new CellBoard(newSize,
                typeAtPosition: p => largestArea.Contains(p) ? CellType.Dead : CellType.Alive);

            var board_x2 = board2.Quadruple();

            Save(board_x2, "2. board 2x", 4);

            board_x2.RunGenerations(1);

            Save(board_x2, "3. board 2x - smoothed", 4);

            var board_x4 = board_x2.Quadruple().RunGenerations(1);

            Save(board_x4, "4. board 4x - smoothed", 2);

            var board_x8 = board_x4.ScaleAndSmooth();

            Save(board_x8, "5. board 8x - smoothed", 1);
        }

        IFastImage Visualize(CellBoard board, bool showOnlyLargestArea, int scale = 1)
        {
            if (showOnlyLargestArea)
            {
                var (area, size) =
                    ConnectedAreaAnalyzer
                        .FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
                        .MaxElement(component => component.Area)
                        ?.TrimExcess(1) ??
                    throw new InvalidOperationException("This can't happen");

                return GenericVisualizer.RenderBinary(size,
                    isTrue: area.Contains,
                    trueColor: SKColors.White,
                    falseColor: SKColors.Black,
                    scale: scale);
            }
            else
            {
                return GenericVisualizer.RenderBinary(board.Dimensions,
                    isTrue: p => board[p] == CellType.Dead,
                    trueColor: SKColors.White,
                    falseColor: SKColors.Black,
                    scale: scale);
            }
        }

        IFastImage Visualize(Size size, ConnectedArea area)
        {
            return GenericVisualizer.RenderBinary(size, area.Contains, SKColors.White, SKColors.Black);
        }
    }
}