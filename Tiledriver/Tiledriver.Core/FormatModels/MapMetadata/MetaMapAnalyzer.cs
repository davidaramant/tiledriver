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
            var allDoorLocations =
                mapData.Triggers
                    .Where(t => t.Action == ActionSpecial.DoorOpen || t.Action == ActionSpecial.PushwallMove)
                    .Select(t => new Point(t.X, t.Y))
                    .ToImmutableHashSet();

            TileSpace GetSpace(int x, int y) => mapData.PlaneMaps[0].TileSpaces[y * mapData.Width + x];

            TileType GetTileType(Point p)
            {
                var tileSpace = mapData.PlaneMaps[0].TileSpaces[p.Y * mapData.Width + p.X];
                if (!tileSpace.HasTile)
                    return TileType.Empty;

                if (allDoorLocations.Contains(p))
                    return TileType.Door;

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
                        spotsToCheck.Enqueue(leftSide.Left());
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
                            // This can only happen with back-to-back doors/push walls.  
                            // Who knows which direction that came from, so check them all.
                            spotsToCheck.AddAllSurrounding(currentSpot);
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
                            switch (GetTileType(pointAbove))
                            {
                                case TileType.Empty:
                                    emptySpanAbove = true;
                                    spotsToCheck.Enqueue(pointAbove);
                                    break;
                                case TileType.Door:
                                    spotsToCheck.Enqueue(pointAbove.Above());
                                    metaMap[pointAbove] = TileType.Door;
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
                            switch (GetTileType(pointAbove))
                            {
                                case TileType.Empty:
                                    break;
                                case TileType.Door:
                                    spotsToCheck.Enqueue(pointAbove.Above());
                                    metaMap[pointAbove] = TileType.Door;
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
                            switch (GetTileType(pointBelow))
                            {
                                case TileType.Empty:
                                    emptySpanBelow = true;
                                    spotsToCheck.Enqueue(pointBelow);
                                    break;
                                case TileType.Door:
                                    spotsToCheck.Enqueue(pointBelow.Below());
                                    metaMap[pointBelow] = TileType.Door;
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
                            switch (GetTileType(pointBelow))
                            {
                                case TileType.Empty:
                                    break;
                                case TileType.Door:
                                    spotsToCheck.Enqueue(pointBelow.Below());
                                    metaMap[pointBelow] = TileType.Door;
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
                if (x < mapData.Width)
                {
                    var rightSide = new Point(x, spot.Y);
                    var type = GetTileType(rightSide);
                    metaMap[rightSide] = type;
                    if (type == TileType.Door)
                    {
                        spotsToCheck.Enqueue(rightSide.Right());
                    }
                }
            }

            return metaMap;
        }
    }
}