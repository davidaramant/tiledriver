// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Linq;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Utils.CellularAutomata
{
    public sealed class CellBoard
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

        public CellBoard RunGenerations(int generations, int minAliveNeighborsToLive)
        {
            foreach (var g in Enumerable.Range(1, generations))
            {
                Generation++;

                for (int row = 0; row < Height; row++)
                {
                    for (int col = 0; col < Width; col++)
                    {
                        var aliveNeighbors = CountAliveNeighbors(PreviousBoard, row: row, col: col);

                        CurrentBoard[row, col] =
                            aliveNeighbors >= minAliveNeighborsToLive ? CellType.Alive : CellType.Dead;
                    }
                }
            }

            return this;
        }

        private int CountAliveNeighbors(CellType[,] board, int row, int col)
        {
            int count = 0;

            // Horizontal
            // left
            if (col > 0 && board[row, col - 1] == CellType.Alive)
            {
                count++;
            }
            // right
            if (col < Dimensions.Width - 1 && board[row, col + 1] == CellType.Alive)
            {
                count++;
            }

            // Vertical
            // top
            if (row > 0 && board[row - 1, col] == CellType.Alive)
            {
                count++;
            }
            // bottom
            if (row < Dimensions.Height - 1 && board[row + 1, col] == CellType.Alive)
            {
                count++;
            }

            // top left corner
            if (col > 0 && row > 0 && board[row - 1, col - 1] == CellType.Alive)
            {
                count++;
            }

            // top right corner
            if (col < Dimensions.Width - 1 && row > 0 && board[row - 1, col + 1] == CellType.Alive)
            {
                count++;
            }

            // bottom left corner
            if (col > 0 && row < Dimensions.Height - 1 && board[row + 1, col - 1] == CellType.Alive)
            {
                count++;
            }

            // bottom right corner
            if (col < Dimensions.Width - 1 && row < Dimensions.Height - 1 && board[row + 1, col + 1] == CellType.Alive)
            {
                count++;
            }

            return count;
        }

    }
}