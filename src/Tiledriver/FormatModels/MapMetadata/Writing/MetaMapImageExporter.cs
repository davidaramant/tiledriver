using Tiledriver.Utils.Images;

namespace Tiledriver.FormatModels.MapMetadata.Writing;

public sealed class MetaMapImageExporter
{
	public static void Export(MetaMap map, MapPalette palette, string outputFilePath, int scale = 1)
	{
		using var image = new FastImage(map.Width, map.Height);
		for (var tileY = 0; tileY < map.Height; tileY++)
		{
			for (var tileX = 0; tileX < map.Width; tileX++)
			{
				var tileColor = palette.PickColor(map[tileX, tileY]);
				image.SetColor(tileX, tileY, tileColor);
			}
		}

		image.Save(outputFilePath, scale: scale);
	}
}
