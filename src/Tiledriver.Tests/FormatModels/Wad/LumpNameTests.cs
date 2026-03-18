using Tiledriver.FormatModels.Wad;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Wad;

public sealed class LumpNameTests
{
	[Theory]
	[InlineData("EXCESSIVE_LENGTH")]
	[InlineData("naive\u00ef")]
	[InlineData("A\0B")]
	public void ShouldRejectInvalidNames(string name)
	{
		Assert.Throws<ArgumentException>(() => new LumpName(name));
	}

	[Theory]
	[InlineData("")]
	[InlineData("lower")]
	[InlineData("SPACE ")]
	[InlineData("MiXeD")]
	[InlineData("!@#$%^&*")]
	public void ShouldAcceptAsciiNamesAllowedBySpec(string name)
	{
		var actual = new LumpName(name);

		Assert.Equal(name, actual.ToString());
	}
}
