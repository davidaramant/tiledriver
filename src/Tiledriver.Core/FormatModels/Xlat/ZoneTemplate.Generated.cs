// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Xlat;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record ZoneTemplate(
	ushort OldNum,
	string Comment = ""
);
