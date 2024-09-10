// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Text;
using Tiledriver.Core.DemoMaps.Doom;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Udmf.Reading;
using Tiledriver.Core.FormatModels.Udmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Udmf.Reading;

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
