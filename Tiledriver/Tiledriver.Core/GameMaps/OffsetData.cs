/*
** OffsetData.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tiledriver.Core.GameMaps
{
    public sealed class OffsetData
    {
        public const int MaxLevelOffsets = 100;

        public ushort RlewMarker { get; }
        public IReadOnlyList<uint> Offsets { get; }

        public OffsetData(ushort rlewMarker, IEnumerable<uint> offsets)
        {
            RlewMarker = rlewMarker;
            Offsets = offsets.TakeWhile(offset => offset != 0 && offset != uint.MaxValue).ToList();
        }

        public static OffsetData ReadOffsets(Stream stream)
        {
            var buffer = new byte[2 + 4 * MaxLevelOffsets];
            stream.Read(buffer, 0, buffer.Length);
            var rlewMarker = BitConverter.ToUInt16(buffer, 0);

            var offsets = new List<uint>();

            for (int i = 0; i < MaxLevelOffsets; i++)
            {
                offsets.Add(BitConverter.ToUInt32(buffer, 2 + 4 * i));
            }

            return new OffsetData(rlewMarker: rlewMarker, offsets: offsets);
        }
    }
}