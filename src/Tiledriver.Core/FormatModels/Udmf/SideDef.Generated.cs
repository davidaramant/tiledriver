// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record SideDef(
	Texture TextureTop,
	Texture TextureBottom,
	Texture TextureMiddle,
	int Sector,
	int OffsetX = 0,
	int OffsetY = 0,
	string Comment = ""
);
