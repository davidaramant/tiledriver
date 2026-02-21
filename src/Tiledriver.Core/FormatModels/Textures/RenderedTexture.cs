using SkiaSharp;

namespace Tiledriver.Core.FormatModels.Textures;

/// <summary>
/// A texture rendered on the fly.
/// </summary>
/// <remarks>
/// This has nothing to do with the TEXTURES lump, but it is frequently used together.
/// </remarks>
public sealed record RenderedTexture(
	SKColor BackgroundColor,
	SKColor TextColor,
	string Text = "",
	string Name = "",
	PatchRotation Rotation = PatchRotation.None
)
{
	public RenderedTexture(
		SKColor BackgroundColor,
		string Text = "",
		string Name = "",
		PatchRotation Rotation = PatchRotation.None
	)
		: this(BackgroundColor, SKColors.Black, Text, Name, Rotation) { }

	public bool HasText => !string.IsNullOrWhiteSpace(Text);

	public void RenderTo(Stream output)
	{
		const int size = 256;

		using var bitmap = new SKBitmap(size, size);
		using (var canvas = new SKCanvas(bitmap))
		{
			var entireArea = new SKRect(0, 0, size, size);

			using var backgroundPaint = new SKPaint { Color = BackgroundColor };
			canvas.DrawRect(entireArea, backgroundPaint);

			if (HasText)
			{
				var lines = Text.Split('\n');

				using var typeFace = SKTypeface.FromFamilyName("Impact");
				using var font = new SKFont(typeFace, size: 32);
				using var textPaint = new SKPaint { Color = TextColor, IsAntialias = true };

				var lineHeight = size / lines.Length;
				var halfLineHeight = lineHeight / 2;
				for (int i = 0; i < lines.Length; i++)
				{
					var textWidth = font.MeasureText(lines[i]);
					canvas.DrawText(
						lines[i],
						(size - textWidth) / 2f,
						halfLineHeight + (i * lineHeight),
						font,
						textPaint
					);
				}
			}
		}

		bitmap.Encode(output, SKEncodedImageFormat.Png, 100);
	}
}
