// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Tests.FormatModels.MapMetadata
{
    [TestFixture]
    public sealed class MetaMapAnalyzerTests
    {
        [Test]
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

        [Test]
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

        private static void AssertMapAnalyzedCorrectly(char[,] shortHandMap, char[,] shortHandMetaMap)
        {
            var mapData = ExpandMapFromShorthand(shortHandMap);
            var metaMap = MetaMapAnalyzer.Analyze(mapData);

            for (int y = 0; y < mapData.Height; y++)
            {
                for (int x = 0; x < mapData.Width; x++)
                {
                    Assert.That(metaMap[x, y], Is.EqualTo(ExpandTile(shortHandMetaMap[y, x])), $"Unexpected tile type at ({x},{y})");
                }
            }
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
                    throw new AssertionException("Unknown character in shorthand meta map");
            }
        }

        private static MapData ExpandMapFromShorthand(char[,] shortHandMap)
        {
            var mapData = new MapData
            {
                Width = shortHandMap.GetLength(1),
                Height = shortHandMap.GetLength(0),
                Tiles =
                {
                    new Tile
                    {
                        Comment = "Wall",
                    },
                    new Tile
                    {
                        BlockingWest = false,
                        BlockingNorth = false,
                        BlockingEast = false,
                        BlockingSouth = false,
                        Comment = "Holowall",
                    }
                }
            };
            mapData.PlaneMaps.Add(new PlaneMap());

            for (int y = 0; y < mapData.Height; y++)
            {
                for (int x = 0; x < mapData.Width; x++)
                {
                    switch (shortHandMap[y, x])
                    {
                        case '*': // void
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: -1, sector: -1, zone: -1));
                            break;
                        case '#': // wall
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: 0, sector: 0, zone: -1));
                            break;
                        case ' ': // empty space
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: -1, sector: 0, zone: -1));
                            break;
                        case 'S': // start position
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: -1, sector: 0, zone: -1));
                            mapData.Things.Add(new Thing { X = x, Y = y, Type = Actor.Player1Start.ClassName });
                            break;
                        case 'D': // door
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: 0, sector: 0, zone: -1));
                            mapData.Triggers.Add(new Trigger { X = x, Y = y, Action = ActionSpecial.DoorOpen });
                            break;
                        case 'P': // pushwall
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: 0, sector: 0, zone: -1));
                            mapData.Triggers.Add(new Trigger { X = x, Y = y, Action = ActionSpecial.PushwallMove });
                            break;
                        default:
                            throw new AssertionException("Unknown character in shorthand map");
                    }
                }
            }

            return mapData;
        }
    }
}