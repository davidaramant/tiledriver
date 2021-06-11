// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    public enum LightHeight
    {
        Middle,
        Floor,
        Ceiling
    }

    public sealed record LightDefinition(
        Position Location,
        int Radius,
        LightHeight Height = LightHeight.Middle)
    {
        /// <summary>
        /// The 1D measure of how many tiles are affected by this light.
        /// </summary>
        /// <remarks>
        /// This is different from the diameter, because the light is assumed to be in the center of a tile. At minimum,
        /// it affects that one tile.
        /// </remarks>
        public int LengthAffected => 2 * Radius + 1;
    }
}