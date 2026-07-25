using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.Common;

public sealed class ParsingException : Exception
{
	public ParsingException(string message)
		: base(message) { }

	public static ParsingException UnexpectedEndOfFile() => new("Unexpected end of file");

	public static ParsingException CreateError(Token? token, string expected)
	{
		if (token == null)
		{
			return UnexpectedEndOfFile();
		}
		return new ParsingException(
			$"Unexpected token {token.GetType().Name} (expected {expected}) on {token.Location}"
		);
	}

	public static ParsingException CreateError<TExpected>(Token? token) => CreateError(token, typeof(TExpected).Name);
}
