// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Episode(
    string Map,
    char Key,
    string Lookup,
    string Name,
    string PicName,
    bool NoSkillMenu = true,
    bool Optional = true,
    bool Remove = true
);
