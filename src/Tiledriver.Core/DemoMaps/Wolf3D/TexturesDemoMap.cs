// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.GameInfo.Wolf3D;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CanvasDrawingExtensions;

namespace Tiledriver.Core.DemoMaps.Wolf3D;

public static class TexturesDemoMap
{
	private const int NumGradients = 10;
	private const int TotalNumLevels = NumGradients + 1 + NumGradients;
	private const int HorizontalBuffer = 5;
	private const int Height = 13;

	public static MapData Create() => CreateMapAndTextures(new TextureQueue());

	public static MapData CreateMapAndTextures(TextureQueue textureQueue)
	{
		var mapSize = new Size(HorizontalBuffer + TotalNumLevels + HorizontalBuffer, Height);

		var (planeMap, tiles) = CreateGeometry(mapSize, textureQueue);

		return new MapData(
			NameSpace: "Wolf3D",
			TileSize: 64,
			Name: "Textures Demo",
			Width: mapSize.Width,
			Height: mapSize.Height,
			Tiles: tiles,
			Sectors: ImmutableArray.Create(new Sector(TextureCeiling: "#C0C0C0", TextureFloor: "#A0A0A0")),
			Zones: ImmutableArray.Create(new Zone()),
			Planes: ImmutableArray.Create(new Plane(Depth: 64)),
			PlaneMaps: ImmutableArray.Create(planeMap),
			Things: ImmutableArray.Create(
				new Thing(
					Type: Actor.Player1Start.ClassName,
					X: 1.5,
					Y: 1.5,
					Z: 0,
					Angle: 0,
					Skill1: true,
					Skill2: true,
					Skill3: true,
					Skill4: true
				)
			),
			Triggers: ImmutableArray<Trigger>.Empty
		);
	}

	private static (ImmutableArray<MapSquare>, ImmutableArray<Tile>) CreateGeometry(
		Size size,
		TextureQueue textureQueue
	)
	{
		var baseTile = DefaultTile.GrayStone1;
		var nsTexture = baseTile.TextureNorth.Name;
		var ewTexture = baseTile.TextureEast.Name;

		var tiles = new List<Tile> { baseTile };

		var board = new Canvas(size)
			.FillRectangle(size.ToRectangle(), tile: -1)
			.OutlineRectangle(size.ToRectangle(), tile: 0);

		var middleX = HorizontalBuffer + NumGradients;
		var middleY = Height / 2;

		board.Set(middleX, middleY, tile: 1);

		foreach (var darkLevel in Enumerable.Range(1, NumGradients))
		{
			board.Set(middleX - darkLevel, middleY, tile: tiles.Count);

			var newNS = nsTexture + "Dark" + darkLevel;
			var newEW = ewTexture + "Dark" + darkLevel;

			tiles.Add(
				baseTile with
				{
					TextureNorth = newNS,
					TextureSouth = newNS,
					TextureEast = newEW,
					TextureWest = newEW,
				}
			);

			textureQueue.Add(
				new CompositeTexture(
					newNS,
					64,
					64,
					ImmutableArray.Create(
						new Patch(nsTexture, 0, 0),
						new Patch(nsTexture, 0, 0, Blend: new ColorBlend("000000", Alpha: 0.1 * darkLevel))
					)
				)
			);
			textureQueue.Add(
				new CompositeTexture(
					newEW,
					64,
					64,
					ImmutableArray.Create(
						new Patch(ewTexture, 0, 0),
						new Patch(ewTexture, 0, 0, Blend: new ColorBlend("000000", Alpha: 0.1 * darkLevel))
					)
				)
			);
		}

		foreach (var lightLevel in Enumerable.Range(1, NumGradients))
		{
			board.Set(middleX + lightLevel, middleY, tile: tiles.Count);

			var newNS = nsTexture + "Light" + lightLevel;
			var newEW = ewTexture + "Light" + lightLevel;

			tiles.Add(
				baseTile with
				{
					TextureNorth = newNS,
					TextureSouth = newNS,
					TextureEast = newEW,
					TextureWest = newEW,
				}
			);

			textureQueue.Add(
				new CompositeTexture(
					newNS,
					64,
					64,
					ImmutableArray.Create(
						new Patch(nsTexture, 0, 0),
						new Patch(nsTexture, 0, 0, Blend: new ColorBlend("FFFFFF", Alpha: 0.1 * lightLevel))
					)
				)
			);
			textureQueue.Add(
				new CompositeTexture(
					newEW,
					64,
					64,
					ImmutableArray.Create(
						new Patch(ewTexture, 0, 0),
						new Patch(ewTexture, 0, 0, Blend: new ColorBlend("FFFFFF", Alpha: 0.1 * lightLevel))
					)
				)
			);
		}

		return (board.ToPlaneMap(), tiles.ToImmutableArray());
	}
}
