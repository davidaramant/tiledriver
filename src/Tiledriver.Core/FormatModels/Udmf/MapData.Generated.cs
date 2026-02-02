// Copyright (c) 2026, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record MapData(
	string NameSpace,
	ImmutableArray<Thing> Things,
	ImmutableArray<Vertex> Vertices,
	ImmutableArray<LineDef> LineDefs,
	ImmutableArray<SideDef> SideDefs,
	ImmutableArray<Sector> Sectors,
	string Comment = ""
);
