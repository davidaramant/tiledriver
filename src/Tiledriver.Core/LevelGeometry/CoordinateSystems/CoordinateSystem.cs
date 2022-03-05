namespace Tiledriver.Core.LevelGeometry.CoordinateSystems;
public static class CoordinateSystem
{
    /// <summary>
    /// Increasing Y goes UP
    /// </summary>
    public static IPositionOffsets BottomLeft { get; } = new BottomLeftOrigin();
    /// <summary>
    /// Increasing Y goes DOWN
    /// </summary>
    public static IPositionOffsets TopLeft { get; } = new TopLeftOrigin();
}
