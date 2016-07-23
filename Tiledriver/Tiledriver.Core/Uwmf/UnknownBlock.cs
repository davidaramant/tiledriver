﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Core.Uwmf
{
    public sealed class UnknownBlock : BaseUwmfBlock, IWriteableUwmfBlock
    {
        public Identifier Name { get; }
        public List<UnknownProperty> Properties { get; } = new List<UnknownProperty>();

        public UnknownBlock(Identifier name)
        {
            Name = name;
        }

        public Stream WriteTo(Stream stream)
        {
            WriteLine(stream, (string)Name);
            WriteLine(stream, "{");
            foreach (var property in Properties)
            {
                WritePropertyVerbatim(stream, (string)property.Name, property.Value, indent: true);
            }
            WriteLine(stream, "}");

            return stream;
        }
    }
}