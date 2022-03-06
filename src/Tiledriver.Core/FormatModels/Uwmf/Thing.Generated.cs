// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Uwmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Thing(
    string Type,
    double X,
    double Y,
    double Z,
    int Angle,
    bool Ambush = false,
    bool Patrol = false,
    bool Skill1 = false,
    bool Skill2 = false,
    bool Skill3 = false,
    bool Skill4 = false,
    string Comment = ""
);
