using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SectorDirector.Input;
using SectorDirector.Renderers;
using Tiledriver.Rendering;

namespace SectorDirector;

public sealed class GameEngine : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private readonly ScreenMessage _screenMessage = new();
	private readonly KeyToggles _keyToggles = new();
	private readonly GameSettings _settings;
	private readonly FrameTimeAggregator _frameTimeAggregator = new();
	private IRenderer _renderer = null!;
	private SpriteBatch _spriteBatch = null!;
	private Texture2D _outputTexture = null!;
	private PixelBuffer _screenBuffer = null!;
	private SpriteFont _messageFont = null!;
	private PlayerInfo _playerInfo = null!;
	private Point _windowedClientSize = new(800, 600);

	public GameEngine()
	{
		_graphics = new GraphicsDeviceManager(this)
		{
			PreferredBackBufferWidth = 800,
			PreferredBackBufferHeight = 600,
			IsFullScreen = false,
			HardwareModeSwitch = false,
			SynchronizeWithVerticalRetrace = true,
		};
		Content.RootDirectory = "Content";
		Window.AllowUserResizing = true;
		Window.ClientSizeChanged += UpdateScreenBufferWithNewSize;

		_settings = new GameSettings(_screenMessage);
		_settings.RendererChanged += (s, e) => RecreateRenderer();
		_settings.RenderScaleChanged += UpdateScreenBufferWithNewSize;

		_keyToggles.FullScreen += KeyToggled_FullScreen;
	}

	private void UpdateScreenBufferWithNewSize(object? sender, EventArgs e)
	{
		var clientSize = Window.ClientBounds.Size;
		if (clientSize.X == 0 || clientSize.Y == 0)
			return;

		if (_graphics.PreferredBackBufferWidth != clientSize.X || _graphics.PreferredBackBufferHeight != clientSize.Y)
		{
			_graphics.PreferredBackBufferWidth = clientSize.X;
			_graphics.PreferredBackBufferHeight = clientSize.Y;
			_graphics.ApplyChanges();
		}

		if (_screenBuffer is null)
			return;

		UpdateScreenBuffer(clientSize.DivideBy(_settings.RenderScale));
	}

	private void RecreateRenderer()
	{
		_renderer = CreateRenderer(_settings.Renderer);
	}

	private IRenderer CreateRenderer(RendererType type)
	{
		throw new NotImplementedException();
	}

	private void KeyToggled_FullScreen(object? sender, EventArgs e)
	{
		var enteringFullscreen = !_graphics.IsFullScreen;
		if (enteringFullscreen)
		{
			_windowedClientSize = Window.ClientBounds.Size;
			_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		}
		else
		{
			_graphics.PreferredBackBufferWidth = _windowedClientSize.X;
			_graphics.PreferredBackBufferHeight = _windowedClientSize.Y;
		}
		_graphics.IsFullScreen = enteringFullscreen;
		_graphics.ApplyChanges();
		UpdateScreenBuffer(
			new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight).DivideBy(
				_settings.RenderScale
			)
		);
	}

	protected override void Initialize()
	{
		TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0);
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		var renderSize = Window.ClientBounds.Size.DivideBy(_settings.RenderScale);
		_outputTexture = new Texture2D(
			_graphics.GraphicsDevice,
			width: renderSize.X,
			height: renderSize.Y,
			mipmap: false,
			format: SurfaceFormat.Bgra32
		);
		_screenBuffer = new PixelBuffer(renderSize.X, renderSize.Y);

		_messageFont = Content.Load<SpriteFont>("Fonts/ScreenMessage");
		//var testMapsPath = Path.Combine(AppContext.BaseDirectory, "testmaps.wad");
		//_maps = WadLoader.Load(testMapsPath).Select(pair => pair.Map).ToList();

		//SwitchToMap(0);
		_renderer = new LineTestRenderer(_settings, _screenMessage);
		_playerInfo = new PlayerInfo();
	}

	protected override void UnloadContent()
	{
		Content.Unload();
		_graphics.Dispose();
		_spriteBatch.Dispose();
		_outputTexture.Dispose();
	}

	protected override void Update(GameTime gameTime)
	{
		var keyboard = Keyboard.GetState();

		if (keyboard.IsKeyDown(Keys.Escape))
		{
			Exit();
		}

		var discreteInput = _keyToggles.Update(keyboard);
		var continuousInputs = ContinuousInputs.None;

		if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
			continuousInputs |= ContinuousInputs.Forward;

		if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
			continuousInputs |= ContinuousInputs.Backward;

		if (keyboard.IsKeyDown(Keys.Left))
			continuousInputs |= ContinuousInputs.TurnLeft;

		if (keyboard.IsKeyDown(Keys.Right))
			continuousInputs |= ContinuousInputs.TurnRight;

		if (keyboard.IsKeyDown(Keys.Q))
			continuousInputs |= ContinuousInputs.StrafeLeft;

		if (keyboard.IsKeyDown(Keys.E))
			continuousInputs |= ContinuousInputs.StrafeRight;

		if (keyboard.IsKeyDown(Keys.OemMinus))
			continuousInputs |= ContinuousInputs.ZoomOut;

		if (keyboard.IsKeyDown(Keys.OemPlus))
			continuousInputs |= ContinuousInputs.ZoomIn;

		if (keyboard.IsKeyDown(Keys.Z))
			continuousInputs |= ContinuousInputs.ResetZoom;

		var gameClock = new GameClock(
			TotalGameTime: gameTime.TotalGameTime,
			ElapsedGameTime: gameTime.ElapsedGameTime,
			IsRunningSlowly: gameTime.IsRunningSlowly
		);

		if (_settings.FollowMode)
		{
			_playerInfo.Update(continuousInputs, gameClock);
		}
		_renderer.Update(continuousInputs, gameClock);
		_settings.Update(discreteInput);

		base.Update(gameTime);
	}

	void UpdateScreenBuffer(Point renderSize)
	{
		if (_screenBuffer.Width != renderSize.X || _screenBuffer.Height != renderSize.Y)
		{
			_frameTimeAggregator.Reset();
			_screenMessage.ShowMessage($"Changing screen buffer to {renderSize.X}x{renderSize.Y}");
			_outputTexture.Dispose();
			_outputTexture = new Texture2D(
				_graphics.GraphicsDevice,
				width: renderSize.X,
				height: renderSize.Y,
				mipmap: false,
				format: SurfaceFormat.Bgra32
			);
			_screenBuffer = new PixelBuffer(renderSize.X, renderSize.Y);
		}
	}

	protected override void Draw(GameTime gameTime)
	{
		var viewport = GraphicsDevice.Viewport;
		_spriteBatch.Begin(
			sortMode: SpriteSortMode.Immediate,
			blendState: BlendState.Opaque,
			samplerState: SamplerState.PointClamp,
			depthStencilState: DepthStencilState.None,
			rasterizerState: RasterizerState.CullNone
		);

		if (_settings.ShowRenderTime)
			_frameTimeAggregator.StartTiming();

		_renderer.Render(_screenBuffer, _playerInfo);

		if (_settings.ShowRenderTime)
			_frameTimeAggregator.StopTiming();

		_outputTexture.SetData(_screenBuffer.Pixels);

		_spriteBatch.Draw(texture: _outputTexture, destinationRectangle: viewport.Bounds, color: Color.White);
		_spriteBatch.End();

		_spriteBatch.Begin(
			sortMode: SpriteSortMode.Immediate,
			blendState: BlendState.AlphaBlend,
			samplerState: SamplerState.LinearClamp,
			depthStencilState: DepthStencilState.None,
			rasterizerState: RasterizerState.CullNone
		);

		var message = _screenMessage.MaybeGetTextToShow(gameTime);
		if (message != string.Empty)
		{
			DrawShadowedString(_messageFont, message, new Vector2(0, 0), Color.White);
		}

		if (_settings.ShowRenderTime)
		{
			var text = $"Average render time: {_frameTimeAggregator.GetAverageFrameTimeInMs():#0.00}ms";
			var size = _messageFont.MeasureString(text);
			DrawShadowedString(_messageFont, text, new Vector2(0, viewport.Height - size.Y), Color.Red);
		}

		_spriteBatch.End();

		base.Draw(gameTime);
	}

	private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
	{
		_spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
		_spriteBatch.DrawString(font, value, position, color);
	}
}
