// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;

namespace Tiledriver.Core.Settings
{
    public sealed record GamePaths(
        string? Wolf3D,
        string? SpearOfDestiny,
        string? Doom)
    {
        public bool Complete => Wolf3D != null && SpearOfDestiny != null && Doom != null;

        public string? Doom2IWad
        {
            get
            {
                if (Doom == null)
                    return null;

                return Path.Combine(Doom, "doom2.wad");
            }
        }

        public GamePaths MergeWith(GamePaths other) =>
            new(
                Wolf3D ?? other.Wolf3D,
                SpearOfDestiny ?? other.SpearOfDestiny,
                Doom ?? other.Doom);
    }
}
