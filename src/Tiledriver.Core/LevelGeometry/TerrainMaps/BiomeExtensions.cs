using SkiaSharp;

namespace Tiledriver.Core.LevelGeometry.TerrainMaps;

public static class BiomeExtensions
{
	public static SKColor ToColor(this Biome biome) =>
		biome switch
		{
			Biome.Water => SKColors.RoyalBlue,
			Biome.Beach => SKColors.SandyBrown,
			Biome.Forest => SKColors.ForestGreen,
			Biome.Jungle => SKColors.DarkGreen,
			Biome.Savannah => SKColors.DarkGoldenrod,
			Biome.Desert => SKColors.Khaki,
			Biome.Snow => SKColors.White,
			Biome.BareRock => SKColors.DimGray,
			_ => throw new ArgumentOutOfRangeException(nameof(biome), biome, null),
		};
}
