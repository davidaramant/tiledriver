namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;

/// <summary>
/// Segments of a square for use in Marching Squares
/// </summary>
/// <remarks>
/// +-----+
/// |0/|\2|
/// |/1|3\|
/// +-----+
/// |\7|5/|
/// |6\|/4|
/// +-----+
/// </remarks>
[Flags]
public enum SquareSegments : byte
{
	None,

	UpperLeftOuter = 1 << 0,
	UpperLeftInner = 1 << 1,

	UpperRightOuter = 1 << 2,
	UpperRightInner = 1 << 3,

	LowerRightOuter = 1 << 4,
	LowerRightInner = 1 << 5,

	LowerLeftOuter = 1 << 6,
	LowerLeftInner = 1 << 7,

	Corner_UpperLeft = UpperLeftOuter,
	Corner_UpperRight = UpperRightOuter,
	Corner_LowerRight = LowerRightOuter,
	Corner_LowerLeft = LowerLeftOuter,

	Corners_Left = UpperLeftOuter | UpperLeftInner | LowerLeftInner | LowerLeftOuter,
	Corners_Right = UpperRightOuter | UpperRightInner | LowerRightInner | LowerRightOuter,
	Corners_Upper = UpperLeftInner | UpperLeftOuter | UpperRightInner | UpperRightOuter,
	Corners_Lower = LowerLeftInner | LowerLeftOuter | LowerRightInner | LowerRightOuter,

	Corners_AllButUpperLeft = UpperLeftInner | UpperRightInner | UpperRightOuter | Corners_Lower,
	Corners_AllButUpperRight = UpperRightInner | UpperLeftInner | UpperLeftOuter | Corners_Lower,
	Corners_AllButLowerLeft = LowerLeftInner | LowerRightInner | LowerRightOuter | Corners_Upper,
	Corners_AllButLowerRight = LowerRightInner | LowerLeftInner | LowerLeftOuter | Corners_Upper,

	Corners_UpperLeftAndLowerRight = Corner_UpperLeft | Corner_LowerRight,
	Corners_UpperRightAndLowerLeft = Corner_UpperRight | Corner_LowerLeft,

	Corners_All = Corners_Upper | Corners_Lower,

	All = 255,
}
