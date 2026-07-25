using Shouldly;
using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Udmf.Reading;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Udmf.Reading;

public sealed class DirectLexerTests
{
	[Fact]
	public void TryReadIdentifierReportsLineAfterMultilineBlockComment()
	{
		using var reader = new StringReader("/* first line\nsecond line\n*/!");
		var lexer = new DirectLexer(reader);

		var exception = Should.Throw<ParsingException>(() => lexer.TryReadIdentifier(out _));

		exception.Message.ShouldBe("Unexpected token (expected identifier or end of file) on Line: 3, Col: 3");
	}
}
