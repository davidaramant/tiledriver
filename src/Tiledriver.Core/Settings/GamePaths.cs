// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.Settings
{
    public sealed record GamePaths(
        string? Wolf3D,
        string? SpearOfDestiny)
    {
        public bool Complete => Wolf3D != null && SpearOfDestiny != null;

        public GamePaths MergeWith(GamePaths other) =>
            new(
                Wolf3D ?? other.Wolf3D,
                SpearOfDestiny ?? other.SpearOfDestiny);
    }
}
