// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using SkiaSharp;
using Tiledriver.Core.FormatModels.Textures;

namespace Tiledriver.Core.ManualTests
{
    public class RenderedTextureTests
    {
        private DirectoryInfo _dirInfo;

        [SetUp]
        public void CreateDirectory()
        {
            _dirInfo = OutputLocation.CreateDirectory("Rendered Textures");
        }

        [Test, Explicit]
        public void RenderTexture()
        {
            var filePath = Path.Combine(_dirInfo.FullName, "Text.png");

            using var stream = File.Open(filePath, FileMode.Create);
            var texture = new RenderedTexture(BackgroundColor: SKColors.White,
                Text: "Line 1\nLine 2\nLINE THREE\nLine Four");
            texture.RenderTo(stream);
        }
    }
}