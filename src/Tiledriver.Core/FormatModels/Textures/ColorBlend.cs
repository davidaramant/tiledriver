// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.Textures;

public sealed record ColorBlend(string Color, double? Alpha = null)
{
	public ColorBlend(byte r, byte g, byte b, double? alpha = null)
		: this($"{r:x2}{g:x2}{b:x2}", alpha) { }
}
