// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Uwmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Sector(
	Texture TextureCeiling,
	Texture TextureFloor,
	string Comment = ""
);
