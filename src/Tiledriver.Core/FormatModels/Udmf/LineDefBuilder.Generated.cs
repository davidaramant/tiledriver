// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.CodeDom.Compiler;

#nullable enable
namespace Tiledriver.Core.FormatModels.Udmf
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial class LineDefBuilder
    {
        int? V1 { get; set; }
        int? V2 { get; set; }
        int? SideFront { get; set; }
        int Id { get; set; } = -1;
        bool Blocking { get; set; } = false;
        bool BlockMonsters { get; set; } = false;
        bool TwoSided { get; set; } = false;
        bool DontPegTop { get; set; } = false;
        bool DontPegBottom { get; set; } = false;
        bool Secret { get; set; } = false;
        bool BlockSound { get; set; } = false;
        bool DontDraw { get; set; } = false;
        bool Mapped { get; set; } = false;
        int Special { get; set; } = 0;
        int Arg0 { get; set; } = 0;
        int Arg1 { get; set; } = 0;
        int Arg2 { get; set; } = 0;
        int Arg3 { get; set; } = 0;
        int Arg4 { get; set; } = 0;
        int SideBack { get; set; } = -1;
        string Comment { get; set; } = "";

        public LineDef Build() =>
            new(
                V1: V1 ?? throw new ArgumentNullException("V1 must have a value assigned."),
                V2: V2 ?? throw new ArgumentNullException("V2 must have a value assigned."),
                SideFront: SideFront ?? throw new ArgumentNullException("SideFront must have a value assigned."),
                Id: Id,
                Blocking: Blocking,
                BlockMonsters: BlockMonsters,
                TwoSided: TwoSided,
                DontPegTop: DontPegTop,
                DontPegBottom: DontPegBottom,
                Secret: Secret,
                BlockSound: BlockSound,
                DontDraw: DontDraw,
                Mapped: Mapped,
                Special: Special,
                Arg0: Arg0,
                Arg1: Arg1,
                Arg2: Arg2,
                Arg3: Arg3,
                Arg4: Arg4,
                SideBack: SideBack,
                Comment: Comment
            );
    }
}
