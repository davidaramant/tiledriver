// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Tiledriver.Core.Uwmf
{
    // TODO: Maybe all the Stream stuff should use tasks instead?
    // TODO: This should really live somewhere else.
    public static class StreamExtensions
    {
        public static void Line(this Stream stream, string value)
        {
            var textBytes = Encoding.ASCII.GetBytes(value + "\n");

            stream.Write(textBytes, 0, textBytes.Length);
        }

        private static void InternalAttribute(this Stream stream, string name, string value, bool indent)
        {
            var indention = indent ? "\t" : String.Empty;
            Line(stream, $"{indention}{name} = {value};");
        }

        public static void Attribute(this Stream stream, string name, string value, bool indent = true)
        {
            InternalAttribute(stream, name, $"\"{value}\"", indent);
        }

        public static void Attribute(this Stream stream, string name, int value, bool indent = true)
        {
            InternalAttribute(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
        }

        public static void Attribute(this Stream stream, string name, double value, bool indent = true)
        {
            InternalAttribute(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
        }

        public static void Attribute(this Stream stream, string name, bool value, bool indent = true)
        {
            InternalAttribute(stream, name, value.ToString().ToLowerInvariant(), indent);
        }

        public static void MaybeAttribute(this Stream stream, bool shouldWrite, string name, string value, bool indent = true)
        {
            if(shouldWrite )
            { 
                InternalAttribute(stream, name, $"\"{value}\"", indent);
            }
        }

        public static void MaybeAttribute(this Stream stream, bool shouldWrite, string name, int value, bool indent = true)
        {
            if (shouldWrite)
            {
                InternalAttribute(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
            }
        }

        public static void MaybeAttribute(this Stream stream, bool shouldWrite, string name, double value, bool indent = true)
        {
            if (shouldWrite)
            {
                InternalAttribute(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
            }
        }

        public static void MaybeAttribute(this Stream stream, bool shouldWrite, string name, bool value, bool indent = true)
        {
            if (shouldWrite)
            {
                InternalAttribute(stream, name, value.ToString().ToLowerInvariant(), indent);
            }
        }

        public static void Blocks(this Stream stream, IEnumerable<IUwmfEntry> blocks)
        {
            foreach (var block in blocks)
            {
                block.WriteTo(stream);
            }
        }

        public static void Blocks(this Stream stream, IEnumerable<TileSpace> tileSpaces)
        {
            stream.Line(String.Join(",\n", tileSpaces));
        }
    }
}
