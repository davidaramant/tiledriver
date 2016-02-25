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