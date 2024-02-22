// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.Utils;

namespace Tiledriver.Core.Settings;

public static class SteamGameSearcher
{
	private static string? GetBaseSteamPath()
	{
		return "C:\\Program Files (x86)\\Steam";
		// TODO: "Will .NET 6 allow this to live in the same project?");
		//var steamCommonRegPath = Path.Combine("Software", "Valve", "Steam");

		//var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(steamCommonRegPath);

		//if (key != null)
		//{
		//    return (key.GetValue("SteamPath") as string);
		//}

		//key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(steamCommonRegPath);
		//if (key != null)
		//{
		//    return (key.GetValue("InstallPath") as string);
		//}

		//return null;
	}

	public static GamePaths GetGamePaths()
	{
		var steamPath = PathUtil.Combine(GetBaseSteamPath(), "SteamApps", "common");

		var wolf3DPath = PathUtil.Combine(steamPath, "Wolfenstein 3D", "base");
		var sodPath = PathUtil.Combine(steamPath, "Spear of Destiny", "base");

		return new GamePaths(
			Wolf3D: PathUtil.VerifyPathExists(wolf3DPath),
			SpearOfDestiny: PathUtil.VerifyPathExists(sodPath),
			Doom: string.Empty
		);
	}
}
