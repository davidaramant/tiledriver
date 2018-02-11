// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Uwmf
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

        public UnknownBlock Clone()
        {
            var block = new UnknownBlock(Name);
            block.Properties.AddRange(Properties.Select(p => p.Clone()));

            return block;
        }
    }
}