// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Moq;
using System.IO;
using System.Text;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Xlat.Reading;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Reading
{
    public sealed class XlatParserTests
    {
        [Fact(Skip = "Doesn't work quite yet")]
        public void ShouldParseWolf3DXlat()
        {
            using var stream = TestFile.Xlat.wolf3d;
            using var textReader = new StreamReader(stream, Encoding.ASCII);
            var lexer = XlatLexer.Create(textReader);
            var translator = XlatParser.Parse(lexer.Scan(),Mock.Of<IResourceProvider>());
        }

        [Fact(Skip = "Doesn't work quite yet")]
        public void ShouldParseSpearXlat()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider
                .Setup(_ => _.Lookup("xlat/wolf3d.txt"))
                .Returns(TestFile.Xlat.wolf3d);

            using var stream = TestFile.Xlat.spear;
            using var textReader = new StreamReader(stream, Encoding.ASCII);
            var lexer = XlatLexer.Create(textReader);
            var translator = XlatParser.Parse(lexer.Scan(),mockProvider.Object);
        }
    }
}