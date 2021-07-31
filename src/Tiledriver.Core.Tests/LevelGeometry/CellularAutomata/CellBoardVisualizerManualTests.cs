// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
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
        public void VisualizeGenerations()
        {
            const int Generations = 10;

            var random = new Random(0);
            var board =
                new CellBoard(new Size(128, 128))
                    .Fill(random, probabilityAlive: 0.6)
                    .MakeBorderAlive(thickness: 3);

            var dirInfo = Directory.CreateDirectory(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Cellular Automata Generation Visualizations"));

            void SaveBoard(CellBoard boardToSave, string prefix = "Generation")
            {
                using var img = CellBoardVisualizer.Render(boardToSave);
                img.Save(Path.Combine(dirInfo.FullName, $"{prefix} {boardToSave.Generation:00}.png"));
            }

            SaveBoard(board);

            for (int i = 0; i < Generations; i++)
            {
                board = board.RunGenerations(1, minAliveNeighborsToLive: 5);
                SaveBoard(board);
            }
        }
    }
}
