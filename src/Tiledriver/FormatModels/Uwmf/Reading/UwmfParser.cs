using System.Collections.Immutable;
using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Udmf.Reading;

namespace Tiledriver.FormatModels.Uwmf.Reading;

public sealed partial class UwmfParser
{
	[global::System.Flags]
	private enum TopLevelFields : byte
	{
		None = 0,
		NameSpace = 1 << 0,
		TileSize = 1 << 1,
		Name = 1 << 2,
		Width = 1 << 3,
		Height = 1 << 4,
		Comment = 1 << 5,
	}

	private readonly IDirectLexer _lexer;
	private string? _namespace;
	private int _tileSize;
	private string? _name;
	private int _width;
	private int _height;
	private string _comment = string.Empty;
	private TopLevelFields _seenTopLevelFields;
	private readonly ImmutableArray<Tile>.Builder _tileBuilder = ImmutableArray.CreateBuilder<Tile>();
	private readonly ImmutableArray<Sector>.Builder _sectorBuilder = ImmutableArray.CreateBuilder<Sector>();
	private readonly ImmutableArray<Zone>.Builder _zoneBuilder = ImmutableArray.CreateBuilder<Zone>();
	private readonly ImmutableArray<Plane>.Builder _planeBuilder = ImmutableArray.CreateBuilder<Plane>();
	private readonly ImmutableArray<ImmutableArray<MapSquare>>.Builder _planeMapBuilder = ImmutableArray.CreateBuilder<
		ImmutableArray<MapSquare>
	>();
	private readonly ImmutableArray<Thing>.Builder _thingBuilder = ImmutableArray.CreateBuilder<Thing>();
	private readonly ImmutableArray<Trigger>.Builder _triggerBuilder = ImmutableArray.CreateBuilder<Trigger>();

	public UwmfParser(IDirectLexer lexer)
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
			SetTopLevelField(identifier, TopLevelFields.NameSpace);
			_namespace = _lexer.ReadString();
			_lexer.ExpectSemicolon();
		}
		else if (identifier.EqualsIgnoreCase("tileSize"))
		{
			SetTopLevelField(identifier, TopLevelFields.TileSize);
			_tileSize = _lexer.ReadInteger();
			_lexer.ExpectSemicolon();
		}
		else if (identifier.EqualsIgnoreCase("name"))
		{
			SetTopLevelField(identifier, TopLevelFields.Name);
			_name = _lexer.ReadString();
			_lexer.ExpectSemicolon();
		}
		else if (identifier.EqualsIgnoreCase("width"))
		{
			SetTopLevelField(identifier, TopLevelFields.Width);
			_width = _lexer.ReadInteger();
			_lexer.ExpectSemicolon();
		}
		else if (identifier.EqualsIgnoreCase("height"))
		{
			SetTopLevelField(identifier, TopLevelFields.Height);
			_height = _lexer.ReadInteger();
			_lexer.ExpectSemicolon();
		}
		else if (identifier.EqualsIgnoreCase("comment"))
		{
			SetTopLevelField(identifier, TopLevelFields.Comment);
			_comment = _lexer.ReadString();
			_lexer.ExpectSemicolon();
		}
		else
		{
			_lexer.SkipValueAndSemicolon();
		}
	}

	private void AddParsedBlock(Identifier blockName)
	{
		if (blockName.EqualsIgnoreCase("planemap"))
		{
			_planeMapBuilder.Add(ParsePlaneMapBlock());
			return;
		}

		AddGeneratedParsedBlock(blockName);
	}

	private ImmutableArray<MapSquare> ParsePlaneMapBlock()
	{
		var cache = new Dictionary<MapSquare, MapSquare>();
		var mapSquares = ImmutableArray.CreateBuilder<MapSquare>();

		if (_lexer.TryExpectCloseBrace())
		{
			return mapSquares.ToImmutable();
		}

		while (true)
		{
			_lexer.ExpectOpenBrace();
			MapSquare square = ReadMapSquare();

			if (!cache.TryGetValue(square, out MapSquare? cachedSquare))
			{
				cachedSquare = square;
				cache.Add(square, square);
			}

			mapSquares.Add(cachedSquare);

			if (_lexer.TryExpectCloseBrace())
			{
				return mapSquares.ToImmutable();
			}

			_lexer.ExpectComma();
		}
	}

	private MapSquare ReadMapSquare()
	{
		int tile = _lexer.ReadInteger();
		_lexer.ExpectComma();
		int sector = _lexer.ReadInteger();
		_lexer.ExpectComma();
		int zone = _lexer.ReadInteger();

		if (_lexer.TryExpectCloseBrace())
		{
			return new MapSquare(Tile: tile, Sector: sector, Zone: zone);
		}

		_lexer.ExpectComma();
		int tag = _lexer.ReadInteger();
		_lexer.ExpectCloseBrace();
		return new MapSquare(Tile: tile, Sector: sector, Zone: zone, Tag: tag);
	}

	private Texture ParseTextureFieldValue() => new(_lexer.ReadString());

	private void SetTopLevelField(Identifier fieldName, TopLevelFields field)
	{
		if ((_seenTopLevelFields & field) == field)
		{
			throw DuplicateField(fieldName);
		}

		_seenTopLevelFields |= field;
	}

	private MapData CreateMapData()
	{
		if (_namespace is null)
		{
			throw MissingRequiredField("namespace");
		}

		if ((_seenTopLevelFields & TopLevelFields.TileSize) == 0)
		{
			throw MissingRequiredField("tileSize");
		}

		if (_name is null)
		{
			throw MissingRequiredField("name");
		}

		if ((_seenTopLevelFields & TopLevelFields.Width) == 0)
		{
			throw MissingRequiredField("width");
		}

		if ((_seenTopLevelFields & TopLevelFields.Height) == 0)
		{
			throw MissingRequiredField("height");
		}

		return new MapData(
			NameSpace: _namespace,
			TileSize: _tileSize,
			Name: _name,
			Width: _width,
			Height: _height,
			Tiles: _tileBuilder.ToImmutable(),
			Sectors: _sectorBuilder.ToImmutable(),
			Zones: _zoneBuilder.ToImmutable(),
			Planes: _planeBuilder.ToImmutable(),
			PlaneMaps: _planeMapBuilder.ToImmutable(),
			Things: _thingBuilder.ToImmutable(),
			Triggers: _triggerBuilder.ToImmutable(),
			Comment: _comment
		);
	}

	private static ParsingException DuplicateField(Identifier fieldName) =>
		new($"Duplicate field definition found: {fieldName}");

	private static ParsingException MissingRequiredField(string fieldName) =>
		new($"Missing required field '{fieldName}'");
}
