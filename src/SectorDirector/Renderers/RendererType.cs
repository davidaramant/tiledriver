using SectorDirector.Input;
using Tiledriver.Rendering;

namespace SectorDirector.Renderers;

// TODO: This stuff can probably move to Tiledriver. It would be nice to test a renderer in a manual test.

public enum RendererType
{
	LineTest,
	FirstPerson,
	Overhead,
	Fire,
	MapHistory,
}

public static class RendererTypeExtensions
{
	public static RendererType Next(this RendererType type) =>
		(RendererType)(((int)type + 1) % Enum.GetValues(typeof(RendererType)).Length);
}

public readonly record struct GameClock(TimeSpan TotalGameTime, TimeSpan ElapsedGameTime, bool IsRunningSlowly);

public interface IRenderer
{
	void Update(ContinuousInputs inputs, GameClock gameTime);

	void Render(IPixelBuffer screen, PlayerInfo player);
}
