// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.FormatModels.MapMetadata.Extensions;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.GameInfo.Wolf3D;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CoordinateSystems;

namespace Tiledriver.Core.FormatModels.MapMetadata;

public static class MetaMapAnalyzer
{
	private static readonly IPositionOffsets Position = CoordinateSystem.TopLeft;

	public static MetaMap Analyze(MapData mapData, bool includeAllEmptyAreas = false)
	{
		var doorLocations = mapData
			.Triggers.Where(t => t.Action == ActionSpecial.DoorOpen)
			.Select(t => new Position(t.X, t.Y))
			.ToImmutableHashSet();
		var pushWallLocations = mapData
			.Triggers.Where(t => t.Action == ActionSpecial.PushwallMove)
			.Select(t => new Position(t.X, t.Y))
			.ToImmutableHashSet();

		MapSquare GetSpace(int x, int y) => mapData.PlaneMaps[0][y * mapData.Width + x];

		TileType GetTileType(Position p)
		{
			var tileSpace = mapData.PlaneMaps[0][p.Y * mapData.Width + p.X];
			if (!tileSpace.HasTile)
				return TileType.Empty;

			if (doorLocations.Contains(p))
				return TileType.Door;

			if (pushWallLocations.Contains(p))
				return TileType.PushWall;

			var tile = mapData.Tiles[tileSpace.Tile];
			var isHoloWall = !tile.BlockingEast && !tile.BlockingNorth && !tile.BlockingWest && !tile.BlockingSouth;
			return isHoloWall ? TileType.Door : TileType.Wall;
		}

		var bounds = new Size(mapData.Width, mapData.Height);
		var metaMap = new MetaMap(mapData.Width, mapData.Height);

		var spotsToCheck = new Queue<Position>();

		// Do we start with the player position, or check every single empty spot in the map?
		if (!includeAllEmptyAreas)
		{
			spotsToCheck.Enqueue(mapData.Things.Single(t => t.Type == Actor.Player1Start.ClassName).GetPosition());
		}
		else
		{
			spotsToCheck.AddRange(
				Enumerable
					.Range(0, mapData.Width * mapData.Height)
					.Select(i => new Position(i % mapData.Width, i / mapData.Width))
					.Where(p => GetTileType(p) == TileType.Empty)
			);
		}

		while (spotsToCheck.Any())
		{
			var spot = spotsToCheck.Dequeue();
			if (!bounds.Contains(spot) || metaMap[spot] != TileType.Unreachable)
				continue;

			bool shouldStop = false;
			int x = spot.X;
			while (x >= 0 && !GetSpace(x, spot.Y).HasTile)
			{
				x--;
			}
			if (x >= 0)
			{
				var leftSide = new Position(x, spot.Y);
				var type = GetTileType(leftSide);
				metaMap[leftSide] = type;
				switch (type)
				{
					case TileType.Door:
						// Doors can be stacked - keep going!
						var left = leftSide + Position.Left;
						while (GetTileType(left) == TileType.Door)
						{
							metaMap[left] = TileType.Door;
							left += Position.Left;
						}
						spotsToCheck.Enqueue(left);
						break;
					case TileType.PushWall:
						spotsToCheck.Enqueue(leftSide + Position.Up);
						spotsToCheck.Enqueue(leftSide + Position.Left);
						spotsToCheck.Enqueue(leftSide + Position.Down);
						break;
					case TileType.Empty:
						break;
					case TileType.Wall:
						if (x == spot.X)
						{
							metaMap[spot] = TileType.Wall;
							shouldStop = true;
						}
						break;
				}
			}
			if (shouldStop)
				continue;
			x++;

			bool emptySpanAbove = false;
			bool emptySpanBelow = false;

			while (x < mapData.Width)
			{
				var currentSpot = new Position(x, spot.Y);
				var type = GetTileType(currentSpot);
				metaMap[currentSpot] = type;

				bool inEmptySpace = true;
				switch (type)
				{
					case TileType.Empty:
						break;
					case TileType.Door:
						// Doors can be stacked - keep going!
						var right = currentSpot + Position.Right;
						while (GetTileType(right) == TileType.Door)
						{
							metaMap[right] = TileType.Door;
							right += Position.Right;
						}
						spotsToCheck.Enqueue(right);
						inEmptySpace = false;
						break;
					case TileType.PushWall:
						spotsToCheck.Enqueue(currentSpot + Position.Up);
						spotsToCheck.Enqueue(currentSpot + Position.Right);
						spotsToCheck.Enqueue(currentSpot + Position.Down);
						inEmptySpace = false;
						break;
					case TileType.Wall:
						inEmptySpace = false;
						break;
					default:
						throw new Exception("How can this happen?");
				}
				if (!inEmptySpace)
					break;

				if (spot.Y > 0)
				{
					var pointAbove = currentSpot + Position.Up;
					if (!emptySpanAbove)
					{
						var tileType = GetTileType(pointAbove);
						switch (GetTileType(pointAbove))
						{
							case TileType.Empty:
								emptySpanAbove = true;
								spotsToCheck.Enqueue(pointAbove);
								break;
							case TileType.Door:
								metaMap[pointAbove] = tileType;
								// Doors can be stacked - keep going!
								var above = pointAbove + Position.Up;
								while (GetTileType(above) == TileType.Door)
								{
									metaMap[above] = TileType.Door;
									above += Position.Up;
								}
								spotsToCheck.Enqueue(above);
								break;
							case TileType.PushWall:
								spotsToCheck.Enqueue(pointAbove + Position.Left);
								spotsToCheck.Enqueue(pointAbove + Position.Up);
								spotsToCheck.Enqueue(pointAbove + Position.Right);
								metaMap[pointAbove] = tileType;
								break;
							case TileType.Wall:
								metaMap[pointAbove] = TileType.Wall;
								break;
							default:
								throw new Exception("How can this happen?");
						}
					}
					else
					{
						var tileType = GetTileType(pointAbove);
						switch (tileType)
						{
							case TileType.Empty:
								break;
							case TileType.Door:
								metaMap[pointAbove] = tileType;
								// Doors can be stacked - keep going!
								var above = pointAbove + Position.Up;
								while (GetTileType(above) == TileType.Door)
								{
									metaMap[above] = TileType.Door;
									above += Position.Up;
								}
								spotsToCheck.Enqueue(above);
								emptySpanAbove = false;
								break;
							case TileType.PushWall:
								spotsToCheck.Enqueue(pointAbove + Position.Left);
								spotsToCheck.Enqueue(pointAbove + Position.Up);
								spotsToCheck.Enqueue(pointAbove + Position.Right);
								metaMap[pointAbove] = tileType;
								emptySpanAbove = false;
								break;
							case TileType.Wall:
								metaMap[pointAbove] = TileType.Wall;
								emptySpanAbove = false;
								break;
							default:
								throw new Exception("How can this happen?");
						}
					}
				}

				if (spot.Y < mapData.Height - 1)
				{
					var pointBelow = currentSpot + Position.Down;
					if (!emptySpanBelow)
					{
						var tileType = GetTileType(pointBelow);
						switch (tileType)
						{
							case TileType.Empty:
								emptySpanBelow = true;
								spotsToCheck.Enqueue(pointBelow);
								break;
							case TileType.Door:
								metaMap[pointBelow] = tileType;
								// Doors can be stacked - keep going!
								var below = pointBelow + Position.Down;
								while (GetTileType(below) == TileType.Door)
								{
									metaMap[below] = TileType.Door;
									below += Position.Down;
								}
								spotsToCheck.Enqueue(below);
								break;
							case TileType.PushWall:
								spotsToCheck.Enqueue(pointBelow + Position.Left);
								spotsToCheck.Enqueue(pointBelow + Position.Down);
								spotsToCheck.Enqueue(pointBelow + Position.Right);
								metaMap[pointBelow] = tileType;
								break;
							case TileType.Wall:
								metaMap[pointBelow] = TileType.Wall;
								break;
							default:
								throw new Exception("How can this happen?");
						}
					}
					else
					{
						var tileType = GetTileType(pointBelow);
						switch (tileType)
						{
							case TileType.Empty:
								break;
							case TileType.Door:
								metaMap[pointBelow] = tileType;
								// Doors can be stacked - keep going!
								var below = pointBelow + Position.Down;
								while (GetTileType(below) == TileType.Door)
								{
									metaMap[below] = TileType.Door;
									below += Position.Down;
								}
								spotsToCheck.Enqueue(below);
								emptySpanBelow = false;
								break;
							case TileType.PushWall:
								spotsToCheck.Enqueue(pointBelow + Position.Left);
								spotsToCheck.Enqueue(pointBelow + Position.Down);
								spotsToCheck.Enqueue(pointBelow + Position.Right);
								metaMap[pointBelow] = tileType;
								emptySpanBelow = false;
								break;
							case TileType.Wall:
								metaMap[pointBelow] = TileType.Wall;
								emptySpanBelow = false;
								break;
							default:
								throw new Exception("How can this happen?");
						}
					}
				}

				x++;
			}
		}

		return metaMap;
	}
}
