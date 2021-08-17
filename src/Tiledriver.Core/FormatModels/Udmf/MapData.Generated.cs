// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Udmf
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record MapData(
        string NameSpace,
        ImmutableList<LineDef> LineDefs,
        ImmutableList<SideDef> SideDefs,
        ImmutableList<Vertex> Vertices,
        ImmutableList<Sector> Sectors,
        ImmutableList<Thing> Things,
        string Comment = ""
    );
}
