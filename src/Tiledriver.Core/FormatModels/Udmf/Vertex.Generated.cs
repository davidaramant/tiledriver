// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Vertex(
    double X,
    double Y,
    string Comment = ""
);
