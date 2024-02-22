// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using System.Linq;

namespace Tiledriver.Core.Utils;

public static class PathUtil
{
	public static string? VerifyPathExists(string? path) => Directory.Exists(path) ? path : null;

	public static string? Combine(params string?[] paths) =>
		paths.Any(p => p is null) ? null : Path.Combine(paths.Select(p => p!).ToArray());
}
