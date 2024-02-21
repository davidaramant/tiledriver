// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.Uwmf
{
	public sealed partial record MapSquare
	{
		public bool HasTile => Tile != -1;
	}
}
