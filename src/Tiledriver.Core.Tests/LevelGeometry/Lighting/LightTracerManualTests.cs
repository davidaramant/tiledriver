// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.DemoMaps.Wolf3D;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Lighting;
using Xunit;
using Xunit.Abstractions;

namespace Tiledriver.Core.Tests.LevelGeometry.Lighting;

public sealed class LightTracerManualTests
{
	private readonly ITestOutputHelper _output;

	public LightTracerManualTests(ITestOutputHelper output)
	{
		_output = output;
	}

	[Fact]
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
				new(new Position(1, map.Height - 2), Brightness: 20, Radius: 20)
			]
		);

		var image = LightMapVisualizer.Render(floorLights, scale: 20);
		image.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_output.png"));
	}
}
