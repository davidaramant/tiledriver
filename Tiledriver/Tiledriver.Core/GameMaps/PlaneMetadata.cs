﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.GameMaps
{
    public struct PlaneMetadata
    {
        public readonly uint Offset;
        public readonly ushort CompressedLength;

        public PlaneMetadata(uint offset, ushort compressedLength)
        {
            Offset = offset;
            CompressedLength = compressedLength;
        }

        public override string ToString()
        {
            return $"Offset: {Offset:x8}, Length: {CompressedLength:x4} ({CompressedLength})";
        }
    }
}