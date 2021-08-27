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
            var path = OutputLocation.CreateDirectory("Cellular Automata Trials").FullName;
            const int minGeneration = 6;
            const int generationRange = 3;
            var trials =
                (from seed in Enumerable.Range(0, 3)
                from size in new[] { 128, 192 }
                from probAlive in new[]{0.59,0.6,0.61}
                select (seed, size, probAlive)).ToArray();

            Parallel.ForEach(trials, trial =>
            {
                var random = new Random(trial.seed);

                var board =
                    new CellBoard(new Size(trial.size, trial.size))
                        .Fill(random, probabilityAlive: trial.probAlive)
                        .MakeBorderAlive(thickness: 3)
                        .RunGenerations(minGeneration);

                for (int i = 0; i <= generationRange; i++)
                {
                    if (i > 0)
                    {
                        board.RunGenerations(1);
                    }

                    var (area,size) =
                        ConnectedAreaAnalyzer
                            .FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
                            .MaxElement(component => component.Area)
                            ?.TrimExcess(1) ??
                        throw new InvalidOperationException("This can't happen");

                    int generation = minGeneration + i;

                    using var img = GenericVisualizer.RenderBinary(size,
                        isTrue: area.Contains,
                        trueColor: SKColors.White,
                        falseColor: SKColors.Black);

                    img.Save(Path.Combine(path, $"Size {trial.size}" +
                                                $" - ProbAlive {trial.probAlive:F2}" +
                                                $" - Seed {trial.seed}" +
                                                $" - Generations {generation}" +
                                                $".png"));
                }
            });
        }
    }
}