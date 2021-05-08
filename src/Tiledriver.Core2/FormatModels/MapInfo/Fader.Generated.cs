// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfo
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record Fader(
        Identifier FadeType,
        IntermissionBackground Background,
        IntermissionDraw Draw,
        string Music,
        double Time
    );
}
