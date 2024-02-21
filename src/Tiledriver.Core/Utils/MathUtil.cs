// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Utils;

public static class MathUtil
{
	public static int Min(int v1, int v2, int v3, int v4) => Math.Min(v1, Math.Min(v2, Math.Min(v3, v4)));

	public static int Max(int v1, int v2, int v3, int v4) => Math.Max(v1, Math.Max(v2, Math.Max(v3, v4)));
}
