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

                return allDoorLocations.Contains(p) ? TileType.Door : TileType.Wall;
            }

            var bounds = new Size(mapData.Width, mapData.Height);
            var metaMap = new MetaMap(mapData.Width, mapData.Height);

            var spotsToCheck = new Queue<Point>();
            var passagesToCheck = new Queue<Point>();

            var startPos = mapData.Things.Single(t => t.Type == Actor.Player1Start.ClassName).GetPosition();

            spotsToCheck.Enqueue(startPos);

            do
            {
                if (passagesToCheck.Any())
                {
                    var passageLocation = passagesToCheck.Dequeue();
                    spotsToCheck.AddAllSurrounding(passageLocation);
                }

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
                        metaMap[leftSide] = GetTileType(leftSide);
                    }
                    x++;

                    bool emptySpanAbove = false;
                    bool emptySpanBelow = false;

                    bool inEmptySpace = true;
                    while (x < mapData.Width && inEmptySpace)
                    {
                        var currentSpot = new Point(x,spot.Y);
                        var type = GetTileType(currentSpot);
                        metaMap[currentSpot] = type;

                        switch (type)
                        {
                            case TileType.Empty:
                                break;
                            case TileType.Door:
                                passagesToCheck.Enqueue(currentSpot);
                                inEmptySpace = false;
                                break;
                            case TileType.Wall:
                                inEmptySpace = false;
                                break;
                            default:
                                throw new Exception("How can this happen?");
                        }

                        if (spot.Y > 0)
                        {
                            var pointAbove = new Point(x, spot.Y - 1);
                            if (!emptySpanAbove)
                            {
                                switch (GetTileType(pointAbove))
                                {
                                    case TileType.Empty:
                                        emptySpanAbove = true;
                                        spotsToCheck.Enqueue(pointAbove);
                                        break;
                                    case TileType.Door:
                                        passagesToCheck.Enqueue(pointAbove);
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
                                        passagesToCheck.Enqueue(pointAbove);
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
                            var pointBelow = new Point(x, spot.Y + 1);
                            if (!emptySpanBelow)
                            {
                                switch (GetTileType(pointBelow))
                                {
                                    case TileType.Empty:
                                        emptySpanBelow = true;
                                        spotsToCheck.Enqueue(pointBelow);
                                        break;
                                    case TileType.Door:
                                        passagesToCheck.Enqueue(pointBelow);
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
                                        passagesToCheck.Enqueue(pointBelow);
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
                        metaMap[rightSide] = GetTileType(rightSide);
                    }
                }
            } while (passagesToCheck.Any());

            return metaMap;
        }


    }
}