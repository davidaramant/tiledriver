// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.Utils.ECWolf;

namespace Tiledriver.Core.Settings
{
    public sealed record TiledriverConfig(
        string ECWolfPath,
        GamePaths GamePaths)
    {
        public Launcher CreateECWolfLauncher() => new(ECWolfPath);
    }
}