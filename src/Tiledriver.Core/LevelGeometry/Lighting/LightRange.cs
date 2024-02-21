// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry.Lighting;

/// <summary>
/// The range of lighting levels.
/// </summary>
/// <param name="DarkLevels">How many levels of darkness there are.</param>
/// <param name="LightLevels">How many levels of light there are (IE overbrights).</param>
public sealed record LightRange(int DarkLevels, int LightLevels)
{
	public int Total => DarkLevels + 1 + LightLevels;
}
