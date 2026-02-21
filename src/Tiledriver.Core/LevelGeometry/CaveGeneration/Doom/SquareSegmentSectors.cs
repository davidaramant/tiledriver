using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed class SquareSegmentSectors
{
	private readonly SectorDescription[] _sectors;

	public SquareSegmentSectors(IEnumerable<SectorDescription> sectors) => _sectors = sectors.ToArray();

	public SectorDescription this[SquareSegment id] => _sectors[(int)id];
	public bool IsUniform => _sectors.Skip(1).All(s => s == _sectors[0]);

	public IEnumerable<EdgeSegment> GetInternalEdges()
	{
		var lookup = new List<EdgeSegment>();

		void AddEdge(EdgeSegmentId id, SquareSegment segment1, SquareSegment segment2)
		{
			var side1 = this[segment1];
			var side2 = this[segment2];

			if (side1 != side2)
			{
				var side1IsSmaller = side1.CompareTo(side2) < 0;
				var (front, frontSegment) = side1IsSmaller ? (side1, segment1) : (side2, segment2);
				var back = side1IsSmaller ? side2 : side1;
				var (left, right) = id.GetLeftAndRight(frontSegment);

				lookup.Add(new EdgeSegment(id, front, back, left, right));
			}
		}

		AddEdge(
			EdgeSegmentId.DiagTopLeft,
			segment1: SquareSegment.UpperLeftOuter,
			segment2: SquareSegment.UpperLeftInner
		);
		AddEdge(
			EdgeSegmentId.DiagTopRight,
			segment1: SquareSegment.UpperRightOuter,
			segment2: SquareSegment.UpperRightInner
		);
		AddEdge(
			EdgeSegmentId.DiagBottomRight,
			segment1: SquareSegment.LowerRightInner,
			segment2: SquareSegment.LowerRightOuter
		);
		AddEdge(
			EdgeSegmentId.DiagBottomLeft,
			segment1: SquareSegment.LowerLeftInner,
			segment2: SquareSegment.LowerLeftOuter
		);
		AddEdge(
			EdgeSegmentId.HorizontalLeft,
			segment1: SquareSegment.UpperLeftInner,
			segment2: SquareSegment.LowerLeftInner
		);
		AddEdge(
			EdgeSegmentId.HorizontalRight,
			segment1: SquareSegment.UpperRightInner,
			segment2: SquareSegment.LowerRightInner
		);
		AddEdge(
			EdgeSegmentId.VerticalTop,
			segment1: SquareSegment.UpperLeftInner,
			segment2: SquareSegment.UpperRightInner
		);
		AddEdge(
			EdgeSegmentId.VerticalBottom,
			segment1: SquareSegment.LowerLeftInner,
			segment2: SquareSegment.LowerRightInner
		);

		return lookup;
	}
}
