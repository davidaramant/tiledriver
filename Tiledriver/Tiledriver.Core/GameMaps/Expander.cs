/*
** Expander.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2013, Braden Obrzut
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

namespace Tiledriver.Core.GameMaps
{
    /// <summary>
    /// Methods for decompressing RLEW encoding and Carmack compression.
    /// </summary>
    public static class Expander
    {
        public static byte[] DecompressRlew(ushort marker, byte[] input, int outputSize)
        {
            var result = new byte[outputSize];
            var resultPos = 0;
            int inputPos = 0;

            while (inputPos < input.Length)
            {
                var word = BitConverter.ToUInt16(input, inputPos);
                if (word == marker)
                {
                    inputPos += 2;
                    var repeatCount = BitConverter.ToUInt16(input, inputPos);
                    inputPos += 2;

                    for (int i = 0; i < repeatCount; i++)
                    {
                        result[resultPos++] = input[inputPos];
                        result[resultPos++] = input[inputPos + 1];
                    }
                }
                else
                {
                    result[resultPos++] = input[inputPos];
                    result[resultPos++] = input[inputPos + 1];
                }
                inputPos += 2;
            }

            return result;
        }

        /// <remarks>
        /// This method was transcribed from wolfmapcommon.cpp in the ECWolf source code.
        /// </remarks>
        public static byte[] DecompressCarmack(byte[] input)
        {
            const byte nearTag = 0xA7;
            const byte farTag = 0xA8;

            var outputLength = BitConverter.ToUInt16(input, 0);

            var outputPos = 0;
            var output = new byte[outputLength];

            var inputPos = 2;


            while (outputPos < outputLength)
            {
                var copyPos = 0;
                var length = input[inputPos++];

                if (length == 0 && (input[inputPos] == nearTag || input[inputPos] == farTag))
                {
                    output[outputPos++] = input[inputPos + 1];
                    output[outputPos++] = input[inputPos];
                    inputPos += 2;
                    continue;
                }
                else if (input[inputPos] == nearTag)
                {
                    copyPos = outputPos - (input[inputPos + 1] * 2);
                    inputPos += 2;
                }
                else if (input[inputPos] == farTag)
                {
                    copyPos = BitConverter.ToUInt16(input, inputPos + 1) * 2;
                    inputPos += 3;
                }
                else
                {
                    output[outputPos++] = length;
                    output[outputPos++] = input[inputPos++];
                    continue;
                }

                if ((outputPos + (length * 2)) > outputLength)
                    break;

                while (length-- > 0)
                {
                    output[outputPos++] = output[copyPos++];
                    output[outputPos++] = output[copyPos++];
                }
            }

            return output;
        }
    }
}
