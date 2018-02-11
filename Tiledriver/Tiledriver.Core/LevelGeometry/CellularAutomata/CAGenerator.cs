// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.CellularAutomata
{
    public static class CAGenerator
    {
        enum CellType
        {
            Empty,
            Rock
        }

        public static MapData Generate(int width = 64, int height = 64)
        {
            var geometry = CreateGeometry(width: width, height: height).ToList();
            var playerStartingPositionIndex = geometry.FindIndex(ts => !ts.HasTile);

            var map = new MapData
            (
                nameSpace: "Wolf3D",
                tileSize: 64,
                name: "Cave",
                width: width,
                height: height,
                comment: "",
                tiles: new[]
                {
                    new Tile
                    (
                        textureNorth: "brock2",
                        textureSouth: "brock2",
                        textureEast: "brock2a",
                        textureWest: "brock2a"
                    ),
                },
                sectors: new[]
                {
                    new Sector
                    (
                        textureCeiling: "br_dark",
                        textureFloor: "br"
                    ),
                },
                zones: new[]
                {
                    new Zone(),
                },
                planes: new[] { new Plane(depth: 64) },
                planeMaps: new[] { new PlaneMap(geometry) },
                things: new[]
                {
                    new Thing
                    (
                        type: Actor.Player1Start.ClassName,
                        x: (playerStartingPositionIndex%width) + 0.5,
                        y: (int)(playerStartingPositionIndex/width) + 0.5,
                        z: 0,
                        angle: 0,
                        skill1: true,
                        skill2: true,
                        skill3: true,
                        skill4: true
                    ),
                },
                triggers: Enumerable.Empty<Trigger>()
            );

            return map;
        }

        private static IEnumerable<TileSpace> CreateGeometry(int width, int height)
        {
            TileSpace SolidTile() => new TileSpace(tile: 0, sector: 0, zone: -1);
            TileSpace EmptyTile() => new TileSpace(tile: -1, sector: 0, zone: 0);
            var caBoard = RunCAGeneration(width: width, height: height, buildInitialBorder: true);

            var entries = new TileSpace[height, width];

            // ### Build a big empty square
            // Top wall
            for (var col = 0; col < width; col++)
            {
                entries[0, col] = SolidTile();
            }

            for (var row = 1; row < height - 1; row++)
            {
                entries[row, 0] = SolidTile();
                for (var col = 1; col < width - 1; col++)
                {
                    entries[row, col] = caBoard[row, col] == CellType.Empty ? EmptyTile() : SolidTile();
                }
                entries[row, width - 1] = SolidTile();
            }

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = SolidTile();
            }

            // Return tilespaces
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    yield return entries[row, col];
                }
            }
        }

        private static CellType[,] RunCAGeneration(int width, int height, bool buildInitialBorder)
        {
            var board = new CellType[height, width];

            var random = new Random(0);

            const double probabilityOfRock = 0.6;
            const int generations = 6;
            const int minRockNeighborsToLive = 5;

            const int borderOffset = 3;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (buildInitialBorder && col < borderOffset || col > (width - 1 - borderOffset) || row < borderOffset || row > (height - borderOffset))
                    {
                        board[row, col] = CellType.Rock;
                    }
                    else
                    {
                        board[row, col] = random.NextDouble() <= probabilityOfRock ? CellType.Rock : CellType.Empty;
                    }
                }
            }

            for (int gen = 0; gen < generations; gen++)
            {
                var nextBoard = new CellType[height, width];

                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        var rockNeighbors = CountRockNeighbors(board, row: row, col: col, width: width, height: height);

                        if (rockNeighbors < minRockNeighborsToLive)
                        {
                            nextBoard[row, col] = CellType.Empty;
                        }
                        else
                        {
                            nextBoard[row, col] = CellType.Rock;
                        }
                    }
                }

                board = nextBoard;
            }

            return board;
        }

        private static int CountRockNeighbors(CellType[,] board, int row, int col, int width, int height)
        {
            int count = 0;

            // Horizontal
            // left
            if (col > 0 && board[row, col - 1] == CellType.Rock)
            {
                count++;
            }
            // right
            if (col < width - 1 && board[row, col + 1] == CellType.Rock)
            {
                count++;
            }

            // Vertical
            // top
            if (row > 0 && board[row - 1, col] == CellType.Rock)
            {
                count++;
            }
            // bottom
            if (row < height - 1 && board[row + 1, col] == CellType.Rock)
            {
                count++;
            }

            // top left corner
            if (col > 0 && row > 0 && board[row - 1, col - 1] == CellType.Rock)
            {
                count++;
            }

            // top right corner
            if (col < width - 1 && row > 0 && board[row - 1, col + 1] == CellType.Rock)
            {
                count++;
            }

            // bottom left corner
            if (col > 0 && row < height - 1 && board[row + 1, col - 1] == CellType.Rock)
            {
                count++;
            }

            // bottom right corner
            if (col < width - 1 && row < height - 1 && board[row + 1, col + 1] == CellType.Rock)
            {
                count++;
            }

            return count;
        }
    }
}