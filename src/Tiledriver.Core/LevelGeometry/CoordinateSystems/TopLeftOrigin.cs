namespace Tiledriver.Core.LevelGeometry.CoordinateSystems;

public sealed class TopLeftOrigin : IPositionOffsets
{
	public PositionDelta Up { get; } = new(0, -1);
	public PositionDelta Down { get; } = new(0, 1);
	public PositionDelta Left { get; } = new(-1, 0);
	public PositionDelta Right { get; } = new(1, 0);
	public PositionDelta UpAndRight { get; } = new(1, -1);
	public PositionDelta UpAndLeft { get; } = new(-1, -1);
	public PositionDelta DownAndRight { get; } = new(1, 1);
	public PositionDelta DownAndLeft { get; } = new(-1, 1);
}
