// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    [DebuggerDisplay("{Name} (block)")]
    public sealed class MapInfoBlock : IMapInfoElement
    {
        bool IMapInfoElement.IsBlock => true;
        MapInfoBlock IMapInfoElement.AsBlock() => this;
        MapInfoProperty IMapInfoElement.AsAssignment()
        {
            throw new NotSupportedException();
        }

        public Identifier Name { get; }
        public ImmutableArray<string> Metadata { get; }
        public IEnumerable<IMapInfoElement> Children { get; }

        public MapInfoBlock(Identifier name, IEnumerable<string> metadata, IEnumerable<IMapInfoElement> children)
        {
            Name = name;
            Metadata = metadata.ToImmutableArray();
            Children = children;
        }
    }
}