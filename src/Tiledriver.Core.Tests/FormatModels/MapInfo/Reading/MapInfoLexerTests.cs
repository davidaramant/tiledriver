using System.Text;
using Tiledriver.Core.FormatModels.MapInfo.Reading;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.MapInfo.Reading;

public sealed class MapInfoLexerTests
{
	[Fact]
	public void ShouldLexWolfCommonMapInfoTestFile()
	{
		using var stream = TestFile.MapInfo.wolfcommon;
		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = MapInfoLexer.Create(textReader);
		var result = lexer.Scan().ToArray();
	}

	[Fact]
	public void ShouldLexWolf3DMapInfoTestFile()
	{
		using var stream = TestFile.MapInfo.wolf3d;
		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = MapInfoLexer.Create(textReader);
		var result = lexer.Scan().ToArray();
	}

	[Fact]
	public void ShouldLexSpearMapInfoTestFile()
	{
		using var stream = TestFile.MapInfo.spear;
		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = MapInfoLexer.Create(textReader);
		var result = lexer.Scan().ToArray();
	}
}
