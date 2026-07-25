using System.Drawing;

namespace Tiledriver.Extensions.Drawing;

public static class DrawingExtensions
{
	public static int Area(this Size s) => s.Height * s.Width;
}
