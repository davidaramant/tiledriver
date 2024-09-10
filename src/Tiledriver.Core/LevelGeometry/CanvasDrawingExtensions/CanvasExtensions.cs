// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry.CanvasDrawingExtensions;

public static class CanvasExtensions
{
	public static ICanvas FillRectangle(
		this ICanvas canvas,
		int? tile = null,
		int? sector = null,
		int? zone = null,
		int? tag = null
	) => canvas.FillRectangle(new Rectangle(new Position(0, 0), canvas.Dimensions), tile, sector, zone, tag);

	public static ICanvas FillRectangle(
		this ICanvas canvas,
		Rectangle area,
		int? tile = null,
		int? sector = null,
		int? zone = null,
		int? tag = null
	)
	{
		foreach (var row in Enumerable.Range(area.TopLeft.Y, area.Size.Height))
		{
			foreach (var col in Enumerable.Range(area.TopLeft.X, area.Size.Width))
			{
				canvas.Set(col, row, tile, sector, zone, tag);
			}
		}

		return canvas;
	}

	public static ICanvas OutlineRectangle(
		this ICanvas canvas,
		int? tile = null,
		int? sector = null,
		int? zone = null,
		int? tag = null
	) => canvas.OutlineRectangle(new Rectangle(new Position(0, 0), canvas.Dimensions), tile, sector, zone, tag);

	public static ICanvas OutlineRectangle(
		this ICanvas canvas,
		Rectangle area,
		int? tile = null,
		int? sector = null,
		int? zone = null,
		int? tag = null
	)
	{
		foreach (var col in Enumerable.Range(area.TopLeft.X, area.Size.Width))
		{
			canvas.Set(col, area.TopLeft.Y, tile, sector, zone, tag);
			canvas.Set(col, area.BottomRight.Y, tile, sector, zone, tag);
		}

		foreach (var row in Enumerable.Range(area.TopLeft.Y, area.Size.Height))
		{
			canvas.Set(area.TopLeft.X, row, tile, sector, zone, tag);
			canvas.Set(area.BottomRight.X, row, tile, sector, zone, tag);
		}

		return canvas;
	}
}
