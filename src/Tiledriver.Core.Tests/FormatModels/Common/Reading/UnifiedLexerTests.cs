using System.Text;
using Shouldly;
using Tiledriver.Core.DemoMaps.Wolf3D;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Uwmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Common.Reading;

public sealed class UnifiedLexerTests
{
	[Theory]
	[InlineData("0", 0)]
	[InlineData("1", 1)]
	[InlineData("0000045", 45)]
	[InlineData("123", 123)]
	[InlineData("-123", -123)]
	[InlineData("+123", 123)]
	[InlineData("0x1234", 0x1234)]
	public void ShouldLexInteger(string input, int expected)
	{
		var tokens = Scan(input);
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<IntegerToken>().Value.ShouldBe(expected);
	}

	[Theory]
	[InlineData("0.", 0d)]
	[InlineData("1.", 1d)]
	[InlineData("1.23", 1.23)]
	[InlineData("-1.23", -1.23)]
	[InlineData("+1.23", 1.23)]
	public void ShouldLexFloat(string input, double expected)
	{
		var tokens = Scan(input);
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<FloatToken>().Value.ShouldBe(expected);
	}

	[Theory]
	[InlineData("true", true)]
	[InlineData("false", false)]
	public void ShouldLexBoolean(string input, bool expected)
	{
		var tokens = Scan(input);
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<BooleanToken>().Value.ShouldBe(expected);
	}

	[Theory]
	[InlineData("\"\"", "")]
	[InlineData("\"Some value 123 _\"", "Some value 123 _")]
	public void ShouldLexString(string input, string expected)
	{
		var tokens = Scan(input);
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<StringToken>().Value.ShouldBe(expected);
	}

	[Theory]
	[InlineData("a")]
	[InlineData("A")]
	[InlineData("_")]
	[InlineData("someName_123")]
	[InlineData("t")]
	[InlineData("true_1")]
	public void ShouldLexIdentifier(string id)
	{
		var tokens = Scan(id);
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<IdentifierToken>().Id.ShouldBe(new Identifier(id));
	}

	[Theory]
	[InlineData("id=1;")]
	[InlineData(" id = 1 ; ")]
	[InlineData("// Some comment\n id = 1 ; ")]
	[InlineData("/*\nSome comment\n*/id = 1 ; ")]
	[InlineData("id=1;//ending comment")]
	[InlineData("id=1;/*ending block comment*/")]
	public void ShouldLexAssignment(string input)
	{
		var tokens = Scan(input);
		tokens.Length.ShouldBe(4);
		tokens[0].ShouldBeOfType<IdentifierToken>();
		tokens[1].ShouldBeOfType<EqualsToken>();
		tokens[2].ShouldBeOfType<IntegerToken>();
		tokens[3].ShouldBeOfType<SemicolonToken>();
	}

	[Fact]
	public void ShouldLexBlock()
	{
		var tokens = Scan("blockName{}");
		tokens.Length.ShouldBe(3);
		tokens[0].ShouldBeOfType<IdentifierToken>();
		tokens[1].ShouldBeOfType<OpenBraceToken>();
		tokens[2].ShouldBeOfType<CloseBraceToken>();
	}

	[Fact]
	public void ShouldLexNewLines()
	{
		var tokens = Scan("blockName\n{\n}\n", createLexer: reader => new UnifiedLexer(reader, reportNewlines: true));
		tokens.Length.ShouldBe(6);
		tokens[0].ShouldBeOfType<IdentifierToken>();
		tokens[1].ShouldBeOfType<NewLineToken>();
		tokens[2].ShouldBeOfType<OpenBraceToken>();
		tokens[3].ShouldBeOfType<NewLineToken>();
		tokens[4].ShouldBeOfType<CloseBraceToken>();
	}

	[Fact]
	public void ShouldLexTuple()
	{
		var tokens = Scan("{1,2,-3}");
		tokens.Length.ShouldBe(7);
		tokens[0].ShouldBeOfType<OpenBraceToken>();
		tokens[1].ShouldBeOfType<IntegerToken>().Value.ShouldBe(1);
		tokens[2].ShouldBeOfType<CommaToken>();
		tokens[3].ShouldBeOfType<IntegerToken>().Value.ShouldBe(2);
		tokens[4].ShouldBeOfType<CommaToken>();
		tokens[5].ShouldBeOfType<IntegerToken>().Value.ShouldBe(-3);
		tokens[6].ShouldBeOfType<CloseBraceToken>();
	}

	[Fact]
	public void ShouldThrowIfNotExpectingDollarSign()
	{
		Assert.Throws<ParsingException>(() => Scan("$id"));
	}

	[Fact]
	public void ShouldNotThrowIfExpectingDollarSign()
	{
		var tokens = Scan("$id", r => new UnifiedLexer(r, allowDollarIdentifiers: true));
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<IdentifierToken>().Id.ToLower().ShouldBe("$id");
	}

	[Fact]
	public void ShouldThrowIfNotExpectingPipe()
	{
		Assert.Throws<ParsingException>(() => Scan("|"));
	}

	[Fact]
	public void ShouldNotThrowIfExpectingPipe()
	{
		var tokens = Scan("|", r => new UnifiedLexer(r, allowPipes: true));
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<PipeToken>();
	}

	private static Token[] Scan(string input, Func<StringReader, UnifiedLexer>? createLexer = null)
	{
		createLexer ??= reader => new UnifiedLexer(reader);

		using var stringReader = new StringReader(input);
		var lexer = createLexer(stringReader);
		return lexer.Scan().ToArray();
	}

	[Fact]
	public void ShouldHandleLexingThingDemoMap()
	{
		var map = ThingDemoMap.Create();

		using var stream = new MemoryStream();
		map.WriteTo(stream);

		stream.Position = 0;

		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = new UnifiedLexer(textReader);
		var result = lexer.Scan().ToArray();
	}

	[Fact]
	public void ShouldLexUwmfTestFile()
	{
		using var stream = TestFile.Uwmf.TEXTMAP;
		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = new UnifiedLexer(textReader);
		var result = lexer.Scan().ToArray();
	}
}
