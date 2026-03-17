using Tiledriver.LevelGeometry.CaveGeneration.Doom.SquareModel;

namespace Tiledriver.LevelGeometry.CaveGeneration.Doom;

public sealed record EdgeSegment(
	EdgeSegmentId Id,
	SectorDescription Front,
	SectorDescription Back,
	SquarePoint Left,
	SquarePoint Right
);
