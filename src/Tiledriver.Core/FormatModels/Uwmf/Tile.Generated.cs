// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Uwmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Tile(
    Texture TextureEast,
    Texture TextureNorth,
    Texture TextureWest,
    Texture TextureSouth,
    bool BlockingEast = true,
    bool BlockingNorth = true,
    bool BlockingWest = true,
    bool BlockingSouth = true,
    bool OffsetVertical = false,
    bool OffsetHorizontal = false,
    bool DontOverlay = false,
    int Mapped = 0,
    string SoundSequence = "",
    string TextureOverhead = "",
    string Comment = ""
);
