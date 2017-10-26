// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using Tiledriver.Core.FormatModels.MapMetadata.Extensions;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.FormatModels.MapMetadata
{
    public static class MetaMapAnalyzer
    {
        public static MetaMap Analyze(MapData mapData)
        {
            var doorLocations =
                mapData.Triggers
                    .Where(t => t.Action == ActionSpecial.DoorOpen )
                    .Select(t => new Point(t.X, t.Y))
                    .ToImmutableHashSet();
            var pushWallLocations =
                mapData.Triggers
                    .Where(t => t.Action == ActionSpecial.PushwallMove)
                    .Select(t => new Point(t.X, t.Y))
                    .ToImmutableHashSet();

            TileSpace GetSpace(int x, int y) => mapData.PlaneMaps[0].TileSpaces[y * mapData.Width + x];

            TileType GetTileType(Point p)
            {
                var tileSpace = mapData.PlaneMaps[0].TileSpaces[p.Y * mapData.Width + p.X];
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

            var spotsToCheck = new Queue<Point>();
            spotsToCheck.Enqueue(mapData.Things.Single(t => t.Type == Actor.Player1Start.ClassName).GetPosition());

            while (spotsToCheck.Any())
            {
                var spot = spotsToCheck.Dequeue();
                if (!bounds.Contains(spot) || metaMap[spot] != TileType.Unreachable)
                    continue;

                int x = spot.X;
                while (x >= 0 && !GetSpace(x, spot.Y).HasTile)
                {
                    x--;
                }
                if (x >= 0)
                {
                    var leftSide = new Point(x, spot.Y);
                    var type = GetTileType(leftSide);
                    metaMap[leftSide] = type;
                    if (type == TileType.Door)
                    {
                        // Doors can be stacked - keep going!
                        var left = leftSide.Left();
                        while (GetTileType(left) == TileType.Door)
                        {
                            metaMap[left] = TileType.Door;
                            left = left.Left();
                        }
                        spotsToCheck.Enqueue(left);
                    }
                    else if (type == TileType.PushWall)
                    {
                        spotsToCheck.Enqueue(leftSide.Above());
                        spotsToCheck.Enqueue(leftSide.Left());
                        spotsToCheck.Enqueue(leftSide.Below());
                    }
                }
                x++;

                bool emptySpanAbove = false;
                bool emptySpanBelow = false;

                while (x < mapData.Width)
                {
                    var currentSpot = new Point(x, spot.Y);
                    var type = GetTileType(currentSpot);
                    metaMap[currentSpot] = type;

                    bool inEmptySpace = true;
                    switch (type)
                    {
                        case TileType.Empty:
                            break;
                        case TileType.Door:
                            // Doors can be stacked - keep going!
                            var right = currentSpot.Right();
                            while (GetTileType(right) == TileType.Door)
                            {
                                metaMap[right] = TileType.Door;
                                right = right.Right();
                            }
                            spotsToCheck.Enqueue(right);
                            inEmptySpace = false;
                            break;
                        case TileType.PushWall:
                            spotsToCheck.Enqueue(currentSpot.Above());
                            spotsToCheck.Enqueue(currentSpot.Right());
                            spotsToCheck.Enqueue(currentSpot.Below());
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
                        var pointAbove = currentSpot.Above();
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
                                    var above = pointAbove.Above();
                                    while (GetTileType(above) == TileType.Door)
                                    {
                                        metaMap[above] = TileType.Door;
                                        above = above.Above();
                                    }
                                    spotsToCheck.Enqueue(above);
                                    break;
                                case TileType.PushWall:
                                    spotsToCheck.Enqueue(pointAbove.Left());
                                    spotsToCheck.Enqueue(pointAbove.Above());
                                    spotsToCheck.Enqueue(pointAbove.Right());
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
                                    var above = pointAbove.Above();
                                    while (GetTileType(above) == TileType.Door)
                                    {
                                        metaMap[above] = TileType.Door;
                                        above = above.Above();
                                    }
                                    spotsToCheck.Enqueue(above);
                                    emptySpanAbove = false;
                                    break;
                                case TileType.PushWall:
                                    spotsToCheck.Enqueue(pointAbove.Left());
                                    spotsToCheck.Enqueue(pointAbove.Above());
                                    spotsToCheck.Enqueue(pointAbove.Right());
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
                        var pointBelow = currentSpot.Below();
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
                                    var below = pointBelow.Below();
                                    while (GetTileType(below) == TileType.Door)
                                    {
                                        metaMap[below] = TileType.Door;
                                        below = below.Below();
                                    }
                                    spotsToCheck.Enqueue(below);
                                    break;
                                case TileType.PushWall:
                                    spotsToCheck.Enqueue(pointBelow.Left());
                                    spotsToCheck.Enqueue(pointBelow.Below());
                                    spotsToCheck.Enqueue(pointBelow.Right());
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
                                    var below = pointBelow.Below();
                                    while (GetTileType(below) == TileType.Door)
                                    {
                                        metaMap[below] = TileType.Door;
                                        below = below.Below();
                                    }
                                    spotsToCheck.Enqueue(below);
                                    emptySpanBelow = false;
                                    break;
                                case TileType.PushWall:
                                    spotsToCheck.Enqueue(pointBelow.Left());
                                    spotsToCheck.Enqueue(pointBelow.Below());
                                    spotsToCheck.Enqueue(pointBelow.Right());
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
}