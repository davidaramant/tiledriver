// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Uwmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record MapSquare(
    int Tile,
    int Sector,
    int Zone,
    int Tag = 0
);
