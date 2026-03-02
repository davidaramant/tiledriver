using SkiaSharp;

namespace Tiledriver.Core.LevelGeometry.TerrainMaps;

public static class BiomeExtensions
{
	public static SKColor ToColor(this Biome biome) =>
		biome switch
		{
			Biome.DeepWater => SKColor.Parse("37365E"),
			Biome.ShallowWater => SKColor.Parse("5E7CA2"),
			Biome.Taiga => SKColor.Parse("CED4BE"),
			Biome.Shrubland => SKColor.Parse("C6CCBC"),
			Biome.TemperateDesert => SKColor.Parse("E5E8CD"),
			Biome.TemperateRainForest => SKColor.Parse("AAC3AB"),
			Biome.TemperateDeciduousForest => SKColor.Parse("B8C8AC"),
			Biome.Grassland => SKColor.Parse("C8D3AE"),
			Biome.TropicalRainForest => SKColor.Parse("A2BAAA"),
			Biome.TropicalSeasonalForest => SKColor.Parse("B0CBA8"),
			Biome.SubtropicalDesert => SKColor.Parse("E7DEC9"),
			Biome.Snow => SKColor.Parse("F7F7F7"),
			Biome.Tundra => SKColor.Parse("DEDDBF"),
			Biome.Bare => SKColor.Parse("BBBBBB"),
			Biome.Scorched => SKColor.Parse("999999"),
			_ => throw new ArgumentOutOfRangeException(nameof(biome), biome, null),
		};
}
