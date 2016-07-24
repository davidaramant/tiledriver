// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;

namespace Tiledriver.Core.GameMaps
{
    /// <summary>
    /// Methods for decompressing RLEW encoding and Carmack compression.
    /// </summary>
    public static class Expander
    {
        public static byte[] DecompressRlew(ushort marker, byte[] input)
        {
            var result = new List<byte>(input.Length);
            int inputPosition = 0;

            while (inputPosition < input.Length)
            {
                var word = BitConverter.ToUInt16(input, inputPosition);
                if (word == marker)
                {
                    inputPosition += 2;
                    var repeatCount = BitConverter.ToUInt16(input, inputPosition);
                    inputPosition += 2;

                    for (int i = 0; i < repeatCount; i++)
                    {
                        result.Add(input[inputPosition]);
                        result.Add(input[inputPosition + 1]);
                    }
                }
                else
                {
                    result.Add(input[inputPosition]);
                    result.Add(input[inputPosition + 1]);
                }
                inputPosition += 2;
            }

            return result.ToArray();
        }
    }
}
