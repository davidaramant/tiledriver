namespace Tiledriver.Core.FormatModels.MapInfo;

public sealed record OffsetGameBorder(
	int Offset,
	string TopLeft,
	string Top,
	string TopRight,
	string Left,
	string Right,
	string BottomLeft,
	string Bottom,
	string BottomRight
) : IGameBorder;
