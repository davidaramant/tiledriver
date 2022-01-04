// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

/// <summary>
/// A point in the edge lattice.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public sealed class LatticePoint : IEquatable<LatticePoint?>
{
    /// <summary>
    /// Which square the point belongs to.
    /// </summary>
    public Position Square { get; }

    /// <summary>
    /// The point inside the square.
    /// </summary>
    public SquarePoint Point { get; }

    public LatticePoint(Position square, SquarePoint point)
    {
        (Square, Point) = point switch
        {
            SquarePoint.RightMiddle => (square.Right(), SquarePoint.LeftMiddle),
            SquarePoint.BottomMiddle => (square.Below(), SquarePoint.TopMiddle),
            _ => (square, point)
        };
    }

    public override string ToString() => $"({Square.X}, {Square.Y}, {Point})";

    #region Equality
    public override bool Equals(object? obj) => Equals(obj as LatticePoint);
    public bool Equals(LatticePoint? other) => other != null && Square.Equals(other.Square) && Point == other.Point;
    public override int GetHashCode() => HashCode.Combine(Square, Point);

    public static bool operator ==(LatticePoint? left, LatticePoint? right) => EqualityComparer<LatticePoint>.Default.Equals(left, right);
    public static bool operator !=(LatticePoint? left, LatticePoint? right) => !(left == right);
    #endregion
}