﻿// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Tests.FormatModels.MapMetadata
{
    public sealed class MetaMapAnalyzerTests
    {
        [Fact]
        public void ShouldAnalyzeSimpleRoom()
        {
            //    0   1   2   3   4   5   6   7   8   9   0
            var level = new[,]
            {
                {'#','#','#','#','#'},
                {'#',' ',' ',' ','#'},
                {'#',' ','S',' ','#'},
                {'#',' ',' ',' ','#'},
                {'#','#','#','#','#'},
            };
            var expectedOutput = new[,]
            {
                {'~','#','#','#','~'},
                {'#',' ',' ',' ','#'},
                {'#',' ',' ',' ','#'},
                {'#',' ',' ',' ','#'},
                {'~','#','#','#','~'},
            };
            AssertMapAnalyzedCorrectly(level, expectedOutput);
        }

        [Fact]
        public void ShouldHandleStackedDoors()
        {
            //    0   1   2   3   4   5   6   7   8   9   0
            var level = new[,]
            {
                {'#','#','#','#','#','#','#','#','#','#','#'},
                {'#','#','#','#','#',' ','#','#','#','#','#'},
                {'#','#','#','#','#','D','#','#','#','#','#'},
                {'#','#','#','#','#','D','#','#','#','#','#'},
                {'#','#','#','#','#',' ','#','#','#','#','#'},
                {'#',' ','D','D',' ','S',' ','D','D',' ','#'},
                {'#','#','#','#','#',' ','#','#','#','#','#'},
                {'#','#','#','#','#','D','#','#','#','#','#'},
                {'#','#','#','#','#','D','#','#','#','#','#'},
                {'#','#','#','#','#',' ','#','#','#','#','#'},
                {'#','#','#','#','#','#','#','#','#','#','#'},
            };
            var expectedOutput = new[,]
            {
                {'~','~','~','~','~','#','~','~','~','~','~'},
                {'~','~','~','~','#',' ','#','~','~','~','~'},
                {'~','~','~','~','~','D','~','~','~','~','~'},
                {'~','~','~','~','~','D','~','~','~','~','~'},
                {'~','#','~','~','#',' ','#','~','~','#','~'},
                {'#',' ','D','D',' ',' ',' ','D','D',' ','#'},
                {'~','#','~','~','#',' ','#','~','~','#','~'},
                {'~','~','~','~','~','D','~','~','~','~','~'},
                {'~','~','~','~','~','D','~','~','~','~','~'},
                {'~','~','~','~','#',' ','#','~','~','~','~'},
                {'~','~','~','~','~','#','~','~','~','~','~'},
            };
            AssertMapAnalyzedCorrectly(level, expectedOutput);
        }

        [Fact]
        public void ShouldHandleStackedPushwalls()
        {
            //    0   1   2   3   4   5   6   7   8   9   0
            var level = new[,]
            {
                {'#','#','#','#','#','#','#','#','#','#','#'}, // 0
                {'#','#','#','#','#',' ','#','#','#','#','#'}, // 1
                {'#','#','#','#','#','P','#','#','#','#','#'}, // 2
                {'#','#','#','#','#','P','#','#','#','#','#'}, // 3
                {'#','#','#','#','#',' ','#','#','#','#','#'}, // 4
                {'#',' ','P','P',' ','S',' ','P','P',' ','#'}, // 5
                {'#','#','#','#','#',' ','#','#','#','#','#'}, // 6
                {'#','#','#','#','#','P','#','#','#','#','#'}, // 7
                {'#','#','#','#','#','P','#','#','#','#','#'}, // 8
                {'#','#','#','#','#',' ','#','#','#','#','#'}, // 9
                {'#','#','#','#','#','#','#','#','#','#','#'}, // 0
            };
            var expectedOutput = new[,]
            {
                {'~','~','~','~','~','#','~','~','~','~','~'}, // 0
                {'~','~','~','~','#',' ','#','~','~','~','~'}, // 1
                {'~','~','~','~','#','P','#','~','~','~','~'}, // 2
                {'~','~','~','~','#','P','#','~','~','~','~'}, // 3
                {'~','#','#','#','#',' ','#','#','#','#','~'}, // 4
                {'#',' ','P','P',' ',' ',' ','P','P',' ','#'}, // 5
                {'~','#','#','#','#',' ','#','#','#','#','~'}, // 6
                {'~','~','~','~','#','P','#','~','~','~','~'}, // 7
                {'~','~','~','~','#','P','#','~','~','~','~'}, // 8
                {'~','~','~','~','#',' ','#','~','~','~','~'}, // 9
                {'~','~','~','~','~','#','~','~','~','~','~'}, // 0
            };
            //    0   1   2   3   4   5   6   7   8   9   0
            AssertMapAnalyzedCorrectly(level, expectedOutput);
        }

        [Fact]
        public void ShouldHandlePushwallsAfterDoors()
        {
            //    0   1   2   3   4   5   6   7   8   9   0
            var level = new[,]
            {
                {'#','#','#','#','#','#','#','#','#','#','#'}, // 0
                {'#','#','#','#','#',' ','#','#','#','#','#'}, // 1
                {'#','#','#','#','#','P','#','#','#','#','#'}, // 2
                {'#','#','#','#','#','D','#','#','#','#','#'}, // 3
                {'#','#','#','#','#',' ','#','#','#','#','#'}, // 4
                {'#',' ','P','D',' ','S',' ','D','P',' ','#'}, // 5
                {'#','#','#','#','#',' ','#','#','#','#','#'}, // 6
                {'#','#','#','#','#','D','#','#','#','#','#'}, // 7
                {'#','#','#','#','#','P','#','#','#','#','#'}, // 8
                {'#','#','#','#','#',' ','#','#','#','#','#'}, // 9
                {'#','#','#','#','#','#','#','#','#','#','#'}, // 0
            };
            var expectedOutput = new[,]
            {
                {'~','~','~','~','~','#','~','~','~','~','~'}, // 0
                {'~','~','~','~','#',' ','#','~','~','~','~'}, // 1
                {'~','~','~','~','#','P','#','~','~','~','~'}, // 2
                {'~','~','~','~','~','D','~','~','~','~','~'}, // 3
                {'~','#','#','~','#',' ','#','~','#','#','~'}, // 4
                {'#',' ','P','D',' ',' ',' ','D','P',' ','#'}, // 5
                {'~','#','#','~','#',' ','#','~','#','#','~'}, // 6
                {'~','~','~','~','~','D','~','~','~','~','~'}, // 7
                {'~','~','~','~','#','P','#','~','~','~','~'}, // 8
                {'~','~','~','~','#',' ','#','~','~','~','~'}, // 9
                {'~','~','~','~','~','#','~','~','~','~','~'}, // 0
            };
            //    0   1   2   3   4   5   6   7   8   9   0
            AssertMapAnalyzedCorrectly(level, expectedOutput);
        }

        [Fact]
        public void ShouldHandleFreeStandingPushwalls()
        {
            //    0   1   2   3   4   5   6
            var level = new[,]
            {
                {'#','#','#','#','#','#','#'}, // 0
                {'#','P',' ',' ',' ','P','#'}, // 1
                {'#',' ',' ','P',' ',' ','#'}, // 2
                {'#',' ','P','S','P',' ','#'}, // 3
                {'#',' ',' ','P',' ',' ','#'}, // 4
                {'#','P',' ',' ',' ','P','#'}, // 5
                {'#','#','#','#','#','#','#'}, // 6
            };
            var expectedOutput = new[,]
            {
                {'~','#','#','#','#','#','~'}, // 0
                {'#','P',' ',' ',' ','P','#'}, // 1
                {'#',' ',' ','P',' ',' ','#'}, // 2
                {'#',' ','P',' ','P',' ','#'}, // 3
                {'#',' ',' ','P',' ',' ','#'}, // 4
                {'#','P',' ',' ',' ','P','#'}, // 5
                {'~','#','#','#','#','#','~'}, // 6
            };
            //    0   1   2   3   4   5   6
            AssertMapAnalyzedCorrectly(level, expectedOutput);
        }

        private static void AssertMapAnalyzedCorrectly(char[,] shortHandMap, char[,] shortHandMetaMap)
        {
            var mapData = ExpandMapFromShorthand(shortHandMap);
            var metaMap = MetaMapAnalyzer.Analyze(mapData);

            var failures = new List<string>();

            for (int y = 0; y < mapData.Height; y++)
            {
                for (int x = 0; x < mapData.Width; x++)
                {
                    var actual = metaMap[x, y];
                    var expected = ExpandTile(shortHandMetaMap[y, x]);
                    if (actual != expected)
                    {
                        failures.Add($"({x},{y}): Expected {expected}, got {actual}");
                    }
                }
            }

            failures.Should().BeEmpty();
        }

        private static TileType ExpandTile(char shortHandType)
        {
            switch (shortHandType)
            {
                case '#': return TileType.Wall;
                case ' ': return TileType.Empty;
                case 'D': return TileType.Door;
                case 'P': return TileType.PushWall;
                case '~': return TileType.Unreachable;
                default:
                    throw new Exception("Unknown character in shorthand meta map");
            }
        }

        private static MapData ExpandMapFromShorthand(char[,] shortHandMap)
        {
            var width = shortHandMap.GetLength(1);
            var height = shortHandMap.GetLength(0);

            var planeMap = new List<MapSquare>();
            var things = ImmutableList.CreateBuilder<Thing>();
            var triggers = ImmutableList.CreateBuilder<Trigger>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    switch (shortHandMap[y, x])
                    {
                        case '*': // void
                            planeMap.Add(new MapSquare(Tile: -1, Sector: -1, Zone: -1));
                            break;
                        case '#': // wall
                            planeMap.Add(new MapSquare(Tile: 0, Sector: 0, Zone: -1));
                            break;
                        case ' ': // empty space
                            planeMap.Add(new MapSquare(Tile: -1, Sector: 0, Zone: -1));
                            break;
                        case 'S': // start position
                            planeMap.Add(new MapSquare(Tile: -1, Sector: 0, Zone: -1));
                            things.Add(new Thing(Type: Actor.Player1Start.ClassName, X: x, Y: y, Z: 0, Angle: 0));
                            break;
                        case 'D': // door
                            planeMap.Add(new MapSquare(Tile: 0, Sector: 0, Zone: -1));
                            triggers.Add(new Trigger(X: x, Y: y, Z: 0, Action: ActionSpecial.DoorOpen));
                            break;
                        case 'P': // pushwall
                            planeMap.Add(new MapSquare(Tile: 0, Sector: 0, Zone: -1));
                            triggers.Add(new Trigger(X: x, Y: y, Z: 0, Action: ActionSpecial.PushwallMove));
                            break;
                        default:
                            throw new Exception("Unknown character in shorthand map");
                    }
                }
            }

            return new MapData
            (
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "To Analyze",
                Width: width,
                Height: height,
                Tiles: new[]
                {
                    new Tile
                    (
                        TextureNorth: "GSTONEA1",
                        TextureSouth: "GSTONEA1",
                        TextureEast: "GSTONEA2",
                        TextureWest: "GSTONEA2",
                        Comment: "Wall"
                    ),
                    new Tile
                    (
                        TextureNorth: "GSTONEA1",
                        TextureSouth: "GSTONEA1",
                        TextureWest: "GSTONEA1",
                        TextureEast: "GSTONEA1",
                        BlockingNorth: false,
                        BlockingSouth: false,
                        BlockingWest: false,
                        BlockingEast: false,
                        Comment: "HoloWall"
                    ),
                }.ToImmutableList(),
                Sectors: ImmutableList.Create<Sector>().Add(
                    new Sector
                    (
                        TextureCeiling: "#C0C0C0",
                        TextureFloor: "#A0A0A0"
                    )),
                Zones: ImmutableList.Create<Zone>().Add(new Zone()),
                Planes: ImmutableList.Create<Plane>().Add(new Plane(Depth: 64)),
                PlaneMaps: ImmutableList.Create<ImmutableArray<MapSquare>>()
                    .Add(planeMap.ToImmutableArray()),
                Things: things.ToImmutableList(),
                Triggers: triggers.ToImmutableList()
            );
        }
    }
}