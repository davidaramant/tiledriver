// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SkiaSharp;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

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

		var trials = (
			from seed in Enumerable.Range(0, 3)
			from size in new[] { 96, 128 }
			from probAlive in new[] { 0.48, 0.5, 0.52 }
			select (seed, size, probAlive)
		).ToArray();

		Parallel.ForEach(
			trials,
			trial =>
			{
				var random = new Random(trial.seed);

				var board = new CellBoard(new Size(trial.size, trial.size))
					.Fill(random, probabilityAlive: trial.probAlive)
					.MakeBorderAlive(thickness: 1)
					.GenerateStandardCave()
					.TrimToLargestDeadArea()
					.ScaleAndSmooth()
					.ScaleAndSmooth();

				using var img = Visualize(board, showOnlyLargestArea: false);

				img.Save(
					Path.Combine(
						path,
						$"Size {trial.size}" + $" - ProbAlive {trial.probAlive:F2}" + $" - Seed {trial.seed}" + $".png"
					)
				);
			}
		);
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

		var random = new Random(0);

		var board = new CellBoard(new Size(128, 128))
			.Fill(random, probabilityAlive: 0.5)
			.MakeBorderAlive(thickness: 1)
			.GenerateStandardCave();

		Save(board, "1. board", 8);

		const int scalingIterations = 3;

		CellBoard last = board;

		foreach (var noise in new[] { 0, 0.05, 0.1, 0.15, 0.2 })
		{
			CellBoard scaled = board;

			foreach (int scalingIteration in Enumerable.Range(1, scalingIterations))
			{
				scaled = scaled.Quadruple().AddNoise(random, noise).RunGenerations(1);

				Save(
					scaled,
					$"{scalingIteration + 1}. board {1 << scalingIteration}x - noise {noise:F2}",
					8 / (1 << scalingIteration)
				);

				last = scaled;
			}
		}
	}

	static IFastImage Visualize(CellBoard board, bool showOnlyLargestArea, int scale = 1)
	{
		if (showOnlyLargestArea)
		{
			var (area, size) =
				ConnectedAreaAnalyzer
					.FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
					.MaxBy(component => component.Area)
					?.TrimExcess(1) ?? throw new InvalidOperationException("This can't happen");

			return GenericVisualizer.RenderBinary(
				size,
				isTrue: area.Contains,
				trueColor: SKColors.White,
				falseColor: SKColors.Black,
				scale: scale
			);
		}
		else
		{
			return GenericVisualizer.RenderBinary(
				board.Dimensions,
				isTrue: p => board[p] == CellType.Dead,
				trueColor: SKColors.White,
				falseColor: SKColors.Black,
				scale: scale
			);
		}
	}
}
