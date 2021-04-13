// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using FluentAssertions;
using System.IO;
using System.Text;
using Tiledriver.Core.FormatModels.Uwmf;
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
            var map = DemoMap.CreateThingDemoMap();

            using var stream = new MemoryStream();
            map.WriteTo(stream);

            stream.Position = 0;

            using var textReader = new StreamReader(stream, Encoding.ASCII);
            var roundTripped = UwmfSemanticAnalyzer.ReadMapData(UwmfParser.Parse(new UwmfLexer(textReader).Scan()));

            // TODO: FluentAssertions can't compare these sanely
        }

        [Fact]
        public void ShouldParseOldDemoMap()
        {
            using var stream =TestFile.Uwmf.TEXTMAP;
            using var textReader = new StreamReader(stream, Encoding.ASCII);
            UwmfSemanticAnalyzer.ReadMapData(UwmfParser.Parse(new UwmfLexer(textReader).Scan()));
        }
    }
}