using Tiledriver.Core.FormatModels.Wad;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Wad;

public sealed class LumpNameTests
{
	[Theory]
	[InlineData("")]
	[InlineData("lower")]
	[InlineData("SPACE ")]
	[InlineData("EXCESSIVE_LENGTH")]
	public void ShouldRejectInvalidNames(string name)
	{
		Assert.Throws<ArgumentException>(() => new LumpName(name));
	}

	[Theory]
	[InlineData("NAME")]
	[InlineData("NAME1")]
	[InlineData("SPC[")]
	[InlineData("SPC]")]
	[InlineData("SPC-")]
	[InlineData("SPC_")]
	public void ShouldAcceptValidNames(string name)
	{
		var _ = new LumpName(name);
	}
}
