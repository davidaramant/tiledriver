// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Drawing;
using System.IO;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Textures;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Textures
{
    public sealed class RenderedTextureTests
    {
        [Fact(Skip = "Remove System.Drawing for this to work")]
        public void ShouldRenderTextureToStream()
        {
            using var ms = new MemoryStream();
            var texture = new RenderedTexture(BackgroundColor: Color.Black, Text: "Test");
            texture.RenderTo(ms);
            ms.Length.Should().BeGreaterThan(0);
        }
    }
}