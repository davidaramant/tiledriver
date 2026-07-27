using System.Numerics;
using SectorDirector.Input;
using SkiaSharp;
using Tiledriver.Rendering;

namespace SectorDirector.Renderers;

public sealed class LineTestRenderer : IRenderer
{
	private readonly GameSettings _settings;
	private readonly ScreenMessage _message;
	private const float MsToGammaSpeed = 0.001f;
	private const float MsToRadiansDeltaSpeed = 0.000001f;
	private float _msToRadians = 0.1f / 1000f;
	private float _angle = 0;

	public LineTestRenderer(GameSettings settings, ScreenMessage message)
	{
		_settings = settings;
		_message = message;
	}

	public void Update(ContinuousInputs inputs, GameClock gameTime)
	{
		var rotationDelta = gameTime.ElapsedGameTime.Milliseconds * MsToRadiansDeltaSpeed;

		if (inputs.HasFlag(ContinuousInputs.TurnLeft))
		{
			_msToRadians -= rotationDelta;
		}
		else if (inputs.HasFlag(ContinuousInputs.TurnRight))
		{
			_msToRadians += rotationDelta;
		}
		else if (inputs.HasFlag(ContinuousInputs.Forward))
		{
			_msToRadians = 0;
		}

		if (inputs.HasFlag(ContinuousInputs.ZoomIn))
		{
			var changeAmount = gameTime.ElapsedGameTime.Milliseconds * MsToGammaSpeed;
			PixelBufferExtensions.GammaExponent += changeAmount;
			_message.ShowMessage($"Current gamma: {PixelBufferExtensions.GammaExponent}");
		}
		else if (inputs.HasFlag(ContinuousInputs.ZoomOut))
		{
			var changeAmount = gameTime.ElapsedGameTime.Milliseconds * MsToGammaSpeed;
			PixelBufferExtensions.GammaExponent -= changeAmount;
			_message.ShowMessage($"Current gamma: {PixelBufferExtensions.GammaExponent}");
		}

		var rotationRadians = gameTime.ElapsedGameTime.Milliseconds * _msToRadians;
		_angle += rotationRadians;
	}

	public void Render(IPixelBuffer screen, PlayerInfo player)
	{
		screen.Fill(SKColors.Black);

		var center = new Vector2(screen.Dimensions.Width / 2, screen.Dimensions.Height / 2);
		var shortestSide = Math.Min(center.X, center.Y);

		var radius = 0.9f * shortestSide;

		const int numSegments = 5;
		var radianOffset = MathF.Tau / numSegments / 2;

		// This fixes jittering
		var pixelOffset = new Vector2(0.5f, 0.5f);

		foreach (var segment in Enumerable.Range(0, numSegments))
		{
			var rotation = Matrix3x2.CreateRotation(segment * radianOffset + _angle);
			var direction = Vector2.Transform(Vector2.UnitX, rotation);
			var startVector = center - direction * radius + pixelOffset;
			var endVector = center + direction * radius + pixelOffset;
			var start = new SKPointI((int)startVector.X, (int)startVector.Y);
			var end = new SKPointI((int)endVector.X, (int)endVector.Y);

			screen.DrawLine(
				start,
				end,
				SKColors.Red,
				mode: _settings.DrawAntiAliased ? LineMode.Smooth : LineMode.Exact
			);
		}

		screen.DrawCircle(screen.Dimensions.Width / 2, screen.Dimensions.Height / 2, (int)radius, SKColors.Red);
	}
}
