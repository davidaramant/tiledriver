using Microsoft.Xna.Framework.Input;

namespace SectorDirector.Input;

public sealed class KeyboardLatch
{
	private readonly Func<KeyboardState, bool> _keyMatcher;
	private bool _pressing = false;

	public KeyboardLatch(Func<KeyboardState, bool> keyMatcher)
	{
		_keyMatcher = keyMatcher;
	}

	public KeyboardLatch(Keys key)
		: this(kb => kb.IsKeyDown(key)) { }

	public event EventHandler? Triggered;

	public bool IsTriggered(KeyboardState state)
	{
		if (_keyMatcher(state))
		{
			if (!_pressing)
			{
				_pressing = true;
				Triggered?.Invoke(this, EventArgs.Empty);
				return true;
			}
		}
		else
		{
			_pressing = false;
		}
		return false;
	}
}
