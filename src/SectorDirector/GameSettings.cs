using SectorDirector.Input;
using SectorDirector.Renderers;

namespace SectorDirector;

public sealed class GameSettings
{
	private readonly ScreenMessage _message;

	public bool FollowMode
	{
		get;
		private set
		{
			if (field != value)
			{
				field = value;
				FollowModeChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	} = true;

	public bool RotateMode
	{
		get;
		private set
		{
			if (field != value)
			{
				field = value;
				RotateModeChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	} = false;

	public bool DrawAntiAliased
	{
		get;
		private set
		{
			if (field != value)
			{
				field = value;
				DrawAntiAliasedModeChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	} = true;

	public bool ShowRenderTime
	{
		get;
		private set
		{
			if (field != value)
			{
				field = value;
				ShowRenderTimeChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	} = false;

	public RendererType Renderer
	{
		get;
		private set
		{
			if (field != value)
			{
				field = value;
				RendererChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	} = RendererType.LineTest;

	public RenderScale RenderScale
	{
		get;
		private set
		{
			if (field != value)
			{
				field = value;
				RenderScaleChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	} = RenderScale.Normal;

	public event EventHandler? FollowModeChanged;
	public event EventHandler? RotateModeChanged;
	public event EventHandler? DrawAntiAliasedModeChanged;
	public event EventHandler? ShowRenderTimeChanged;
	public event EventHandler? RendererChanged;
	public event EventHandler? RenderScaleChanged;

	public GameSettings(ScreenMessage message)
	{
		_message = message;
	}

	public void Update(DiscreteInput input)
	{
		switch (input)
		{
			case DiscreteInput.ToggleFollowMode:
				FollowMode = !FollowMode;
				_message.ShowMessage($"Follow mode is {(FollowMode ? "ON" : "OFF")}");
				break;
			case DiscreteInput.ToggleRotateMode:
				RotateMode = !RotateMode;
				_message.ShowMessage($"Rotate mode is {(RotateMode ? "ON" : "OFF")}");
				break;
			case DiscreteInput.ToggleLineAntiAliasing:
				DrawAntiAliased = !DrawAntiAliased;
				_message.ShowMessage($"Draw antialiased lines is {(DrawAntiAliased ? "ON" : "OFF")}");
				break;
			case DiscreteInput.ToggleShowRenderTime:
				ShowRenderTime = !ShowRenderTime;
				break;
			case DiscreteInput.SwitchRenderer:
				Renderer = Renderer.Next();
				break;
			case DiscreteInput.ToggleOverheadMap:
				if (Renderer == RendererType.FirstPerson)
				{
					Renderer = RendererType.Overhead;
				}
				else if (Renderer == RendererType.Overhead)
				{
					Renderer = RendererType.FirstPerson;
				}
				break;
			case DiscreteInput.DecreaseRenderFidelity:
				RenderScale = RenderScale.DecreaseFidelity();
				break;
			case DiscreteInput.IncreaseRenderFidelity:
				RenderScale = RenderScale.IncreaseFidelity();
				break;
			default:
				break;
		}
	}
}
