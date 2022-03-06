// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Linq;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.Utils.CellularAutomata
{
    public sealed class CellBoard : IBox
    {
        private readonly CellType[,] _board;

        public Size Dimensions { get; }
        public int Width => Dimensions.Width;
        public int Height => Dimensions.Height;
        public CellType this[Position p] => _board[p.Y, p.X];
        
        public CellBoard(Size dimensions)
        {
            Dimensions = dimensions;

            _board = new CellType[dimensions.Height, dimensions.Width];
        }

        public CellBoard(Size dimensions, Func<Position,CellType> typeAtPosition) : this(dimensions)
        {
            for (int y = 0; y < dimensions.Height; y++)
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    _board[y, x] = typeAtPosition(new Position(x, y));
                }
            }
        }

        private CellBoard(Size dimensions, CellType[,] initial) : this(dimensions) => _board = initial;

        public CellBoard Quadruple()
        {
            var newSize = Dimensions * 2;
            var initial = new CellType[newSize.Height, newSize.Width];

            for (int y = 0; y < Dimensions.Height; y++)
            {
                for (int x = 0; x < Dimensions.Width; x++)
                {
                    var value = _board[y, x];
                    initial[y * 2, x * 2] = value;
                    initial[y * 2, x * 2 + 1] = value;
                    initial[y * 2 + 1, x * 2] = value;
                    initial[y * 2 + 1, x * 2 + 1] = value;
                }
            }

            return new CellBoard(newSize, initial);
        }

        public CellBoard Fill(Random random, double probabilityAlive)
        {
            var initial = new CellType[Dimensions.Height, Dimensions.Width];

            for (int row = 0; row < Dimensions.Height; row++)
            {
                for (int col = 0; col < Dimensions.Width; col++)
                {
                    initial[row, col] = random.NextDouble() <= probabilityAlive ? CellType.Alive : CellType.Dead;
                }
            }

            return new CellBoard(Dimensions, initial);
        }

        public CellBoard AddNoise(Random random, double percentage)
        {
            var allPositions = Dimensions.GetAllPositions().ToList();
            allPositions.Shuffle(random);

            var initial = (CellType[,])_board.Clone();

            foreach (var pos in allPositions.Take((int)(Dimensions.Area * percentage)))
            {
                var value = _board[pos.Y, pos.X];
                initial[pos.Y, pos.X] = value == CellType.Alive ? CellType.Dead : CellType.Alive;
            }

            return new CellBoard(Dimensions, initial);
        }

        public CellBoard MakeBorderAlive(int thickness)
        {
            var initial = (CellType[,])_board.Clone();

            // This can obviously be improved but who cares; doubtful this will be the long pole in the tent
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (thickness > 0 && col < thickness || col > (Width - 1 - thickness) || row < thickness || row > (Height - thickness))
                    {
                        initial[row, col] = CellType.Alive;
                    }
                }
            }

            return new CellBoard(Dimensions, initial);
        }

        public CellBoard RunGenerations(int generations, CellRule.IsNextGenAlive? isNextGenAlive = null)
        {
            isNextGenAlive ??= CellRule.FiveOrMoreNeighbors;

            var currentBoard = new CellType[Dimensions.Height, Dimensions.Width];
            var previousBoard = (CellType[,])_board.Clone();

            foreach (var g in Enumerable.Range(1, generations))
            {
                for (int row = 0; row < Height; row++)
                {
                    for (int col = 0; col < Width; col++)
                    {
                        currentBoard[row, col] =
                            isNextGenAlive(
                                previousBoard[row, col], 
                                radius => CountAliveNeighbors(previousBoard, row, col, radius)) 
                            ? CellType.Alive 
                            : CellType.Dead;
                    }
                }

                (currentBoard, previousBoard) = (previousBoard, currentBoard);
            }

            return new CellBoard(Dimensions, previousBoard);
        }

        private int CountAliveNeighbors(CellType[,] board, int row, int col, int radius)
        {
            int count = 0;

            for (int y = row - radius; y < row + radius + 1; y++)
            {
                for (int x = col - radius; x < col + radius + 1; x++)
                {
                    if (x < 0 || x >= Dimensions.Width ||
                        y < 0 || y >= Dimensions.Height ||
                        (x == col && y == col))
                    {
                        count++;
                        continue;
                    }

                    if (board[y, x] == CellType.Alive)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}