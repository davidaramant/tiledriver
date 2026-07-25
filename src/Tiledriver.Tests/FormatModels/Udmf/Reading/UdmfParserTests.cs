using System.Text;
using Shouldly;
using Tiledriver.DemoMaps.Doom;
using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Common.Reading;
using Tiledriver.FormatModels.Udmf.Reading;
using Tiledriver.FormatModels.Udmf.Writing;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Udmf.Reading;

public sealed class UdmfParserTests
{
	[Fact]
	public void ShouldParseTopLevelAssignmentsFromFakeLexer()
	{
		var lexer = new FakeLexer(
			new IdentifierToken(FilePosition.StartOfFile, "namespace"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "zdoom"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "comment"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "test map"),
			new SemicolonToken(FilePosition.StartOfFile)
		);

		var result = new UdmfParser(lexer).Parse();

		result.NameSpace.ShouldBe("zdoom");
		result.Comment.ShouldBe("test map");
		result.Things.ShouldBeEmpty();
		result.Vertices.ShouldBeEmpty();
	}

	[Fact]
	public void ShouldParseVertexBlockFromFakeLexer()
	{
		var lexer = new FakeLexer(
			new IdentifierToken(FilePosition.StartOfFile, "namespace"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "zdoom"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "vertex"),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "x"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 10),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "y"),
			new EqualsToken(FilePosition.StartOfFile),
			new FloatToken(FilePosition.StartOfFile, 25.5),
			new SemicolonToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile)
		);

		var result = new UdmfParser(lexer).Parse();

		result.Vertices.Length.ShouldBe(1);
		result.Vertices[0].X.ShouldBe(10);
		result.Vertices[0].Y.ShouldBe(25.5);
	}

	[Fact]
	public void ShouldRejectDuplicateFieldDefinitions()
	{
		var lexer = new FakeLexer(
			new IdentifierToken(FilePosition.StartOfFile, "namespace"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "zdoom"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "vertex"),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "x"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 10),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "x"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 20),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "y"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 30),
			new SemicolonToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile)
		);

		var exception = Should.Throw<ParsingException>(() => new UdmfParser(lexer).Parse());
		exception.Message.ShouldContain("Duplicate field definition found: x");
	}

	[Fact]
	public void ShouldRejectUnknownBlockNames()
	{
		var lexer = new FakeLexer(
			new IdentifierToken(FilePosition.StartOfFile, "namespace"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "zdoom"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "unknown"),
			new OpenBraceToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile)
		);

		var exception = Should.Throw<ParsingException>(() => new UdmfParser(lexer).Parse());
		exception.Message.ShouldBe("Unknown block: unknown");
	}

	[Fact]
	public void ShouldHandleParsingDemoMap()
	{
		var map = DemoMap.Create();

		using var stream = new MemoryStream();
		map.WriteTo(stream);

		stream.Position = 0;

		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = new DirectLexer(textReader);
		var result = new UdmfParser(lexer).Parse();

		UdmfComparison.AssertEqual(result, map);
	}

	private sealed class FakeLexer(params Token[] tokens) : IDirectLexer
	{
		private readonly Token[] _tokens = tokens;
		private int _position = -1;
		private Token? _bufferedToken;

		public Identifier ReadIdentifier()
		{
			Token token = ReadNext();
			return token is IdentifierToken identifierToken
				? identifierToken.Id
				: throw new InvalidOperationException($"Expected identifier but found {token.GetType().Name}");
		}

		public int ReadInteger()
		{
			Token token = ReadNext();
			return token is IntegerToken integerToken
				? integerToken.Value
				: throw new InvalidOperationException($"Expected integer but found {token.GetType().Name}");
		}

		public double ReadDouble()
		{
			Token token = ReadNext();
			return token switch
			{
				IntegerToken integerToken => integerToken.Value,
				FloatToken floatToken => floatToken.Value,
				_ => throw new InvalidOperationException($"Expected number but found {token.GetType().Name}"),
			};
		}

		public bool ReadBoolean()
		{
			Token token = ReadNext();
			return token is BooleanToken booleanToken
				? booleanToken.Value
				: throw new InvalidOperationException($"Expected boolean but found {token.GetType().Name}");
		}

		public string ReadString()
		{
			Token token = ReadNext();
			return token is StringToken stringToken
				? stringToken.Value
				: throw new InvalidOperationException($"Expected string but found {token.GetType().Name}");
		}

		public void ExpectEquals() => Expect<EqualsToken>("=");

		public void ExpectSemicolon() => Expect<SemicolonToken>(";");

		public void ExpectComma() => Expect<CommaToken>(",");

		public void ExpectOpenBrace() => Expect<OpenBraceToken>("{");

		public void ExpectCloseBrace() => Expect<CloseBraceToken>("}");

		public bool TryReadIdentifier(out Identifier name)
		{
			if (!TryReadNext(out Token? token))
			{
				name = default;
				return false;
			}

			if (token is not IdentifierToken identifierToken)
			{
				throw new InvalidOperationException($"Expected identifier but found {token?.GetType().Name}");
			}

			name = identifierToken.Id;
			return true;
		}

		public bool TryExpectEquals() => TryExpect<EqualsToken>();

		public bool TryExpectOpenBrace() => TryExpect<OpenBraceToken>();

		public bool TryExpectCloseBrace() => TryExpect<CloseBraceToken>();

		public void SkipValueAndSemicolon()
		{
			Token token = ReadNext();
			if (token is not IntegerToken and not FloatToken and not BooleanToken and not StringToken)
			{
				throw new InvalidOperationException($"Expected value but found {token.GetType().Name}");
			}

			ExpectSemicolon();
		}

		private Token ReadNext() =>
			TryReadNext(out Token? token)
				? token!
				: throw new InvalidOperationException("Unexpected end of fake token stream.");

		private void Expect<TToken>(string expectedName)
			where TToken : Token
		{
			Token token = ReadNext();
			if (token is not TToken)
			{
				throw new InvalidOperationException($"Expected {expectedName} but found {token.GetType().Name}");
			}
		}

		private bool TryExpect<TToken>()
			where TToken : Token
		{
			if (_bufferedToken is not null)
			{
				if (_bufferedToken is TToken)
				{
					_bufferedToken = null;
					return true;
				}

				return false;
			}

			if (!TryReadNext(out Token? token))
			{
				return false;
			}

			if (token is TToken)
			{
				return true;
			}

			_bufferedToken = token;
			return false;
		}

		private bool TryReadNext(out Token? token)
		{
			if (_bufferedToken is not null)
			{
				token = _bufferedToken;
				_bufferedToken = null;
				return true;
			}

			int nextIndex = _position + 1;
			if (nextIndex >= _tokens.Length)
			{
				token = null;
				return false;
			}

			_position = nextIndex;
			token = _tokens[_position];
			return true;
		}
	}
}
