// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.FormatModels.MapMetadata.Writing
{
	public sealed class MetaMapImageExporter
	{
		public static void Export(MetaMap map, MapPalette palette, string outputFilePath, int scale = 1)
		{
			using var image = new FastImage(map.Width, map.Height, scale);
			for (var tileY = 0; tileY < map.Height; tileY++)
			{
				for (var tileX = 0; tileX < map.Width; tileX++)
				{
					var tileColor = palette.PickColor(map[tileX, tileY]);
					image.SetPixel(tileX, tileY, tileColor);
				}
			}

			image.Save(outputFilePath);
		}
	}
}
