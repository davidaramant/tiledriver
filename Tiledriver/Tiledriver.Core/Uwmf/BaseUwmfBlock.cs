// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

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