// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tiledriver.Core.FormatModels.Common.Reading
{
	/// <summary>
	/// Lexer for both UWMF and XLAT formats.
	/// </summary>
	public sealed class UnifiedLexer
	{
		private readonly TextReader _reader;
		private readonly bool _reportNewlines;
		private readonly bool _allowDollarIdentifiers;
		private readonly bool _allowPipes;
		private FilePosition _currentPosition = FilePosition.StartOfFile;
		private const char Null = '\0';
		private readonly StringBuilder _tokenBuffer = new();

		public UnifiedLexer(
			TextReader reader,
			bool reportNewlines = false,
			bool allowDollarIdentifiers = false,
			bool allowPipes = false
		)
		{
			_reader = reader;
			_reportNewlines = reportNewlines;
			_allowDollarIdentifiers = allowDollarIdentifiers;
			_allowPipes = allowPipes;
		}

		public IEnumerable<Token> Scan()
		{
			if (_reportNewlines)
			{
				return ScanInternal();
			}

			return ScanInternal().Where(t => t is not NewLineToken);
		}

		private IEnumerable<Token> ScanInternal()
		{
			char next = PeekChar();
			while (next != Null)
			{
				switch (next)
				{
					case '=':
						yield return new EqualsToken(_currentPosition);
						SkipChar();
						break;
					case ';':
						yield return new SemicolonToken(_currentPosition);
						SkipChar();
						break;
					case '{':
						yield return new OpenBraceToken(_currentPosition);
						SkipChar();
						break;
					case '}':
						yield return new CloseBraceToken(_currentPosition);
						SkipChar();
						break;
					case ',':
						yield return new CommaToken(_currentPosition);
						SkipChar();
						break;

					case char digit when char.IsDigit(next):
						yield return LexNumber(digit);
						break;
					case '-':
					case '+':
						yield return LexNumber(next);
						break;

					case '"':
						yield return LexString();
						break;

					case char when char.IsLetter(next):
					case '_':
						yield return LexIdentifier();
						break;

					case '/':
						SkipComment();
						break;

					case '\n':
						yield return new NewLineToken(_currentPosition);
						SkipChar();
						_currentPosition = _currentPosition.NextLine();
						break;

					case char when char.IsWhiteSpace(next):
						SkipChar();
						break;

					case '$' when _allowDollarIdentifiers:
						yield return LexIdentifier();
						break;

					case '|' when _allowPipes:
						yield return new PipeToken(_currentPosition);
						SkipChar();
						break;

					case Null:
						yield break;

					default:
						throw new ParsingException($"Unexpected character {next} at {_currentPosition}");
				}

				next = PeekChar();
			}
		}

		private Token LexNumber(char first)
		{
			var start = _currentPosition;
			ConsumeChar();

			if (first == '0' && PeekChar() == 'x')
			{
				_tokenBuffer.Clear();
				SkipChar();

				static bool IsHexChar(char c) => char.IsDigit(c) || c is >= 'a' and <= 'f' or >= 'A' and <= 'F';

				if (!IsHexChar(PeekChar()))
				{
					throw new ParsingException("Malformed hex number: " + _currentPosition);
				}

				while (IsHexChar(PeekChar()))
				{
					ConsumeChar();
				}
				return new IntegerToken(start, BufferAsHexInteger());
			}

			while (char.IsDigit(PeekChar()))
			{
				ConsumeChar();
			}
			if (PeekChar() != '.')
			{
				return new IntegerToken(start, BufferAsInteger());
			}

			ConsumeChar();

			while (char.IsDigit(PeekChar()))
			{
				ConsumeChar();
			}

			return new FloatToken(start, BufferAsFloat());
		}

		private StringToken LexString()
		{
			var start = _currentPosition;
			SkipChar();

			while (PeekChar() != '"')
			{
				ConsumeChar();
			}
			SkipChar();

			return new StringToken(start, BufferAsString());
		}

		private Token LexIdentifier()
		{
			var start = _currentPosition;
			ConsumeChar();

			while (char.IsLetterOrDigit(PeekChar()) || PeekChar() == '_')
			{
				ConsumeChar();
			}

			var name = BufferAsString();
			return name switch
			{
				"true" => new BooleanToken(start, true),
				"false" => new BooleanToken(start, false),
				_ => new IdentifierToken(start, new Identifier(name)),
			};
		}

		private void SkipComment()
		{
			var start = _currentPosition;
			SkipChar();
			switch (PeekChar())
			{
				case '/':
					SkipChar();
					while (PeekChar() != '\n' && PeekChar() != Null)
					{
						SkipChar();
					}

					if (PeekChar() == '\n')
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
						while (PeekChar() != '*')
						{
							SkipChar();
						}
						SkipChar();
						if (PeekChar() == '/')
						{
							SkipChar();
							inside = false;
						}
					}
					break;
				default:
					throw new ParsingException("Malformed comment on " + start);
			}
		}

		private char PeekChar()
		{
			var next = _reader.Peek();
			return next > -1 ? (char)next : Null;
		}

		private void SkipChar()
		{
			_currentPosition = _currentPosition.NextChar();
			if (_reader.Read() == Null)
			{
				throw new ParsingException("Unexpected end of file at " + _currentPosition);
			}
		}

		private void ConsumeChar()
		{
			_currentPosition = _currentPosition.NextChar();
			var next = _reader.Read();
			if (next == -1)
			{
				throw new ParsingException("Unexpected end of file at " + _currentPosition);
			}
			_tokenBuffer.Append((char)next);
		}

		private int BufferAsHexInteger()
		{
			var value = int.Parse(_tokenBuffer.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
			_tokenBuffer.Clear();
			return value;
		}

		private int BufferAsInteger()
		{
			var value = int.Parse(_tokenBuffer.ToString());
			_tokenBuffer.Clear();
			return value;
		}

		private double BufferAsFloat()
		{
			var value = double.Parse(_tokenBuffer.ToString());
			_tokenBuffer.Clear();
			return value;
		}

		private string BufferAsString()
		{
			var value = _tokenBuffer.ToString();
			_tokenBuffer.Clear();
			return value;
		}
	}
}
