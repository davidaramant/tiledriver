namespace Tiledriver.Core.LevelGeometry;

public sealed record Rectangle(Position TopLeft, Size Size)
{
	public Position BottomRight => new(TopLeft.X + Size.Width - 1, TopLeft.Y + Size.Height - 1);
}
