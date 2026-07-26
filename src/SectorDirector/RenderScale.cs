namespace SectorDirector;

public enum RenderScale
{
	Normal = 1,
	Quarter = 2,
	Eighth = 4,
}

public static class RenderScaleExtensions
{
	public static RenderScale DecreaseFidelity(this RenderScale scale) =>
		scale == RenderScale.Normal ? RenderScale.Quarter : RenderScale.Eighth;

	public static RenderScale IncreaseFidelity(this RenderScale scale) =>
		scale == RenderScale.Eighth ? RenderScale.Quarter : RenderScale.Normal;
}
