using Tiledriver.Core.Utils.ECWolf;
using Tiledriver.Core.Utils.UZDoom;

namespace Tiledriver.Core.Settings;

public sealed record TiledriverConfig(string ECWolfPath, string UZDoomPath, GamePaths GamePaths)
{
	public ECWolfLauncher CreateECWolfLauncher() => new(ECWolfPath);

	public UZDoomLauncher CreateUZDoomLauncher() =>
		new(new DoomConfig(UZDoomPath, GamePaths.Doom2IWad ?? throw new ArgumentException("No Doom path specified")));
}
