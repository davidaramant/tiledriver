// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.Textures
{
    public enum RenderStyle
    {
        /// <summary>
        /// Renders the patch as normal and solid. This is the default if no Style is specified.
        /// </summary>
        Copy,

        /// <summary>
        /// Causes the patch to be drawn with additive translucency, resulting in brightening effect.
        /// </summary>
        Add,

        /// <summary>
        /// This is the same as Copy, but does additional processing to respect a PNG's alpha channel.
        /// </summary>
        CopyAlpha,

        /// <summary>
        /// This works just like Copy except it multiplies each pixel's alpha channel by the specified Alpha property.
        /// </summary>
        CopyNewAlpha,

        /// <summary>
        /// This has an extreme darkening effect, similar to Photoshop's "burn" style.
        /// </summary>
        Modulate,

        /// <summary>
        /// This is the same as CopyAlpha, except it only copies the patch's alpha channel where it has a higher alpha than what's underneath.
        /// </summary>
        Overlay,

        /// <summary>
        /// Same as subtract, but re-inverts the patch so it appears normal when being applied.
        /// </summary>
        ReverseSubtract,

        /// <summary>
        /// Subtracts the patch from patches below, resulting in a darkening effect. This implies inverting.
        /// </summary>
        Subtract,

        /// <summary>
        /// Applies regular translucency to the patch.
        /// </summary>
        Translucent,
    }
}
