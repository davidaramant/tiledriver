using SkiaSharp;
using Tiledriver.DemoMaps.Wolf3D;
using Tiledriver.FormatModels.Uwmf;
using Tiledriver.LevelGeometry;
using Tiledriver.LevelGeometry.CaveGeneration.Wolf;
using Tiledriver.LevelGeometry.Extensions;
using Tiledriver.LevelGeometry.Lighting;
using Tiledriver.Utils.CellularAutomata;
using Tiledriver.Utils.ConnectedComponentLabeling;
using Tiledriver.Utils.Images;

namespace Tiledriver.ManualTests;

[TestFixture]
public class LightTracerVisualization
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Light Tracer");
	private const int Seed = 13;

	void SaveImage(IFastImage image, string description, int scale) =>
		image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"), scale);

	[Test, Explicit]
	public void ShouldGenerateVisualizationOfSimpleLightMap()
	{
		MapData map = TileDemoMap.Create();
		var (floorLights, _) = LightTracer.Trace(
			map,
			new LightRange(DarkLevels: 10, LightLevels: 10),
			[
				new(new Position(1, 1), Brightness: 20, Radius: 20),
				new(new Position(map.Width - 2, map.Height - 2), Brightness: 20, Radius: 20),
				new(new Position(map.Width - 2, 1), Brightness: 20, Radius: 20),
				new(new Position(1, map.Height - 2), Brightness: 20, Radius: 20),
			]
		);

		using var image = LightMapVisualizer.Render(floorLights);
		SaveImage(image, "Simple Light Map", scale: 20);
	}

	[Test, Explicit]
	public void ShowFloorVsCeilingLighting()
	{
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

		using var floorImg = LightMapVisualizer.Render(floorLighting, lights, largestComponent);
		using var ceilingImg = LightMapVisualizer.Render(ceilingLight, lights, largestComponent);

		SaveImage(floorImg, "Floor", scale: 5);
		SaveImage(ceilingImg, "Ceiling", scale: 5);
	}
}
