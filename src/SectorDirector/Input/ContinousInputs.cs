namespace SectorDirector.Input;

[Flags]
public enum ContinuousInputs
{
	None = 0,
	Forward = 1 << 0,
	Backward = 1 << 1,
	TurnLeft = 1 << 2,
	TurnRight = 1 << 3,
	StrafeLeft = 1 << 4,
	StrafeRight = 1 << 5,
	ZoomIn = 1 << 6,
	ZoomOut = 1 << 7,
	ResetZoom = 1 << 8,
}
