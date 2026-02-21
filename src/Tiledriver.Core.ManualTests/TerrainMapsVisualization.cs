using Tiledriver.Core.LevelGeometry.TerrainMaps;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public sealed class TerrainMapsVisualization
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Terrain Maps");

	void SaveImage(IFastImage image, string description) =>
		image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

	[Test, Explicit]
	public void DrawImage()
	{
		using var image = TerrainMapGenerator.Create(width: 2048, height: 2048);
		SaveImage(image, "Test");
	}
}
