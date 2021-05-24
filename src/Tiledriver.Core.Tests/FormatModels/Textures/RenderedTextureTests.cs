// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using SkiaSharp;
using Tiledriver.Core.FormatModels.Textures;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Textures
{
    public sealed class RenderedTextureTests
    {
        [Fact]
        public void ShouldRenderTextureToStream()
        {
            using var stream = new MemoryStream();
            var texture = new RenderedTexture(BackgroundColor: SKColors.White, Text: "Test");
            texture.RenderTo(stream);
            stream.Length.Should().BeGreaterThan(0);
        }

        [Fact(Skip="WIP")]
        public void ShouldRenderTextureAndShowIt()
        {
            const string fileName = nameof(RenderedTextureTests) + "_" + nameof(ShouldRenderTextureAndShowIt) + ".png";
            try
            {
                using var stream = File.Open(fileName, FileMode.Create);
                var texture = new RenderedTexture(BackgroundColor: SKColors.White, Text: "Test");
                texture.RenderTo(stream);

                Process.Start("open",fileName);
                Debugger.Break();
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }
    }
}