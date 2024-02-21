// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using System.Linq;
using System.Text;
using Tiledriver.Core.FormatModels.Common.Reading;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Reading
{
    public sealed class XlatLexerTests
    {
        [Fact]
        public void ShouldLexWolfXlatTestFile()
        {
            using var stream = TestFile.Xlat.wolf3d;
            using var textReader = new StreamReader(stream, Encoding.ASCII);
            var lexer = new UnifiedLexer(textReader, allowDollarIdentifiers: true, allowPipes: true);
            var result = lexer.Scan().ToArray();
        }

        [Fact]
        public void ShouldLexSpearXlatTestFile()
        {
            using var stream = TestFile.Xlat.spear;
            using var textReader = new StreamReader(stream, Encoding.ASCII);
            var lexer = new UnifiedLexer(textReader, allowDollarIdentifiers: true, allowPipes: true);
            var result = lexer.Scan().ToArray();
        }
    }
}
