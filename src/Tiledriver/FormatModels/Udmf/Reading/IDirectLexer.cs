using Tiledriver.FormatModels.Common;

namespace Tiledriver.FormatModels.Udmf.Reading;

/// <summary>
/// A direct-read lexer interface for parsers that consume typed values without materializing token objects.
/// </summary>
public interface IDirectLexer
{
	Identifier ReadIdentifier();

	int ReadInteger();

	double ReadDouble();

	bool ReadBoolean();

	string ReadString();

	void ExpectEquals();

	void ExpectSemicolon();

	void ExpectComma();

	void ExpectOpenBrace();

	void ExpectCloseBrace();

	bool TryReadIdentifier(out Identifier name);

	bool TryExpectEquals();

	bool TryExpectOpenBrace();

	bool TryExpectCloseBrace();

	void SkipValueAndSemicolon();
}
