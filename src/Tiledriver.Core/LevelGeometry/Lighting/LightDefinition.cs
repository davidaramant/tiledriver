namespace Tiledriver.Core.LevelGeometry.Lighting;

public enum LightHeight
{
	Middle,
	Floor,
	Ceiling,
}

public sealed record LightDefinition(
	Position Center,
	int Brightness,
	int Radius,
	LightHeight Height = LightHeight.Middle
)
{
	/// <summary>
	/// The 1D measure of how many tiles are affected by this light.
	/// </summary>
	/// <remarks>
	/// This is different from the diameter, because the light is assumed to be in the center of a tile. At minimum,
	/// it affects that one tile.
	/// </remarks>
	public int LengthAffected => 2 * Radius + 1;

	public (int Floor, int Ceiling) GetBrightness(Position location)
	{
		int ReduceBrightness(int brightness) => (int)Math.Max(0, brightness - 0.2 * Brightness);

		(int Floor, int Ceiling) TakeIntoAccountHeight(int brightness) =>
			Height switch
			{
				LightHeight.Middle => (brightness, brightness),
				LightHeight.Floor => (brightness, ReduceBrightness(brightness)),
				LightHeight.Ceiling => (ReduceBrightness(brightness), brightness),
				_ => throw new InvalidProgramException("Unknown LightHeight"),
			};

		var d2 = Center.DistanceSquared(location);
		var r2 = Radius * Radius;

		if (d2 > r2)
			return (0, 0);

		var d = Math.Sqrt(d2);

		var fraction = 1 - d / Radius;

		return TakeIntoAccountHeight((int)(Brightness * fraction));
	}
}
