// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Udmf
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record SideDef(
        int Sector,
        int OffsetX = 0,
        int OffsetY = 0,
        string TextureTop = "-",
        string TextureBottom = "-",
        string TextureMiddle = "-",
        string Comment = ""
    );
}
