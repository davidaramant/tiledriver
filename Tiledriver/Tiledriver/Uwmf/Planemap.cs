using System;
using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Planemap : IUwmfEntry
    {
        public readonly List<PlanemapEntry> Entries = new List<PlanemapEntry>();

        public StreamWriter Write(StreamWriter writer)
        {
            return writer.
                Line("planemap").
                Line("{").
                Line(String.Join(",\n",Entries)).
                Line("}");
        }
    }
}