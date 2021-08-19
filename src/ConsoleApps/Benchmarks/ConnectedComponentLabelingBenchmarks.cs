// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;

namespace Benchmarks
{
    public class ConnectedComponentLabelingBenchmarks
    {
        private const int Seed = 3;
        private readonly Size _dimensions = new(1024, 1024);
        private CellBoard _board;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(Seed);
            _board =
                new CellBoard(_dimensions)
                    .Fill(random, probabilityAlive: 0.6)
                    .MakeBorderAlive(thickness: 3)
                    .RunGenerations(6, minAliveNeighborsToLive: 5);
        }

        [Benchmark]
        public int Baseline()
        {
            return ConnectedComponentAnalyzer
                .FindEmptyAreas(_board.Dimensions, p => _board[p] == CellType.Dead).Count();
        }

        //[Benchmark]
        //public int Tweaks()
        //{
        //    return ConnectedComponentAnalyzer
        //        .FindEmptyAreas2(_board.Dimensions, p => _board[p] == CellType.Dead).Count();
        //}
    }
}