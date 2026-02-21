using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry;

public sealed class Canvas : ICanvas
{
	private readonly int[] _tiles;
	private readonly int[] _sectors;
	private readonly int[] _zones;
	private readonly int[] _tags;

	public Size Dimensions { get; }

	public MapSquare this[Position pos]
	{
		get => this[pos.X, pos.Y];
		set => this[pos.X, pos.Y] = value;
	}

	public MapSquare this[int x, int y]
	{
		get
		{
			var index = GetIndex(x, y);
			return new MapSquare(_tiles[index], _sectors[index], _zones[index], _tags[index]);
		}
		set
		{
			var index = GetIndex(x, y);
			_tiles[index] = value.Tile;
			_sectors[index] = value.Sector;
			_zones[index] = value.Zone;
			_tags[index] = value.Tag;
		}
	}

	public Canvas(Size dimensions)
	{
		Dimensions = dimensions;

		var size = dimensions.Width * dimensions.Height;

		_tiles = new int[size];
		_sectors = new int[size];
		_zones = new int[size];
		_tags = new int[size];

		Array.Fill(_tiles, -1);
		Array.Fill(_tags, -1);
	}

	public Canvas Fill(IEnumerable<MapSquare> tileSpaces)
	{
		int index = 0;
		foreach (var ts in tileSpaces)
		{
			_tiles[index] = ts.Tile;
			_sectors[index] = ts.Sector;
			_zones[index] = ts.Zone;
			_tags[index] = ts.Tag;

			index++;
		}

		return this;
	}

	public ICanvas Set(Position pos, int? tile = null, int? sector = null, int? zone = null, int? tag = null) =>
		Set(pos.X, pos.Y, tile, sector, zone, tag);

	public ICanvas Set(int x, int y, int? tile = null, int? sector = null, int? zone = null, int? tag = null)
	{
		var startIndex = GetIndex(x, y);

		if (tile != null)
		{
			_tiles[startIndex] = (int)tile;
		}
		if (sector != null)
		{
			_sectors[startIndex] = (int)sector;
		}
		if (zone != null)
		{
			_zones[startIndex] = (int)zone;
		}
		if (tag != null)
		{
			_tags[startIndex] = (int)tag;
		}

		return this;
	}

	public ImmutableArray<MapSquare> ToPlaneMap()
	{
		var tileSpaces = new MapSquare[_tiles.Length];

		for (int i = 0; i < _tiles.Length; i++)
		{
			tileSpaces[i] = new MapSquare(_tiles[i], _sectors[i], _zones[i], _tags[i]);
		}

		return [.. tileSpaces];
	}

	public ICanvas ToCanvas() => new Canvas(Dimensions).Fill(ToPlaneMap());

	private int GetIndex(int x, int y) => y * Dimensions.Width + x;
}
