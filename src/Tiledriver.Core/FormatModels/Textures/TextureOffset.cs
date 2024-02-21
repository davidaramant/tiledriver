// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.Textures
{
	public readonly struct TextureOffset
	{
		public readonly int Horizontal;
		public readonly int Vertical;

		public TextureOffset(int horizontal, int vertical)
		{
			Horizontal = horizontal;
			Vertical = vertical;
		}
	}
}
