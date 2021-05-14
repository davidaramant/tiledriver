// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Tiledriver.Core.FormatModels.Textures
{
    /// <summary>
    /// A texture rendered on the fly.
    /// </summary>
    /// <remarks>
    /// This has nothing to do with the TEXTURES lump, but it is frequently used together.
    /// </remarks>
    public sealed record RenderedTexture(
        Color BackgroundColor,
        Color TextColor = new(),
        string Text = "",
        PatchRotation Rotation = PatchRotation.None)
    {
        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public void RenderTo(Stream output)
        {
            using Bitmap canvas = new(256, 256);

            RectangleF area = new RectangleF(0, 0, canvas.Width, canvas.Height);

            using Graphics g = Graphics.FromImage(canvas);

            using var backgroundBrush = new SolidBrush(BackgroundColor);
            g.FillRectangle(backgroundBrush, area);
            if (HasText)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                StringFormat format = new()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                using var textBrush = new SolidBrush(TextColor);
                g.DrawString(Text, new Font("Impact", 32), textBrush, area, format);
            }

            g.Flush();
            canvas.Save(output, ImageFormat.Png);
        }
    }
}