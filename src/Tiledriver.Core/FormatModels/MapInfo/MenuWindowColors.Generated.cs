// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record MenuWindowColors(
	string Background,
	string Top,
	string Bottom,
	string IndexBackground,
	string IndexTop,
	string IndexBottom
);
