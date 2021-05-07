// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.MapInfo
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record SpecialAction(
        string ActorClass,
        string Special,
        int Arg0,
        int Arg1,
        int Arg2,
        int Arg3,
        int Arg4
    );
}
