// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.CodeDom.Compiler;

#nullable enable
namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial class VertexBuilder
{
    double? X { get; set; }
    double? Y { get; set; }
    string Comment { get; set; } = "";

    public Vertex Build() =>
        new(
            X: X ?? throw new ArgumentNullException("X must have a value assigned."),
            Y: Y ?? throw new ArgumentNullException("Y must have a value assigned."),
            Comment: Comment
        );
}
