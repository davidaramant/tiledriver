using Tiledriver.Core.LevelGeometry.CaveGeneration.Wolf;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.LevelGeometry.Lighting;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public class LightTracerVisualization
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Light Tracer");
	private const int Seed = 13;

	[Test, Explicit]
	public void ShowFloorVsCeilingLighting()
	{
		void SaveImage(IFastImage image, string description) =>
			image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

		var random = new Random(Seed);
		var board = new CellBoard(new(128, 128))
			.Fill(random, probabilityAlive: 0.5)
			.MakeBorderAlive(thickness: 3)
			.GenerateStandardCave();

		var (largestComponent, dimensions) = ConnectedAreaAnalyzer
			.FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
			.OrderByDescending(component => component.Area)
			.First()
			.TrimExcess(border: 1);

		var interior = largestComponent.DetermineInteriorEdgeDistance(Neighborhood.VonNeumann);

		// Place some lights
		var lightRange = new LightRange(DarkLevels: 15, LightLevels: 5);
		var lights = CaveThingPlacement
			.RandomlyPlaceLights(
				interior.Where(pair => pair.Value == 2).Select(pair => pair.Key).ToList(),
				random,
				lightRange,
				percentAreaToCover: 0.05,
				varyHeight: true
			)
			.ToArray();

		var (floorLighting, ceilingLight) = LightTracer.Trace(
			dimensions,
			p => !largestComponent.Contains(p),
			lightRange,
			lights
		);

		using var floorImg = LightMapVisualizer.Render(floorLighting, lights, largestComponent, scale: 5);
		using var ceilingImg = LightMapVisualizer.Render(ceilingLight, lights, largestComponent, scale: 5);

		SaveImage(floorImg, "Floor");
		SaveImage(ceilingImg, "Ceiling");
	}
}
