// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.Textures
{
    public sealed record Patch(
        string Name,
        int XOrigin,
        int YOrigin,
        PatchNamespace Namespace = PatchNamespace.Patch,
        bool FlipX = false,
        bool FlipY = false,
        bool UseOffsets = false,
        PatchRotation Rotate = PatchRotation.None,
        Translation? Translation = null,
        ColorBlend? Blend = null,
        double Alpha = 1,
        RenderStyle Style = RenderStyle.Copy);
}