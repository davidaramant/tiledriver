// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using Tiledriver.Core.Utils.ECWolf;
using Tiledriver.Core.Utils.GZDoom;

namespace Tiledriver.Core.Settings;

public sealed record TiledriverConfig(string ECWolfPath, string GZDoomPath, GamePaths GamePaths)
{
	public ECWolfLauncher CreateECWolfLauncher() => new(ECWolfPath);

	public GZDoomLauncher CreateGZDoomLauncher() =>
		new(new DoomConfig(GZDoomPath, GamePaths.Doom2IWad ?? throw new ArgumentException("No Doom path specified")));
}
