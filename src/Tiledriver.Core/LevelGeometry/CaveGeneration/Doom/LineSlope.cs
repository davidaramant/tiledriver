namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public enum LineSlope
{
	Horizontal,
	Vertical,

	/// <summary>
	/// As X increases, Y increases
	/// </summary>
	IncreasingY,

	/// <summary>
	/// As X increases, Y decreases
	/// </summary>
	DecreasingY,
}
