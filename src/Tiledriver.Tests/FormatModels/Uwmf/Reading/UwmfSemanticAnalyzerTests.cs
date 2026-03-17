using System.Text;
using Tiledriver.DemoMaps.Wolf3D;
using Tiledriver.FormatModels.Common.Reading;
using Tiledriver.FormatModels.Uwmf.Reading;
using Tiledriver.FormatModels.Uwmf.Writing;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Uwmf.Reading;

public sealed class UdmfSemanticAnalyzerTests
{
	[Fact]
	public void ShouldRoundTripDemoMap()
	{
		var map = ThingDemoMap.Create();

		using var stream = new MemoryStream();
		map.WriteTo(stream);

		stream.Position = 0;

		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var roundTripped = UwmfSemanticAnalyzer.ReadMapData(UwmfParser.Parse(new UnifiedLexer(textReader).Scan()));

		UwmfComparison.AssertEqual(roundTripped, map);
	}

	[Fact]
	public void ShouldParseOldDemoMap()
	{
		using var stream = TestFile.Uwmf.TEXTMAP;
		using var textReader = new StreamReader(stream, Encoding.ASCII);
		UwmfSemanticAnalyzer.ReadMapData(UwmfParser.Parse(new UnifiedLexer(textReader).Scan()));
	}
}
