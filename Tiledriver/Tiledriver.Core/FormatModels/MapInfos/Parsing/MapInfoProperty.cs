// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    [DebuggerDisplay("{Name} = {Value}")]
    public sealed class MapInfoProperty : IMapInfoElement
    {
        bool IMapInfoElement.IsBlock => false;
        MapInfoBlock IMapInfoElement.AsBlock()
        {
            throw new NotSupportedException();
        }
        MapInfoProperty IMapInfoElement.AsAssignment() => this;


        public Identifier Name { get; }
        public string Value { get; }

        public MapInfoProperty(Identifier name, string value)
        {
            Name = name;
            Value = value;
        }

        public MapInfoProperty(Identifier name) : this(name, String.Empty)
        {
        }
    }
}