// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry.Lighting;

public sealed class LightMap
{
	private readonly Dictionary<Position, int> _lightLevels = new();
	private int _defaultLightLevel = 0;
	public Size Size { get; }
	public LightRange Range { get; }

	public int this[int col, int row] => this[new Position(col, row)];
	public int this[Position p] =>
		_lightLevels.TryGetValue(p, out int light) ? light : (Size.Contains(p) ? _defaultLightLevel : 0);

	public LightMap(LightRange range, Size size)
	{
		Range = range;
		Size = size;
	}

	public LightMap Blackout()
	{
		_defaultLightLevel = -Range.DarkLevels;
		return this;
	}

	public void Lighten(Position point, int amount)
	{
		if (!Size.Contains(point))
			return;

		if (!_lightLevels.TryGetValue(point, out int light))
		{
			light = _defaultLightLevel;
		}

		light = Math.Min(light + amount, Range.LightLevels);

		_lightLevels[point] = light;
	}
}
