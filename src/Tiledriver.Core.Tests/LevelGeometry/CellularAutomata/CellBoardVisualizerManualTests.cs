// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CellularAutomata;
using Xunit;
using Xunit.Abstractions;

namespace Tiledriver.Core.Tests.LevelGeometry.CellularAutomata
{
    public sealed class CellBoardVisualizerManualTests
    {
        private readonly ITestOutputHelper _output;

        public CellBoardVisualizerManualTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldVisualizeBoards()
        {
            var stopwatch = Stopwatch.StartNew();
            const int Generations = 3;

            var random = new Random(0);
            var board =
                new CellBoard(new Size(128, 128))
                    .Fill(random, probabilityAlive: 0.5)
                    .MakeBorderAlive(thickness: 2);

            _output.WriteLine($"Created board: {stopwatch.ElapsedMilliseconds}");

            var dirInfo = Directory.CreateDirectory(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Cellular Automata Generation Visualizations"));

            _output.WriteLine($"Started rendering board: {stopwatch.ElapsedMilliseconds}");
            using var img = CellBoardVisualizer.Render(board);
            _output.WriteLine($"Saving image: {stopwatch.ElapsedMilliseconds}");
            img.Save(Path.Combine(dirInfo.FullName, "Generation 0.png"));

            for (int i = 0; i < Generations; i++)
            {
                _output.WriteLine($"Running generation {i+1}: {stopwatch.ElapsedMilliseconds}");
                board = board.RunGenerations(1, minAliveNeighborsToLive: 5);
                _output.WriteLine($"Rendering generation {i+1}: {stopwatch.ElapsedMilliseconds}");
                using var genImg = CellBoardVisualizer.Render(board);
                _output.WriteLine($"Saving generation {i+1}: {stopwatch.ElapsedMilliseconds}");
                img.Save(Path.Combine(dirInfo.FullName, $"Generation {i + 1}.png"));
            }
        }
    }
}
