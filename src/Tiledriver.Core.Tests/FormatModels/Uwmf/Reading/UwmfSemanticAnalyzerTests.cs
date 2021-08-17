// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using System.Text;
using Tiledriver.Core.DemoMaps.Wolf3D;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Uwmf.Reading;
using Tiledriver.Core.FormatModels.Uwmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Reading
{
    public sealed class UwmfSemanticAnalyzerTests
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
}