// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Xlat
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record TileTemplate(
        ushort OldNum,
        string TextureEast,
        string TextureNorth,
        string TextureWest,
        string TextureSouth,
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
}
