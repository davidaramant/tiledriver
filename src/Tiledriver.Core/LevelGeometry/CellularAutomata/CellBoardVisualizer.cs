// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SkiaSharp;
using System.Threading.Tasks;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.LevelGeometry.CellularAutomata
{
    public static class CellBoardVisualizer
    {
        public static IFastImage Render(CellBoard board, int scale = 10)
        {
            var image = new FastImage(board.Width, board.Height, scale);

            Parallel.For(0, board.Height, y =>
            {
                for (int x = 0; x < board.Width; x++)
                {
                    image.SetPixel(x, y, board[new Position(x, y)] == CellType.Alive ? SKColors.Black : SKColors.White);
                }
            });

            return image;
        }
    }
}
