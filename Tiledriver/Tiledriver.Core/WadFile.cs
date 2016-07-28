/*
** WadFile.cs
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
using System.IO;
using System.Text;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.Core
{
    public static class WadFile
    {
        // HACK: This is very simplistic but good enough for now
        public static void Save(Map map, string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
            {
                WriteArray(fileStream, Encoding.ASCII.GetBytes("PWAD"));
                WriteArray(fileStream, BitConverter.GetBytes(3));
                // Fill in the directory offset later  
                fileStream.Position += 4;

                int firstEntryPosition = (int)fileStream.Position;

                map.WriteTo(fileStream);

                int directoryPosition = (int)fileStream.Position;

                // MAP01 marker
                WriteArray(fileStream, BitConverter.GetBytes(0));
                WriteArray(fileStream, BitConverter.GetBytes(0));
                WritePaddedArray(fileStream, Encoding.ASCII.GetBytes("MAP01"), 8);

                // TEXTMAP
                WriteArray(fileStream, BitConverter.GetBytes(firstEntryPosition));
                WriteArray(fileStream, BitConverter.GetBytes(directoryPosition - firstEntryPosition));
                WritePaddedArray(fileStream, Encoding.ASCII.GetBytes("TEXTMAP"), 8);

                // ENDMAP marker
                WriteArray(fileStream, BitConverter.GetBytes(0));
                WriteArray(fileStream, BitConverter.GetBytes(0));
                WritePaddedArray(fileStream, Encoding.ASCII.GetBytes("ENDMAP"), 8);

                // Go back and set the directory position
                fileStream.Position = firstEntryPosition - 4;
                WriteArray(fileStream, BitConverter.GetBytes(directoryPosition));
            }
        }

        private static void WriteArray(FileStream fileStream, byte[] bytes)
        {
            fileStream.Write(bytes, 0, bytes.Length);
        }

        private static void WritePaddedArray(FileStream fileStream, byte[] bytes, int length)
        {
            var result = new byte[length];
            Array.Copy(bytes, result, bytes.Length);
            WriteArray(fileStream, result);
        }
    }
}
