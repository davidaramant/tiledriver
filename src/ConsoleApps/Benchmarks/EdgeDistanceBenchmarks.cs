using BenchmarkDotNet.Attributes;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;

namespace Benchmarks;

public class EdgeDistanceBenchmarks
{
	private ConnectedArea _area = default!;

	[GlobalSetup]
	public void Setup()
	{
		var random = new Random(3);
		var board = new CellBoard(new Size(256, 256))
			.Fill(random, probabilityAlive: 0.5)
			.MakeBorderAlive(thickness: 1)
			.GenerateStandardCave()
			.ScaleAndSmooth(times: 3)
			.TrimToLargestDeadArea();

		_area = ConnectedAreaAnalyzer
			.FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
			.MaxBy(a => a.Area)!;
	}

	[Benchmark]
	public IReadOnlyDictionary<Position, int> Baseline() => _area.DetermineInteriorEdgeDistance(Neighborhood.Moore);

	//[Benchmark]
	//public IReadOnlyDictionary<Position, int> Tweaked() => _area.DetermineDistanceToEdges2(Neighborhood.Moore);
}
