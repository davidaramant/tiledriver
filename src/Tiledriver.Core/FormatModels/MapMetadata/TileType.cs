// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.MapMetadata
{
	public enum TileType : byte
	{
		Unreachable = 0,
		Empty = 1,
		Wall = 2,
		PushWall = 3,
		Door = 4,
	}
}
