// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;

namespace Tiledriver.Core.FormatModels.Wad
{
    public sealed class Marker : ILump
    {
        public LumpName Name { get; }
        public bool HasData => false;

        public Marker(LumpName name)
        {
            Name = name;
        }

        public void WriteTo(Stream stream)
        {
            // Do nothing; no data
        }

        public byte[] GetData()
        {
            return Array.Empty<byte>();
        }
    }
}
