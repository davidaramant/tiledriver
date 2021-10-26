// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Linq;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Utils.CellularAutomata
{
    public sealed class CellBoard : IBox
    {
        private readonly CellType[,] _odd;
        private readonly CellType[,] _even;

        public Size Dimensions { get; }
        public int Width => Dimensions.Width;
        public int Height => Dimensions.Height;
        public int Generation { get; private set; }
        public CellType this[Position p] => CurrentBoard[p.Y, p.X];

        private CellType[,] CurrentBoard => Generation % 2 == 0 ? _even : _odd;
        private CellType[,] PreviousBoard => Generation % 2 == 0 ? _odd : _even;

        public CellBoard(Size dimensions)
        {
            Dimensions = dimensions;

            _odd = new CellType[dimensions.Height, dimensions.Width];
            _even = new CellType[dimensions.Height, dimensions.Width];
        }

        public CellBoard(Size dimensions, Func<Position,CellType> typeAtPosition) : this(dimensions)
        {
            for (int y = 0; y < dimensions.Height; y++)
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    _even[y, x] = typeAtPosition(new Position(x, y));
                }
            }
        }

        private CellBoard(Size dimensions, CellType[,] initial) : this(dimensions)
        {
            _even = initial;
        }

        public CellBoard Quadruple()
        {
            var newSize = Dimensions * 2;
            var initial = new CellType[newSize.Height, newSize.Width];

            var current = CurrentBoard;

            for (int y = 0; y < Dimensions.Height; y++)
            {
                for (int x = 0; x < Dimensions.Width; x++)
                {
                    var value = current[y, x];
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
            Generation = 0;

            for (int row = 0; row < Dimensions.Height; row++)
            {
                for (int col = 0; col < Dimensions.Width; col++)
                {
                    _even[row, col] = random.NextDouble() <= probabilityAlive ? CellType.Alive : CellType.Dead;
                }
            }

            return this;
        }

        public CellBoard MakeBorderAlive(int thickness)
        {
            // This can obviously be improved but who cares; doubtful this will be the long pole in the tent
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (thickness > 0 && col < thickness || col > (Width - 1 - thickness) || row < thickness || row > (Height - thickness))
                    {
                        CurrentBoard[row, col] = CellType.Alive;
                    }
                }
            }

            return this;
        }

        public CellBoard RunGenerations(int generations, CellRule.IsNextGenAlive? isNextGenAlive = null)
        {
            isNextGenAlive ??= CellRule.FiveOrMoreNeighbors;

            foreach (var g in Enumerable.Range(1, generations))
            {
                Generation++;

                for (int row = 0; row < Height; row++)
                {
                    for (int col = 0; col < Width; col++)
                    {
                        CurrentBoard[row, col] =
                            isNextGenAlive(
                                PreviousBoard[row, col], 
                                radius => CountAliveNeighbors(PreviousBoard, row, col, radius)) 
                            ? CellType.Alive 
                            : CellType.Dead;
                    }
                }
            }

            return this;
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