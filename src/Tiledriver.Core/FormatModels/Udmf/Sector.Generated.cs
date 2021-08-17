// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Udmf
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record Sector(
        Texture TextureFloor,
        Texture TextureCeiling,
        int HeightFloor = 0,
        int HeightCeiling = 0,
        int LightLevel = 160,
        int Special = 0,
        int Id = 0,
        string Comment = ""
    );
}
