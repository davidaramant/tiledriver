using System.Globalization;

namespace Tiledriver.FormatModels.Common.Reading;

public sealed class TokenScanner : ITokenScanner
{
	private readonly TextReader _reader;
	private readonly TokenScannerOptions _options;
	private readonly Dictionary<string, Identifier> _identifierCache = new(StringComparer.OrdinalIgnoreCase);
	private readonly Dictionary<string, string> _stringCache = new(StringComparer.Ordinal);
	private FilePosition _currentPosition = FilePosition.StartOfFile;
	private char _currentChar;
	private const char Null = '\0';
	private char[] _tokenBuffer = new char[64];
	private int _tokenLength;

	public TokenScanner(TextReader reader, TokenScannerOptions? options = null)
	{
		_reader = reader;
		_options = options ?? new TokenScannerOptions();
		_currentChar = ReadChar();
	}

	public IEnumerable<Token> Scan()
	{
		while (TryReadNextToken(out Token? token))
		{
			yield return token;
		}
	}

	private bool TryReadNextToken(out Token token)
	{
		while (true)
		{
			char next = _currentChar;
			switch (next)
			{
				case '=':
					token = new EqualsToken(_currentPosition);
					SkipChar();
					return true;
				case ';':
					token = new SemicolonToken(_currentPosition);
					SkipChar();
					return true;
				case '{':
					token = new OpenBraceToken(_currentPosition);
					SkipChar();
					return true;
				case '}':
					token = new CloseBraceToken(_currentPosition);
					SkipChar();
					return true;
				case ',':
					token = new CommaToken(_currentPosition);
					SkipChar();
					return true;

				case var digit when IsAsciiDigit(next):
					token = LexNumber(digit);
					return true;
				case '-':
				case '+':
					token = LexNumber(next);
					return true;

				case '"':
					token = LexString();
					return true;

				case var c when IsAsciiLetter(c):
				case '_':
					token = LexIdentifier();
					return true;

				case '/':
					SkipComment();
					continue;

				case '\n':
					var start = _currentPosition;
					SkipChar();
					_currentPosition = _currentPosition.NextLine();
					if (_options.ReportNewlines)
					{
						token = new NewLineToken(start);
						return true;
					}

					continue;

				case var c when IsWhitespace(c):
					SkipChar();
					continue;

				case '$' when _options.AllowDollarIdentifiers:
					token = LexIdentifier();
					return true;

				case '|' when _options.AllowPipes:
					token = new PipeToken(_currentPosition);
					SkipChar();
					return true;

				case Null:
					token = null!;
					return false;

				default:
					throw new ParsingException($"Unexpected character {next} at {_currentPosition}");
			}
		}
	}

	private Token LexNumber(char first)
	{
		var start = _currentPosition;
		ClearTokenBuffer();
		ConsumeChar();

		if (first == '0' && _currentChar == 'x')
		{
			ClearTokenBuffer();
			SkipChar();

			if (!IsHexChar(_currentChar))
			{
				throw new ParsingException("Malformed hex number: " + _currentPosition);
			}

			while (IsHexChar(_currentChar))
			{
				ConsumeChar();
			}

			return new IntegerToken(start, BufferAsHexInteger(start));
		}

		while (IsAsciiDigit(_currentChar))
		{
			ConsumeChar();
		}

		if (_currentChar != '.')
		{
			return new IntegerToken(start, BufferAsInteger(start));
		}

		ConsumeChar();

		while (IsAsciiDigit(_currentChar))
		{
			ConsumeChar();
		}

		return new FloatToken(start, BufferAsFloat(start));
	}

	private StringToken LexString()
	{
		var start = _currentPosition;
		SkipChar();

		while (_currentChar != '"')
		{
			if (_currentChar == Null)
			{
				throw new ParsingException("Unterminated string starting at " + start);
			}

			ConsumeChar();
		}

		SkipChar();
		return new StringToken(start, BufferAsString());
	}

	private Token LexIdentifier()
	{
		var start = _currentPosition;
		ConsumeChar();

		while (IsIdentifierChar(_currentChar))
		{
			ConsumeChar();
		}

		if (TokenBufferEquals("true"))
		{
			ClearTokenBuffer();
			return new BooleanToken(start, true);
		}

		if (TokenBufferEquals("false"))
		{
			ClearTokenBuffer();
			return new BooleanToken(start, false);
		}

		return new IdentifierToken(start, BufferAsIdentifier());
	}

	private void SkipComment()
	{
		var start = _currentPosition;
		SkipChar();
		switch (_currentChar)
		{
			case '/':
				SkipChar();
				while (_currentChar != '\n' && _currentChar != Null)
				{
					SkipChar();
				}

				if (_currentChar == '\n')
				{
					SkipChar();
					_currentPosition = _currentPosition.NextLine();
				}

				break;
			case '*':
				SkipChar();
				bool inside = true;
				while (inside)
				{
					while (_currentChar != '*')
					{
						if (_currentChar == Null)
						{
							throw new ParsingException("Unterminated block comment starting at " + start);
						}
						SkipChar();
					}
					SkipChar();
					if (_currentChar == '/')
					{
						SkipChar();
						inside = false;
					}
					else if (_currentChar == Null)
					{
						throw new ParsingException("Unterminated block comment starting at " + start);
					}
				}
				break;
			default:
				throw new ParsingException("Malformed comment on " + start);
		}
	}

	private char ReadChar()
	{
		var next = _reader.Read();
		return next > -1 ? (char)next : Null;
	}

	private void SkipChar()
	{
		if (_currentChar == Null)
		{
			throw new ParsingException("Unexpected end of file at " + _currentPosition);
		}

		_currentPosition = _currentPosition.NextChar();
		_currentChar = ReadChar();
	}

	private void ConsumeChar()
	{
		if (_currentChar == Null)
		{
			throw new ParsingException("Unexpected end of file at " + _currentPosition);
		}

		AppendTokenChar(_currentChar);
		_currentPosition = _currentPosition.NextChar();
		_currentChar = ReadChar();
	}

	private int BufferAsHexInteger(FilePosition start)
	{
		if (!int.TryParse(TokenBufferSpan, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out int value))
		{
			throw new ParsingException("Malformed hex number: " + start);
		}

		ClearTokenBuffer();
		return value;
	}

	private int BufferAsInteger(FilePosition start)
	{
		if (!int.TryParse(TokenBufferSpan, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int value))
		{
			throw new ParsingException("Malformed integer number: " + start);
		}

		ClearTokenBuffer();
		return value;
	}

	private double BufferAsFloat(FilePosition start)
	{
		if (!double.TryParse(TokenBufferSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
		{
			throw new ParsingException("Malformed floating point number: " + start);
		}

		ClearTokenBuffer();
		return value;
	}

	private string BufferAsString()
	{
		var lookup = _stringCache.GetAlternateLookup<ReadOnlySpan<char>>();
		if (lookup.TryGetValue(TokenBufferSpan, out string? cached))
		{
			ClearTokenBuffer();
			return cached;
		}

		var value = new string(TokenBufferSpan);
		ClearTokenBuffer();
		_stringCache.Add(value, value);
		return value;
	}

	private Identifier BufferAsIdentifier()
	{
		var lookup = _identifierCache.GetAlternateLookup<ReadOnlySpan<char>>();
		if (lookup.TryGetValue(TokenBufferSpan, out Identifier identifier))
		{
			ClearTokenBuffer();
			return identifier;
		}

		var name = BufferAsString();
		identifier = new Identifier(name);
		_identifierCache.Add(name, identifier);
		return identifier;
	}

	private ReadOnlySpan<char> TokenBufferSpan => _tokenBuffer.AsSpan(0, _tokenLength);

	private void ClearTokenBuffer() => _tokenLength = 0;

	private void AppendTokenChar(char c)
	{
		if (_tokenLength == _tokenBuffer.Length)
		{
			Array.Resize(ref _tokenBuffer, _tokenBuffer.Length * 2);
		}

		_tokenBuffer[_tokenLength++] = c;
	}

	private bool TokenBufferEquals(string value) => TokenBufferSpan.SequenceEqual(value);

	private static bool IsAsciiDigit(char c) => c is >= '0' and <= '9';

	private static bool IsAsciiLetter(char c) => c is >= 'A' and <= 'Z' or >= 'a' and <= 'z';

	private static bool IsIdentifierChar(char c) => IsAsciiLetter(c) || IsAsciiDigit(c) || c == '_';

	private static bool IsHexChar(char c) => IsAsciiDigit(c) || c is >= 'a' and <= 'f' or >= 'A' and <= 'F';

	private static bool IsWhitespace(char c) => c is ' ' or '\t' or '\r' or '\f' or '\v';
}
