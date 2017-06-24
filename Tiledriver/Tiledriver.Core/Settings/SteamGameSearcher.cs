// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using Functional.Maybe;
using Microsoft.Win32;
using Tiledriver.Core.Extensions.MaybeFileSystem;

namespace Tiledriver.Core.Settings
{
    public static class SteamGameSearcher
    {
        private static Maybe<string> GetBaseSteamPath()
        {
            var steamCommonRegPath = Path.Combine("Software", "Valve", "Steam");

            var key = Registry.CurrentUser.OpenSubKey(steamCommonRegPath);

            if (key != null)
            {
                return (key.GetValue("SteamPath") as string).ToMaybe();
            }

            key = Registry.LocalMachine.OpenSubKey(steamCommonRegPath);
            if (key != null)
            {
                return (key.GetValue("InstallPath") as string).ToMaybe();
            }

            return Maybe<string>.Nothing;
        }

        public static GamePaths GetGamePaths()
        {
            var steamPath = GetBaseSteamPath()
                .Select(path => Path.Combine(path, "SteamApps", "common"));

            var wolf3DPath = steamPath.Select(path => Path.Combine(path, "Wolfenstein 3D")).VerifyPathExists();
            var sodPath = steamPath.Select(path => Path.Combine(path, "Spear of Destiny")).VerifyPathExists();

            return new GamePaths(wolf3D: wolf3DPath, spearOfDestiny: sodPath);
        }
    }
}
