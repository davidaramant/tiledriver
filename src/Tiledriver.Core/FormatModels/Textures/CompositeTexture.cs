// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Textures
{
    public sealed record CompositeTexture(
        string Name,
        int Width,
        int Height,
        ImmutableArray<Patch> Patches,
        bool Optional = false,
        TextureNamespace Namespace = TextureNamespace.Texture,
        TextureOffset Offset = new(),
        double XScale = 1,
        double YScale = 1,
        bool WorldPanning = false,
        bool NoDecals = false);
}