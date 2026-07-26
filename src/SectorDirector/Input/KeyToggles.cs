using Microsoft.Xna.Framework.Input;

namespace SectorDirector.Input;

public sealed class KeyToggles
{
	readonly KeyboardLatch _toggleFullscreenLatch = new(kb =>
		(kb.IsKeyDown(Keys.LeftAlt) || kb.IsKeyDown(Keys.RightAlt)) && kb.IsKeyDown(Keys.Enter)
	);

	readonly List<(KeyboardLatch latch, DiscreteInput input)> _simpleToggles = new();

	public KeyToggles()
	{
		AddSimpleToggles(
			(Keys.F, DiscreteInput.ToggleFollowMode),
			(Keys.R, DiscreteInput.ToggleRotateMode),
			(Keys.A, DiscreteInput.ToggleShowRenderTime),
			(Keys.D, DiscreteInput.ToggleLineAntiAliasing),
			(Keys.T, DiscreteInput.SwitchRenderer),
			(Keys.Tab, DiscreteInput.ToggleOverheadMap),
			(Keys.OemOpenBrackets, DiscreteInput.DecreaseRenderFidelity),
			(Keys.OemCloseBrackets, DiscreteInput.IncreaseRenderFidelity)
		);
	}

	private void AddSimpleToggles(params (Keys key, DiscreteInput input)[] simpleToggles)
	{
		foreach (var simple in simpleToggles)
		{
			_simpleToggles.Add((new KeyboardLatch(simple.key), simple.input));
		}
	}

	public event EventHandler? FullScreen;

	public DiscreteInput Update(KeyboardState keyboardState)
	{
		foreach (var simpleToggle in _simpleToggles)
		{
			if (simpleToggle.latch.IsTriggered(keyboardState))
			{
				return simpleToggle.input;
			}
		}

		if (_toggleFullscreenLatch.IsTriggered(keyboardState))
		{
			FullScreen?.Invoke(this, EventArgs.Empty);
		}

		return DiscreteInput.None;
	}
}
