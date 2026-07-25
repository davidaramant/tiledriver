using Tiledriver.DemoMaps.Doom;
using Tiledriver.FormatModels.Udmf.Reading;
using Tiledriver.FormatModels.Udmf.Writing;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Udmf.Reading;

public sealed class UdmfSemanticAnalyzerTests
{
	[Fact]
	public void ShouldRoundTripDemoMap()
	{
		var map = DemoMap.Create();

		using var stream = new MemoryStream();
		map.WriteTo(stream);

		stream.Position = 0;

		var roundTripped = UdmfReader.Read(stream);

		UdmfComparison.AssertEqual(roundTripped, map);
	}
}
