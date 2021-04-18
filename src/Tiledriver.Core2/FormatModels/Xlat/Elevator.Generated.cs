// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Xlat
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record Elevator(
        ushort OldNum
    );
}
