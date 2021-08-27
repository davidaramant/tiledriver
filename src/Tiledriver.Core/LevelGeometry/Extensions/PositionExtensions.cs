// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;

namespace Tiledriver.Core.LevelGeometry.Extensions
{
    public enum Neighborhood
    {
        /// <summary>
        /// 4-way neighborhood
        /// </summary>
        VonNeumann,
        /// <summary>
        /// 8-way neighborhood
        /// </summary>
        Moore
    }

    public static class PositionExtensions
    {
        public static Position Left(this Position p) => new(p.X - 1, p.Y);
        public static Position Above(this Position p) => new(p.X, p.Y - 1);
        public static Position Right(this Position p) => new(p.X + 1, p.Y);
        public static Position Below(this Position p) => new(p.X, p.Y + 1);

        public static Position AboveLeft(this Position p) => new(p.X - 1, p.Y - 1);
        public static Position AboveRight(this Position p) => new(p.X + 1, p.Y - 1);
        public static Position BelowRight(this Position p) => new(p.X + 1, p.Y + 1);
        public static Position BelowLeft(this Position p) => new(p.X - 1, p.Y + 1);


        public static bool Touches(this Position p, Position other) =>
            (p.X == other.X && (p.Y == other.Y - 1 || p.Y == other.Y + 1)) ||
            (p.Y == other.Y && (p.X == other.X - 1 || p.X == other.X + 1));

        public static IEnumerable<Position> GetVonNeumannNeighbors(this Position p)
        {
            yield return Left(p);
            yield return Above(p);
            yield return Right(p);
            yield return Below(p);
        }

        public static IEnumerable<Position> GetMooreNeighbors(this Position p)
        {
            yield return Left(p);
            yield return AboveLeft(p);
            yield return Above(p);
            yield return AboveRight(p);
            yield return Right(p);
            yield return BelowRight(p);
            yield return Below(p);
            yield return BelowLeft(p);
        }

        public static IEnumerable<Position> GetNeighbors(this Position p, Neighborhood neighborhood) =>
            neighborhood switch
            {
                Neighborhood.Moore => p.GetMooreNeighbors(),
                Neighborhood.VonNeumann => p.GetVonNeumannNeighbors(),
                _ => throw new Exception("Unknown type")
            };
    }
}
