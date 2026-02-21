using SkiaSharp;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.LevelGeometry.TerrainMaps;

public static class TerrainMapGenerator
{
	public static IFastImage Create(int width, int height)
	{
		var image = new FastImage(width, height);
		image.Fill(SKColors.White);
		return image;
	}
}
