using System.Collections.Immutable;
using Tiledriver.FormatModels.Common;

namespace Tiledriver.FormatModels.Udmf.Reading;

public sealed partial class UdmfParser
{
	private readonly IDirectLexer _lexer;
	private string? _namespace;
	private string _comment = "";
	private readonly ImmutableArray<Thing>.Builder _thingBuilder = ImmutableArray.CreateBuilder<Thing>();
	private readonly ImmutableArray<Vertex>.Builder _verticesBuilder = ImmutableArray.CreateBuilder<Vertex>();
	private readonly ImmutableArray<LineDef>.Builder _lineDefBuilder = ImmutableArray.CreateBuilder<LineDef>();
	private readonly ImmutableArray<SideDef>.Builder _sideDefBuilder = ImmutableArray.CreateBuilder<SideDef>();
	private readonly ImmutableArray<Sector>.Builder _sectorBuilder = ImmutableArray.CreateBuilder<Sector>();

	public UdmfParser(IDirectLexer lexer)
	{
		ArgumentNullException.ThrowIfNull(lexer);
		_lexer = lexer;
	}

	public MapData Parse()
	{
		while (_lexer.TryReadIdentifier(out Identifier identifier))
		{
			if (_lexer.TryExpectEquals())
			{
				ParseTopLevelAssignment(identifier);
			}
			else if (_lexer.TryExpectOpenBrace())
			{
				AddParsedBlock(identifier);
			}
			else
			{
				throw new ParsingException($"Expected '=' or '{{' after identifier '{identifier}'");
			}
		}

		return CreateMapData();
	}

	private void ParseTopLevelAssignment(Identifier identifier)
	{
		if (identifier.EqualsIgnoreCase("namespace"))
		{
			if (_namespace is not null)
			{
				throw DuplicateField(identifier);
			}

			_namespace = _lexer.ReadString();
			_lexer.ExpectSemicolon();
		}
		else if (identifier.EqualsIgnoreCase("comment"))
		{
			_comment = _lexer.ReadString();
			_lexer.ExpectSemicolon();
		}
		else
		{
			_lexer.SkipValueAndSemicolon();
		}
	}

	private Texture ParseTextureFieldValue(bool optional)
	{
		string name = _lexer.ReadString();
		return optional && name == "-" ? Texture.None : new Texture(name);
	}

	private static ParsingException DuplicateField(Identifier fieldName) =>
		new($"Duplicate field definition found: {fieldName}");

	private static ParsingException MissingRequiredField(Identifier blockName, string fieldName) =>
		new($"Missing required field '{fieldName}' in '{blockName}'");
}
