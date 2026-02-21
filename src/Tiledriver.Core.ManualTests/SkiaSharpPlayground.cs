// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using SkiaSharp;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public sealed class SkiaSharpPlayground
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("SkiaSharp Playground");

	private void Save(IFastImage image, string name) => image.Save(Path.Combine(_dirInfo.FullName, name + ".png"));

	[Test, Explicit]
	public void HsvColor()
	{
		using var img = GenericVisualizer.RenderPalette(
			new Size(10, 10),
			pos => SKColor.FromHsv(0, 100 * (pos.X / 10f), 100 * (pos.Y / 10f)),
			scale: 20
		);

		Save(img, "HSV with Saturation x Value");
	}

	[Test, Explicit]
	public void HslColor()
	{
		using var img = GenericVisualizer.RenderPalette(
			new Size(10, 10),
			pos => SKColor.FromHsl(0, 100 * (pos.X / 10f), 100 * (pos.Y / 10f)),
			scale: 20
		);

		Save(img, "HSL with Saturation x Luminosity");
	}
}
