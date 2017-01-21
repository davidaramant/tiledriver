// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class MapInfoProperty : IMapInfoElement
    {
        bool IMapInfoElement.IsBlock => false;
        MapInfoBlock IMapInfoElement.AsBlock()
        {
            throw new NotSupportedException();
        }
        MapInfoProperty IMapInfoElement.AsAssignment() => this;


        public Identifier Name { get; }
        public ImmutableArray<string> Values { get; }

        public MapInfoProperty(Identifier name, IEnumerable<string> values)
        {
            Name = name;
            Values = values.ToImmutableArray();
        }

        public MapInfoProperty(Identifier name) : this(name, ImmutableArray<string>.Empty)
        {
        }

        public override string ToString()
        {
            return $"{Name} = {string.Join(", ", Values)}";
        }
    }
}