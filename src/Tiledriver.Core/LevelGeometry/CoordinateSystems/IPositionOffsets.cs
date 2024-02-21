namespace Tiledriver.Core.LevelGeometry.CoordinateSystems;

public interface IPositionOffsets
{
	PositionDelta Up { get; }
	PositionDelta Down { get; }
	PositionDelta Left { get; }
	PositionDelta Right { get; }

	PositionDelta UpAndRight { get; }
	PositionDelta UpAndLeft { get; }
	PositionDelta DownAndRight { get; }
	PositionDelta DownAndLeft { get; }
}
