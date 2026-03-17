namespace Tiledriver.FormatModels.MapInfo;

public sealed record InsetGameBorder(string TopColor, string BottomColor, string HighlightColor) : IGameBorder;
