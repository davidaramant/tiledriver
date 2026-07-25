using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.Udmf.Reading;

/// <summary>
/// A direct-read lexer that returns typed values without materializing intermediate token objects.
/// </summary>
public sealed class DirectLexer : IDirectLexer
{
	private readonly TextReader _reader;
	private readonly Dictionary<string, Identifier> _identifierCache = new(StringComparer.OrdinalIgnoreCase);
	private readonly Dictionary<string, string> _stringCache = new(StringComparer.Ordinal);
	private FilePosition _currentPosition = FilePosition.StartOfFile;
	private char _currentChar;
	private char[] _tokenBuffer = new char[64];
	private int _tokenLength;
	private const char Null = '\0';

	public DirectLexer(TextReader reader)
	{
		_reader = reader;
		_currentChar = ReadChar();
	}

	public Identifier ReadIdentifier()
	{
		SkipWhitespaceAndComments();
		if (!IsAsciiLetter(_currentChar) && _currentChar != '_')
		{
			throw Unexpected("identifier");
		}

		return LexIdentifierValue();
	}

	public int ReadInteger()
	{
		SkipWhitespaceAndComments();
		if (!IsAsciiDigit(_currentChar) && _currentChar != '-' && _currentChar != '+')
		{
			throw Unexpected("integer");
		}

		var start = _currentPosition;
		ClearTokenBuffer();
		char first = _currentChar;
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

			return BufferAsHexInteger(start);
		}

		while (IsAsciiDigit(_currentChar))
		{
			ConsumeChar();
		}

		if (_currentChar == '.')
		{
			throw new ParsingException($"Expected integer but found float on {start}");
		}

		return BufferAsInteger(start);
	}

	public double ReadDouble()
	{
		SkipWhitespaceAndComments();
		if (!IsAsciiDigit(_currentChar) && _currentChar != '-' && _currentChar != '+')
		{
			throw Unexpected("number");
		}

		var start = _currentPosition;
		ClearTokenBuffer();
		char first = _currentChar;
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

			return BufferAsHexInteger(start);
		}

		while (IsAsciiDigit(_currentChar))
		{
			ConsumeChar();
		}

		if (_currentChar != '.')
		{
			return BufferAsInteger(start);
		}

		ConsumeChar();

		while (IsAsciiDigit(_currentChar))
		{
			ConsumeChar();
		}

		return BufferAsFloat(start);
	}

	public bool ReadBoolean()
	{
		SkipWhitespaceAndComments();
		if (!IsAsciiLetter(_currentChar))
		{
			throw Unexpected("boolean");
		}

		var start = _currentPosition;
		ClearTokenBuffer();
		ConsumeChar();

		while (IsIdentifierChar(_currentChar))
		{
			ConsumeChar();
		}

		if (TokenBufferEquals("true"))
		{
			ClearTokenBuffer();
			return true;
		}

		if (TokenBufferEquals("false"))
		{
			ClearTokenBuffer();
			return false;
		}

		throw new ParsingException($"Expected boolean but found identifier on {start}");
	}

	public string ReadString()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != '"')
		{
			throw Unexpected("string");
		}

		return LexStringValue();
	}

	public void ExpectEquals()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != '=')
		{
			throw Unexpected("=");
		}

		SkipChar();
	}

	public void ExpectSemicolon()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != ';')
		{
			throw Unexpected(";");
		}

		SkipChar();
	}

	public void ExpectComma()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != ',')
		{
			throw Unexpected(",");
		}

		SkipChar();
	}

	public void ExpectOpenBrace()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != '{')
		{
			throw Unexpected("{");
		}

		SkipChar();
	}

	public void ExpectCloseBrace()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != '}')
		{
			throw Unexpected("}");
		}

		SkipChar();
	}

	public bool TryReadIdentifier(out Identifier name)
	{
		SkipWhitespaceAndComments();
		if (_currentChar == Null)
		{
			name = default;
			return false;
		}

		if (!IsAsciiLetter(_currentChar) && _currentChar != '_')
		{
			throw Unexpected("identifier or end of file");
		}

		name = LexIdentifierValue();
		return true;
	}

	public bool TryExpectEquals()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != '=')
		{
			return false;
		}

		SkipChar();
		return true;
	}

	public bool TryExpectOpenBrace()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != '{')
		{
			return false;
		}

		SkipChar();
		return true;
	}

	public bool TryExpectCloseBrace()
	{
		SkipWhitespaceAndComments();
		if (_currentChar != '}')
		{
			return false;
		}

		SkipChar();
		return true;
	}

	public void SkipValueAndSemicolon()
	{
		SkipWhitespaceAndComments();

		switch (_currentChar)
		{
			case '"':
				_ = LexStringValue();
				break;
			case var c when IsAsciiLetter(c):
			case '_':
				_ = LexIdentifierOrBooleanForSkip();
				break;
			case var c when IsAsciiDigit(c):
			case '-':
			case '+':
				SkipNumericValue();
				break;
			default:
				throw Unexpected("value");
		}

		ExpectSemicolon();
	}

	private void SkipWhitespaceAndComments()
	{
		while (true)
		{
			switch (_currentChar)
			{
				case '/':
					SkipComment();
					continue;
				case var c when IsWhitespace(c) || c == '\n':
					SkipChar();
					if (c == '\n')
					{
						_currentPosition = _currentPosition.NextLine();
					}
					continue;
				default:
					return;
			}
		}
	}

	private Identifier LexIdentifierValue()
	{
		ClearTokenBuffer();
		ConsumeChar();

		while (IsIdentifierChar(_currentChar))
		{
			ConsumeChar();
		}

		return BufferAsIdentifier();
	}

	private string LexStringValue()
	{
		SkipChar();

		while (_currentChar != '"')
		{
			if (_currentChar == Null)
			{
				throw new ParsingException("Unterminated string starting at " + _currentPosition);
			}

			ConsumeChar();
		}

		SkipChar();
		return BufferAsString();
	}

	private void SkipNumericValue()
	{
		var start = _currentPosition;
		ClearTokenBuffer();
		char first = _currentChar;
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

			ClearTokenBuffer();
			return;
		}

		while (IsAsciiDigit(_currentChar))
		{
			ConsumeChar();
		}

		if (_currentChar == '.')
		{
			ConsumeChar();
			while (IsAsciiDigit(_currentChar))
			{
				ConsumeChar();
			}
		}

		ClearTokenBuffer();
	}

	private bool LexIdentifierOrBooleanForSkip()
	{
		var start = _currentPosition;
		ClearTokenBuffer();
		ConsumeChar();

		while (IsIdentifierChar(_currentChar))
		{
			ConsumeChar();
		}

		if (TokenBufferEquals("true") || TokenBufferEquals("false"))
		{
			ClearTokenBuffer();
			return true;
		}

		throw new ParsingException($"Expected value but found identifier on {start}");
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

						if (_currentChar == '\n')
						{
							SkipChar();
							_currentPosition = _currentPosition.NextLine();
						}
						else
						{
							SkipChar();
						}
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

	private ParsingException Unexpected(string expected) =>
		new($"Unexpected token (expected {expected}) on {_currentPosition}");

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
		int value = ParseHexInteger(TokenBufferSpan, start);

		ClearTokenBuffer();
		return value;
	}

	private int BufferAsInteger(FilePosition start)
	{
		int value = ParseInteger(TokenBufferSpan, start);

		ClearTokenBuffer();
		return value;
	}

	private double BufferAsFloat(FilePosition start)
	{
		double value = ParseFloat(TokenBufferSpan, start);

		ClearTokenBuffer();
		return value;
	}

	private static int ParseHexInteger(ReadOnlySpan<char> value, FilePosition start)
	{
		int result = 0;
		foreach (char c in value)
		{
			int digit = c switch
			{
				>= '0' and <= '9' => c - '0',
				>= 'a' and <= 'f' => c - 'a' + 10,
				>= 'A' and <= 'F' => c - 'A' + 10,
				_ => throw new ParsingException("Malformed hex number: " + start),
			};

			try
			{
				checked
				{
					result = (result * 16) + digit;
				}
			}
			catch (OverflowException)
			{
				throw new ParsingException("Malformed hex number: " + start);
			}
		}

		return result;
	}

	private static int ParseInteger(ReadOnlySpan<char> value, FilePosition start)
	{
		int index = 0;
		bool negative = false;

		if (value[index] is '-' or '+')
		{
			negative = value[index] == '-';
			index++;
		}

		if (index >= value.Length)
		{
			throw new ParsingException("Malformed integer number: " + start);
		}

		int result = 0;
		for (; index < value.Length; index++)
		{
			char c = value[index];
			if (!IsAsciiDigit(c))
			{
				throw new ParsingException("Malformed integer number: " + start);
			}

			try
			{
				checked
				{
					result = (result * 10) - (c - '0');
				}
			}
			catch (OverflowException)
			{
				throw new ParsingException("Malformed integer number: " + start);
			}
		}

		if (negative)
		{
			return result;
		}

		if (result == int.MinValue)
		{
			throw new ParsingException("Malformed integer number: " + start);
		}

		return -result;
	}

	private static double ParseFloat(ReadOnlySpan<char> value, FilePosition start)
	{
		int index = 0;
		bool negative = false;

		if (value[index] is '-' or '+')
		{
			negative = value[index] == '-';
			index++;
		}

		if (index >= value.Length)
		{
			throw new ParsingException("Malformed floating point number: " + start);
		}

		double result = 0;
		while (index < value.Length && IsAsciiDigit(value[index]))
		{
			result = (result * 10) + (value[index] - '0');
			index++;
		}

		if (index >= value.Length || value[index] != '.')
		{
			throw new ParsingException("Malformed floating point number: " + start);
		}

		index++;
		double scale = 0.1;
		while (index < value.Length)
		{
			char c = value[index];
			if (!IsAsciiDigit(c))
			{
				throw new ParsingException("Malformed floating point number: " + start);
			}

			result += (c - '0') * scale;
			scale *= 0.1;
			index++;
		}

		return negative ? -result : result;
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
