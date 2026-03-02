namespace Tiledriver.Core.ManualTests;

[TestFixture]
public sealed class DiamondSquareVisualization() : BaseVisualization("Diamond Square")
{
	[Test, Explicit]
	public void BlackAndWhite()
	{
		const string prefix = "bw";
		DeleteImages(prefix);
	}
}
