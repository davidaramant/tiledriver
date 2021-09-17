// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SkiaSharp;
using Tiledriver.Core.Extensions.Enumerable;
using Tiledriver.Core.LevelGeometry;
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
                    from size in new[] { 128, 192 }
                    from probAlive in new[] { 0.48, 0.5, 0.52 }
                    select (seed, size, probAlive)).ToArray();

            const bool showOnlyTheLargestArea = true;

            Parallel.ForEach(trials, trial =>
            {
                var random = new Random(trial.seed);

                var board =
                    new CellBoard(new Size(trial.size, trial.size))
                        .Fill(random, probabilityAlive: trial.probAlive)
                        .MakeBorderAlive(thickness: 3)
                        .GenerateStandardCave();

                using var img = Visualize(board, showOnlyTheLargestArea);

                img.Save(Path.Combine(path, $"Size {trial.size}" +
                                            $" - ProbAlive {trial.probAlive:F2}" +
                                            $" - Seed {trial.seed}" +
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

            const bool showOnlyTheLargestArea = false;

            var random = new Random();

            var board =
                new CellBoard(new Size(128, 128))
                    .Fill(random, probabilityAlive: 0.5)
                    .MakeBorderAlive(thickness: 3)
                    .GenerateStandardCave();

            foreach (var scale in new[] { 1, 2, 3 })
            {
                using var img = Visualize(board.Scale(scale), showOnlyTheLargestArea);

                img.Save(Path.Combine(path, $"cave x{scale}.png"));
            }
        }

        IFastImage Visualize(CellBoard board, bool showOnlyLargestArea)
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
                    falseColor: SKColors.Black);
            }
            else
            {
                return GenericVisualizer.RenderBinary(board.Dimensions,
                    isTrue: p => board[p] == CellType.Dead,
                    trueColor: SKColors.White,
                    falseColor: SKColors.Black);
            }
        }
    }
}