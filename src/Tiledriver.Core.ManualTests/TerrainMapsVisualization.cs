using SkiaSharp;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public class TerrainMapsVisualization
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Terrain Maps");

	void SaveImage(IFastImage image, string description) =>
		image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

	[Test, Explicit]
	public void DrawImage()
	{
		using var image = new FastImage(new(1024, 1024));
		image.Fill(SKColors.White);
		SaveImage(image, "Test");
	}
}
