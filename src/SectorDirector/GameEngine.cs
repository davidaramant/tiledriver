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
		Window.ClientSizeChanged += UpdateScreenBufferWithNewSize;

		_settings = new GameSettings(_screenMessage);
		_settings.RendererChanged += (s, e) => RecreateRenderer();
		_settings.RenderScaleChanged += UpdateScreenBufferWithNewSize;

		_keyToggles.FullScreen += KeyToggled_FullScreen;
	}

	private Point CurrentScreenSize =>
		new Point(x: _graphics.PreferredBackBufferWidth, y: _graphics.PreferredBackBufferHeight);

	private void UpdateScreenBufferWithNewSize(object? sender, EventArgs e) =>
		UpdateScreenBuffer(CurrentScreenSize.DivideBy(_settings.RenderScale));

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
		_graphics.IsFullScreen = !_graphics.IsFullScreen;
		if (_graphics.IsFullScreen)
		{
			_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		}
		else
		{
			_graphics.PreferredBackBufferWidth = 800;
			_graphics.PreferredBackBufferHeight = 600;
		}
		_graphics.ApplyChanges();
	}

	protected override void Initialize()
	{
		TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0);
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		_outputTexture = new Texture2D(
			_graphics.GraphicsDevice,
			width: CurrentScreenSize.X,
			height: CurrentScreenSize.Y,
			mipmap: false,
			format: SurfaceFormat.Bgra32
		);
		_screenBuffer = new PixelBuffer(
			width: _graphics.PreferredBackBufferWidth,
			height: _graphics.PreferredBackBufferHeight
		);

		_messageFont = Content.Load<SpriteFont>("Fonts/ScreenMessage");
		//var testMapsPath = Path.Combine(AppContext.BaseDirectory, "testmaps.wad");
		//_maps = WadLoader.Load(testMapsPath).Select(pair => pair.Map).ToList();

		//SwitchToMap(0);
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
			// It randomly crashes if exiting in fullscreen for whatever reason
			_graphics.IsFullScreen = false;
			_graphics.ApplyChanges();

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

		if (_settings.FollowMode)
		{
			//	_playerInfo.Update(_continuousInputs, gameTime);
		}
		//_renderer.Update(_continuousInputs, gameTime);
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
		_spriteBatch.Begin(
			sortMode: SpriteSortMode.Immediate,
			blendState: BlendState.Opaque,
			samplerState: SamplerState.PointWrap,
			depthStencilState: DepthStencilState.None,
			rasterizerState: RasterizerState.CullNone
		);

		if (_settings.ShowRenderTime)
			_frameTimeAggregator.StartTiming();

		//_renderer.Render(_screenBuffer, _playerInfo);

		if (_settings.ShowRenderTime)
			_frameTimeAggregator.StopTiming();

		_outputTexture.SetData(_screenBuffer.Pixels);

		_spriteBatch.Draw(
			texture: _outputTexture,
			destinationRectangle: new Rectangle(x: 0, y: 0, width: CurrentScreenSize.X, height: CurrentScreenSize.Y),
			color: Color.White
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
			DrawShadowedString(_messageFont, text, new Vector2(0, CurrentScreenSize.Y - size.Y), Color.Red);
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
