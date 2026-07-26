using Microsoft.Xna.Framework;

namespace SectorDirector;

public static class MonoGameExtensions
{
	public static Point DivideBy(this Point p, int denominator) => new Point(p.X / denominator, p.Y / denominator);

	public static Point DivideBy(this Point p, RenderScale renderScale) => p.DivideBy((int)renderScale);
}
