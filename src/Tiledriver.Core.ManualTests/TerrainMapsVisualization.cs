// Copyright (c) 2026, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

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
