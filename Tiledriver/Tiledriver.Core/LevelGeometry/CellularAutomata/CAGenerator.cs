// Copyright (c) 2018, David Aramant, Grant Kimsey
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

        public static MapData Generate(int width = 128, int height = 128)
        {
            const double probabilityOfRock = 0.6;
            const int generations = 6;
            const int minRockNeighborsToLive = 5;
            const int borderOffset = 3;
            const double stalagmiteProb = 0.015;
            const double stalactiteProb = 0.03;

            var geometry = CreateGeometry(
                width,
                height,
                probabilityOfRock, 
                generations,
                minRockNeighborsToLive,
                borderOffset,
                stalagmiteProb, 
                stalactiteProb, 
                out var decorations).ToList();
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
                        textureNorth: "bw0a",
                        textureSouth: "bw0a",
                        textureEast: "bw0b",
                        textureWest: "bw0b"
                    ),
                    new Tile
                    (
                        textureNorth: "bw1a",
                        textureSouth: "bw1a",
                        textureEast: "bw1b",
                        textureWest: "bw1b"
                    ),
                    new Tile
                    (
                        textureNorth: "bw2a",
                        textureSouth: "bw2a",
                        textureEast: "bw2b",
                        textureWest: "bw2b"
                    ),
                    new Tile
                    (
                        textureNorth: "bw3a",
                        textureSouth: "bw3a",
                        textureEast: "bw3b",
                        textureWest: "bw3b"
                    ),
                },
                sectors: new[]
                {
                    new Sector
                    (
                        textureCeiling: "bf0",
                        textureFloor: "bf0"
                    ),
                    new Sector
                    (
                        textureCeiling: "bf1",
                        textureFloor: "bf1"
                    ),
                    new Sector
                    (
                        textureCeiling: "bf2",
                        textureFloor: "bf2"
                    ),
                    new Sector
                    (
                        textureCeiling: "bf3",
                        textureFloor: "bf3"
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
                    GenerateThingAtCoordinateCenter(
                        Actor.Player1Start.ClassName,
                        (int)(playerStartingPositionIndex % width),
                        (int)(playerStartingPositionIndex / width))
                }.Concat(decorations),
                triggers: Enumerable.Empty<Trigger>()
            );

            return map;
        }

        private static IEnumerable<TileSpace> CreateGeometry(
            int width, 
            int height, 
            double rockProb, 
            int generations, 
            int minRockNeighborsToLive,
            int boardBorderOffset,
            double stalagmiteProb, 
            double stalactiteProb, 
            out IEnumerable<Thing> decorations)
        {
            TileSpace SolidTile() => new TileSpace(tile: 0, sector: 0, zone: -1);
            TileSpace EmptyTile() => new TileSpace(tile: -1, sector: 0, zone: 0);

            var randomGenerator = new Random(0);
            var caBoard = RunCAGeneration(width, height, randomGenerator, rockProb, generations, minRockNeighborsToLive, boardBorderOffset);
            
            var entries = new TileSpace[height, width];
            var decorationList = new List<Thing>();

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
                    var decoration = GetDecorationForCoordinates(caBoard, randomGenerator, stalactiteProb, stalagmiteProb, row, col);
                    if(decoration != null)
                    {
                        decorationList.Add(decoration);
                    }
                }
                entries[row, width - 1] = SolidTile();
            }
            decorations = decorationList;

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = SolidTile();
            }

            // Return tilespaces
            var tileSpaces = new List<TileSpace>();
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    tileSpaces.Add(entries[row, col]);
                }
            }
            return tileSpaces;
        }

        private static CellType[,] RunCAGeneration(int width, int height, Random random, double probabilityOfRock, int generations, int minRockNeighborsToLive, int borderOffset)
        {
            var board = new CellType[height, width];                 

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (borderOffset > 0 && col < borderOffset || col > (width - 1 - borderOffset) || row < borderOffset || row > (height - borderOffset))
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

        private static Thing GetDecorationForCoordinates(CellType[,] board, Random randomGenerator, double stalactiteProb, double stalagmiteProb, int row, int col)
        {
            if(board[row, col] == CellType.Rock)
            {
                return null;
            }                        
            
            if (IsSpaceInCove(board, row, col))
            {
                return GenerateRandomTreasureAtCoordinates(randomGenerator, col, row);
            }
            if(randomGenerator.NextDouble() < stalactiteProb)
            {
                return GenerateThingAtCoordinateCenter("Stalactite1", col, row);
            }
            if (randomGenerator.NextDouble() < stalagmiteProb)
            {
                return GenerateThingAtCoordinateCenter("Stalagmite1", col, row);
            }

            return null;
        }

        private static bool IsSpaceInCove(CellType[,] board, int row, int col)
        {
            //north-facing deep cove is this | | this |   or this   |
            //                               |_|      |_|         |_|
            var isNorthFacingDeepCove = (board[row - 1, col - 1] == CellType.Rock || board[row - 1, col + 1] == CellType.Rock) &&
                board[row, col - 1] == CellType.Rock && 
                board[row + 1, col - 1] == CellType.Rock && 
                board[row + 1, col] == CellType.Rock && 
                board[row + 1, col + 1] == CellType.Rock && 
                board[row, col + 1] == CellType.Rock;

            var isEastFacingDeepCove = (board[row - 1, col + 1] == CellType.Rock || board[row + 1, col + 1] == CellType.Rock) &&
                board[row - 1, col - 1] == CellType.Rock &&
                board[row, col - 1] == CellType.Rock &&
                board[row + 1, col - 1] == CellType.Rock &&
                board[row + 1, col] == CellType.Rock &&
                board[row - 1, col] == CellType.Rock;                

            var isWestFacingDeepCove = (board[row - 1, col - 1] == CellType.Rock || board[row + 1, col - 1] == CellType.Rock) &&
                board[row - 1, col] == CellType.Rock &&                
                board[row + 1, col] == CellType.Rock &&
                board[row + 1, col + 1] == CellType.Rock &&
                board[row, col + 1] == CellType.Rock &&
                board[row - 1, col + 1] == CellType.Rock;

            var isSouthFacingDeepCove = (board[row + 1, col - 1] == CellType.Rock || board[row + 1, col + 1] == CellType.Rock) &&
                board[row - 1, col - 1] == CellType.Rock &&
                board[row, col - 1] == CellType.Rock &&                
                board[row - 1, col] == CellType.Rock &&                
                board[row, col + 1] == CellType.Rock &&
                board[row - 1, col + 1] == CellType.Rock;

            return isNorthFacingDeepCove ||
                isEastFacingDeepCove ||
                isWestFacingDeepCove ||
                isSouthFacingDeepCove;
        }

        private static Thing GenerateRandomTreasureAtCoordinates(Random randomGenerator, int col, int row)
        {
            var treasureList = Actor.GetAll().Where(a => a.Category == CategoryType.Treasure).ToArray();
            return GenerateThingAtCoordinateCenter(treasureList[randomGenerator.Next(treasureList.Length)].ClassName, col, row);
        }

        private static Thing GenerateThingAtCoordinateCenter(string type, int col, int row)
        {
            return new Thing
                    (
                        type: type,
                        x: col + 0.5,
                        y: row + 0.5,
                        z: 0,
                        angle: 0,
                        skill1: true,
                        skill2: true,
                        skill3: true,
                        skill4: true
                    );
        }
    }
}