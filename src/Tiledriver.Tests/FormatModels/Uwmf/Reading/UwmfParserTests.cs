using System.Text;
using Shouldly;
using Tiledriver.DemoMaps.Wolf3D;
using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Common.Reading;
using Tiledriver.FormatModels.Udmf.Reading;
using Tiledriver.FormatModels.Uwmf.Reading;
using Tiledriver.FormatModels.Uwmf.Writing;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Uwmf.Reading;

public sealed class UwmfParserTests
{
	[Fact]
	public void ShouldParseTopLevelAssignmentsFromFakeLexer()
	{
		var lexer = new FakeUwmfLexer(
			new IdentifierToken(FilePosition.StartOfFile, "namespace"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "ecwolf"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "tileSize"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 64),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "name"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "test map"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "width"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 10),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "height"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 8),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "comment"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "hello"),
			new SemicolonToken(FilePosition.StartOfFile)
		);

		var result = new UwmfParser(lexer).Parse();

		result.NameSpace.ShouldBe("ecwolf");
		result.TileSize.ShouldBe(64);
		result.Name.ShouldBe("test map");
		result.Width.ShouldBe(10);
		result.Height.ShouldBe(8);
		result.Comment.ShouldBe("hello");
		result.Tiles.ShouldBeEmpty();
		result.PlaneMaps.ShouldBeEmpty();
	}

	[Fact]
	public void ShouldParseTileBlockFromFakeLexer()
	{
		var lexer = CreateMinimalMapLexer(
			new IdentifierToken(FilePosition.StartOfFile, "tile"),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureEast"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "STONE"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureNorth"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "NORTH"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureWest"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "WEST"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureSouth"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "SOUTH"),
			new SemicolonToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile)
		);

		var result = new UwmfParser(lexer).Parse();

		result.Tiles.Length.ShouldBe(1);
		result.Tiles[0].TextureEast.Name.ShouldBe("STONE");
		result.Tiles[0].TextureNorth.Name.ShouldBe("NORTH");
		result.Tiles[0].TextureWest.Name.ShouldBe("WEST");
		result.Tiles[0].TextureSouth.Name.ShouldBe("SOUTH");
	}

	[Fact]
	public void ShouldParsePlaneMapFromFakeLexer()
	{
		var lexer = CreateMinimalMapLexer(
			new IdentifierToken(FilePosition.StartOfFile, "planemap"),
			new OpenBraceToken(FilePosition.StartOfFile),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 1),
			new CommaToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 2),
			new CommaToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 3),
			new CloseBraceToken(FilePosition.StartOfFile),
			new CommaToken(FilePosition.StartOfFile),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 4),
			new CommaToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 5),
			new CommaToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 6),
			new CommaToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 7),
			new CloseBraceToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile)
		);

		var result = new UwmfParser(lexer).Parse();

		result.PlaneMaps.Length.ShouldBe(1);
		result.PlaneMaps[0].Length.ShouldBe(2);
		result.PlaneMaps[0][0].Tile.ShouldBe(1);
		result.PlaneMaps[0][0].Sector.ShouldBe(2);
		result.PlaneMaps[0][0].Zone.ShouldBe(3);
		result.PlaneMaps[0][0].Tag.ShouldBe(0);
		result.PlaneMaps[0][1].Tag.ShouldBe(7);
	}

	[Fact]
	public void ShouldRejectDuplicateFieldDefinitions()
	{
		var lexer = CreateMinimalMapLexer(
			new IdentifierToken(FilePosition.StartOfFile, "tile"),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureEast"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "STONE"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureEast"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "BRICK"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureNorth"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "NORTH"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureWest"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "WEST"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "textureSouth"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "SOUTH"),
			new SemicolonToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile)
		);

		var exception = Should.Throw<ParsingException>(() => new UwmfParser(lexer).Parse());
		exception.Message.ShouldContain("Duplicate field definition found: textureEast");
	}

	[Fact]
	public void ShouldHandleParsingDemoMap()
	{
		var map = ThingDemoMap.Create();

		using var stream = new MemoryStream();
		map.WriteTo(stream);

		stream.Position = 0;

		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var result = new UwmfParser(new DirectLexer(textReader)).Parse();

		UwmfComparison.AssertEqual(result, map);
	}

	[Fact]
	public void ShouldHandleParsingTestFile()
	{
		using var stream = TestFile.Uwmf.TEXTMAP;
		using var textReader = new StreamReader(stream, Encoding.ASCII);

		_ = new UwmfParser(new DirectLexer(textReader)).Parse();
	}

	private static FakeUwmfLexer CreateMinimalMapLexer(params Token[] additionalTokens) =>
		new([
			new IdentifierToken(FilePosition.StartOfFile, "namespace"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "ecwolf"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "tileSize"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 64),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "name"),
			new EqualsToken(FilePosition.StartOfFile),
			new StringToken(FilePosition.StartOfFile, "test map"),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "width"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 2),
			new SemicolonToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, "height"),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 2),
			new SemicolonToken(FilePosition.StartOfFile),
			.. additionalTokens,
		]);

	private sealed class FakeUwmfLexer(params Token[] tokens) : IDirectLexer
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
