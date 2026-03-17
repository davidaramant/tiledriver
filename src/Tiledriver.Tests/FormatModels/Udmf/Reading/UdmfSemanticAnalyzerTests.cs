using System.Text;
using Tiledriver.DemoMaps.Doom;
using Tiledriver.FormatModels.Common.Reading;
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

		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var roundTripped = UdmfSemanticAnalyzer.ReadMapData(UdmfParser.Parse(new UnifiedLexer(textReader).Scan()));

		UdmfComparison.AssertEqual(roundTripped, map);
	}
}
