using Shouldly;
using Tiledriver.FormatModels.Common.Reading;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Common.Reading;

public sealed class TokenScannerTests
{
	[Theory]
	[InlineData("0", 0)]
	[InlineData("-123", -123)]
	[InlineData("0x1234", 0x1234)]
	public void ShouldLexInteger(string input, int expected)
	{
		var tokens = Scan(input);
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<IntegerToken>().Value.ShouldBe(expected);
	}

	[Theory]
	[InlineData("0.", 0d)]
	[InlineData("1.23", 1.23)]
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

	[Fact]
	public void ShouldLexNewLinesWhenEnabled()
	{
		var tokens = Scan("blockName\n{\n}\n", new TokenScannerOptions(ReportNewlines: true));
		tokens.Length.ShouldBe(6);
		tokens[1].ShouldBeOfType<NewLineToken>();
		tokens[3].ShouldBeOfType<NewLineToken>();
	}

	[Fact]
	public void ShouldLexDollarIdentifiersWhenEnabled()
	{
		var tokens = Scan("$id", new TokenScannerOptions(AllowDollarIdentifiers: true));
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<IdentifierToken>().Id.ToLower().ShouldBe("$id");
	}

	[Fact]
	public void ShouldLexPipesWhenEnabled()
	{
		var tokens = Scan("|", new TokenScannerOptions(AllowPipes: true));
		tokens.Length.ShouldBe(1);
		tokens[0].ShouldBeOfType<PipeToken>();
	}

	[Fact]
	public void ShouldInternRepeatedIdentifiersIgnoringCase()
	{
		var tokens = Scan("Repeat repeat");
		var first = tokens[0].ShouldBeOfType<IdentifierToken>();
		var second = tokens[1].ShouldBeOfType<IdentifierToken>();

		first.Id.ToString().ShouldBe("Repeat");
		second.Id.ToString().ShouldBe("Repeat");
		ReferenceEquals((string)first.Id, (string)second.Id).ShouldBeTrue();
	}

	[Fact]
	public void ShouldInternRepeatedStringsWithSameCasing()
	{
		var tokens = Scan("\"repeat\" \"repeat\"");
		var first = tokens[0].ShouldBeOfType<StringToken>();
		var second = tokens[1].ShouldBeOfType<StringToken>();

		ReferenceEquals(first.Value, second.Value).ShouldBeTrue();
	}

	private static Token[] Scan(string input, TokenScannerOptions? options = null)
	{
		using var stringReader = new StringReader(input);
		return new TokenScanner(stringReader, options).Scan().ToArray();
	}
}
