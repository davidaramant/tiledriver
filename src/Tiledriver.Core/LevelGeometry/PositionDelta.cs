namespace Tiledriver.Core.LevelGeometry;

public sealed record PositionDelta(int X, int Y)
{
	public static PositionDelta operator +(PositionDelta d1, PositionDelta d2) => new(d1.X + d2.X, d1.Y + d2.Y);

	public static PositionDelta operator -(PositionDelta d1, PositionDelta d2) => new(d1.X - d2.X, d1.Y - d2.Y);

	public static PositionDelta operator +(PositionDelta d, int scalar) => new(d.X + scalar, d.Y + scalar);

	public static PositionDelta operator -(PositionDelta d, int scalar) => new(d.X - scalar, d.Y - scalar);

	public int MagnitudeSquared() => X * X + Y * Y;
}
