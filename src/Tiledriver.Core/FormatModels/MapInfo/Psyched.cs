﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.MapInfo;

public sealed class Psyched
{
	public string Color1 { get; }
	public string Color2 { get; }
	public int? Offset { get; }

	public Psyched(string color1, string color2, int? offset = null)
	{
		Color1 = color1;
		Color2 = color2;
		Offset = offset;
	}
}
