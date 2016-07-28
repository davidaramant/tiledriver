/*
** BaseUwmfBlock.cs
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
using System.Globalization;
using System.IO;
using System.Text;

namespace Tiledriver.Core.Uwmf
{
    public abstract class BaseUwmfBlock
    {
        protected static void WriteLine(Stream stream, string value)
        {
            var textBytes = Encoding.ASCII.GetBytes(value + "\n");

            stream.Write(textBytes, 0, textBytes.Length);
        }

        protected static void WritePropertyVerbatim(Stream stream, string name, string value, bool indent)
        {
            var indention = indent ? "\t" : String.Empty;
            WriteLine(stream, $"{indention}{name} = {value};");
        }

        protected static void WriteProperty(Stream stream, string name, string value, bool indent)
        {
            WritePropertyVerbatim(stream, name, $"\"{value}\"", indent);
        }

        protected static void WriteProperty(Stream stream, string name, int value, bool indent)
        {
            WritePropertyVerbatim(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
        }

        protected static void WriteProperty(Stream stream, string name, double value, bool indent)
        {
            WritePropertyVerbatim(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
        }

        protected static void WriteProperty(Stream stream, string name, bool value, bool indent)
        {
            WritePropertyVerbatim(stream, name, value.ToString().ToLowerInvariant(), indent);
        }

        protected static void WriteBlocks(Stream stream, IEnumerable<IWriteableUwmfBlock> blocks)
        {
            foreach (var block in blocks)
            {
                block.WriteTo(stream);
            }
        }
    }
}