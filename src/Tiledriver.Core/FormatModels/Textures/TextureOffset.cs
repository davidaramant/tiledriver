namespace Tiledriver.Core.FormatModels.Textures;

public readonly struct TextureOffset
{
	public readonly int Horizontal;
	public readonly int Vertical;

	public TextureOffset(int horizontal, int vertical)
	{
		Horizontal = horizontal;
		Vertical = vertical;
	}
}
