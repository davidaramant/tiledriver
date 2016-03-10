// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Planemap : IUwmfEntry
    {
        public readonly List<PlanemapEntry> Entries = new List<PlanemapEntry>();

        public Planemap(){}

        public Planemap(IEnumerable<PlanemapEntry> entries)
        {
            Entries.AddRange(entries);
        }

        public Stream WriteTo(Stream stream)
        {
            return stream.
                Line("planemap").
                Line("{").
                Line(String.Join(",\n",Entries)).
                Line("}");
        }
    }
}