// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record LineDef(
    int V1,
    int V2,
    int SideFront,
    int Id = -1,
    bool Blocking = false,
    bool BlockMonsters = false,
    bool TwoSided = false,
    bool DontPegTop = false,
    bool DontPegBottom = false,
    bool Secret = false,
    bool BlockSound = false,
    bool DontDraw = false,
    bool Mapped = false,
    int Special = 0,
    int Arg0 = 0,
    int Arg1 = 0,
    int Arg2 = 0,
    int Arg3 = 0,
    int Arg4 = 0,
    int SideBack = -1,
    string Comment = ""
);
