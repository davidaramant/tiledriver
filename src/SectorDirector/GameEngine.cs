using Microsoft.Xna.Framework;

namespace SectorDirector;

public sealed class GameEngine : Game
{
	readonly GraphicsDeviceManager _graphics;
	readonly ScreenMessage _screenMessage = new();

	public GameEngine()
	{
		_graphics = new GraphicsDeviceManager(this)
		{
			PreferredBackBufferWidth = 800,
			PreferredBackBufferHeight = 600,
			IsFullScreen = false,
			SynchronizeWithVerticalRetrace = true,
		};
		Content.RootDirectory = "Content";
		Window.AllowUserResizing = true;
		//Window.ClientSizeChanged += UpdateScreenBufferWithNewSize;
	}
}
