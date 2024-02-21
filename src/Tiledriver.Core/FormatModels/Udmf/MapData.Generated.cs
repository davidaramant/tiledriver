// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record MapData(
	string NameSpace,
	ImmutableArray<LineDef> LineDefs,
	ImmutableArray<SideDef> SideDefs,
	ImmutableArray<Vertex> Vertices,
	ImmutableArray<Sector> Sectors,
	ImmutableArray<Thing> Things,
	string Comment = ""
);
