// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using SkiaSharp;

namespace Tiledriver.Core.FormatModels.Textures
{
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
        PatchRotation Rotation = PatchRotation.None)
    {
        public RenderedTexture(
            SKColor BackgroundColor,
            string Text = "",
            PatchRotation Rotation = PatchRotation.None)
            : this(BackgroundColor, SKColors.Black, Text, Rotation)
        {
        }

        public RenderedTexture(
            System.Drawing.Color BackgroundColor,
            System.Drawing.Color TextColor = new(),
            string Text = "",
            PatchRotation Rotation = PatchRotation.None) : this(SKColor.Empty, SKColor.Empty)
        {
        }

        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public void RenderTo(Stream output)
        {
            const int size = 256;

            using var bitmap = new SKBitmap(size, size);
            using (var canvas = new SKCanvas(bitmap))
            {
                var entireArea = new SKRect(0, 0, size, size);

                using var backgroundPaint = new SKPaint {Color = BackgroundColor};
                canvas.DrawRect(entireArea, backgroundPaint);

                if (HasText)
                {
                    using var font = new SKFont();
                    using var textPaint = new SKPaint
                    {
                        Color = TextColor,
                        IsAntialias = true,
                        TextAlign = SKTextAlign.Center,
                        TextSize = size / 5f,
                    };
                    canvas.DrawText(Text, size / 2f, size / 2f, font, textPaint);
                }
            }

            bitmap.Encode(output, SKEncodedImageFormat.Png, 100);
        }
    }
}