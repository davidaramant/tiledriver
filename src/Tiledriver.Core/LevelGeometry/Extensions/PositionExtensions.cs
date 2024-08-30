// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using Tiledriver.Core.LevelGeometry.CoordinateSystems;

namespace Tiledriver.Core.LevelGeometry.Extensions;

public enum Neighborhood
{
	/// <summary>
	/// 4-way neighborhood
	/// </summary>
	VonNeumann,

	/// <summary>
	/// 8-way neighborhood
	/// </summary>
	Moore,
}

public static class PositionExtensions
{
	public static bool Touches(this Position p, Position other) =>
		(p.X == other.X && (p.Y == other.Y - 1 || p.Y == other.Y + 1))
		|| (p.Y == other.Y && (p.X == other.X - 1 || p.X == other.X + 1));

	/// <summary>
	/// Gets the 4 neighbors of this position.
	/// </summary>
	public static IEnumerable<Position> GetVonNeumannNeighbors(this Position p)
	{
		// Coordinate system doesn't matter for this
		yield return p + CoordinateSystem.BottomLeft.Up;
		yield return p + CoordinateSystem.BottomLeft.Right;
		yield return p + CoordinateSystem.BottomLeft.Down;
		yield return p + CoordinateSystem.BottomLeft.Left;
	}

	/// <summary>
	/// Gets the 8 neighbors of this position.
	/// </summary>
	public static IEnumerable<Position> GetMooreNeighbors(this Position p)
	{
		// Coordinate system doesn't matter for this
		yield return p + CoordinateSystem.BottomLeft.Up;
		yield return p + CoordinateSystem.BottomLeft.UpAndRight;
		yield return p + CoordinateSystem.BottomLeft.Right;
		yield return p + CoordinateSystem.BottomLeft.DownAndRight;
		yield return p + CoordinateSystem.BottomLeft.Down;
		yield return p + CoordinateSystem.BottomLeft.DownAndLeft;
		yield return p + CoordinateSystem.BottomLeft.Left;
		yield return p + CoordinateSystem.BottomLeft.UpAndLeft;
	}

	public static IEnumerable<Position> GetNeighbors(this Position p, Neighborhood neighborhood) =>
		neighborhood switch
		{
			Neighborhood.Moore => p.GetMooreNeighbors(),
			Neighborhood.VonNeumann => p.GetVonNeumannNeighbors(),
			_ => throw new Exception("Unknown type"),
		};
}
