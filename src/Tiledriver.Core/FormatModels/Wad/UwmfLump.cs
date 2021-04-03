// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using JetBrains.Annotations;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.FormatModels.Wad
{
    public sealed class UwmfLump : ILump
    {
        private readonly MapData _mapData;
        public LumpName Name { get; }
        public bool HasData => true;


        public UwmfLump([NotNull]LumpName name, [NotNull]MapData mapData)
        {
            Name = name;
            _mapData = mapData;
        }

        public void WriteTo(Stream stream)
        {
            _mapData.WriteTo(stream);
        }

        public byte[] GetData()
        {
            throw new NotImplementedException();
        }
    }
}
