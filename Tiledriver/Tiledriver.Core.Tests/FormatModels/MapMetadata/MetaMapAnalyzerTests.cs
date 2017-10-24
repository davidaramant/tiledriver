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
            var level = new char[,]
            {
                { '#', '#', '#', '#', '#'},
                { '#', ' ', ' ', ' ', '#'},
                { '#', ' ', 'S', ' ', '#'},
                { '#', ' ', ' ', ' ', '#'},
                { '#', '#', '#', '#', '#'},
            };
            var mapData = ExpandMapFromShorthand(level);

            var metaMap = MetaMapAnalyzer.Analyze(mapData);

            for (int x = 1; x < 4; x++)
            {
                AssertSpot(metaMap, x, 0, TileType.Wall);
                AssertSpot(metaMap, x, 4, TileType.Wall);
            }

            for (int y = 1; y < 4; y++)
            {
                AssertSpot(metaMap, 0, y, TileType.Wall);
                AssertSpot(metaMap, 4, y, TileType.Wall);
            }

            for (int y = 1; y < 4; y++)
            {
                for (int x = 1; x < 4; x++)
                {
                    AssertSpot(metaMap, x, y, TileType.Empty);
                }
            }
        }

        private static void AssertSpot(MetaMap metaMap, int x, int y, TileType expectedType)
        {
            Assert.That(metaMap[x, y], Is.EqualTo(expectedType), $"Unexpected tile type at ({x},{y})");
        }

        private static MapData ExpandMapFromShorthand(char[,] shortHandMap)
        {
            var mapData = new MapData
            {
                Width = shortHandMap.GetLength(1),
                Height = shortHandMap.GetLength(0)
            };
            mapData.PlaneMaps.Add(new PlaneMap());

            for (int y = 0; y < mapData.Height; y++)
            {
                for (int x = 0; x < mapData.Width; x++)
                {
                    switch (shortHandMap[y, x])
                    {
                        case '*':
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: -1, sector: -1, zone: -1));
                            break;
                        case '#':
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: 0, sector: 0, zone: -1));
                            break;
                        case ' ':
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: -1, sector: 0, zone: -1));
                            break;
                        case 'S':
                            mapData.PlaneMaps[0].TileSpaces.Add(new TileSpace(tile: -1, sector: 0, zone: -1));
                            mapData.Things.Add(new Thing { X = x, Y = y, Type = Actor.Player1Start.ClassName });
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