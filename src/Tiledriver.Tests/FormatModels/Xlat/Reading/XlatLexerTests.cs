using System.Text;
using Tiledriver.FormatModels.Xlat.Reading;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Xlat.Reading;

public sealed class XlatLexerTests
{
	[Fact]
	public void ShouldLexWolfXlatTestFile()
	{
		using var stream = TestFile.Xlat.wolf3d;
		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = XlatLexer.Create(textReader);
		var result = lexer.Scan().ToArray();
	}

	[Fact]
	public void ShouldLexSpearXlatTestFile()
	{
		using var stream = TestFile.Xlat.spear;
		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = XlatLexer.Create(textReader);
		var result = lexer.Scan().ToArray();
	}
}
