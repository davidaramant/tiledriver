// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public static class SquareSegmentsExtensions
{
    public static SquareSegments ToSquareSegments(this SquareSegment seg) => (SquareSegments)(1 << (int)seg);

    public static IEnumerable<SquareSegment> GetAllSegments() => Enumerable.Range(0, 8).Cast<SquareSegment>();
}
