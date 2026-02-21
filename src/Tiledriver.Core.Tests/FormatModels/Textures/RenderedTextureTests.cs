using Shouldly;
using SkiaSharp;
using Tiledriver.Core.FormatModels.Textures;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Textures;

public sealed class RenderedTextureTests
{
	[Fact]
	public void ShouldRenderTextureToStream()
	{
		using var stream = new MemoryStream();
		var texture = new RenderedTexture(BackgroundColor: SKColors.White, Text: "Test");
		texture.RenderTo(stream);
		stream.Length.ShouldBeGreaterThan(0);
	}
}
