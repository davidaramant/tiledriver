using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Tiledriver.Uwmf
{
    // TODO: Maybe all the Stream stuff should use tasks instead?
    public static class StreamExtensions
    {
        public static Stream Line(this Stream stream, string value)
        {
            var textBytes = Encoding.ASCII.GetBytes(value + "\n");

            stream.Write(textBytes, 0, textBytes.Length);
            return stream;
        }

        private static Stream InternalAttribute(this Stream stream, string name, string value, bool indent)
        {
            var indention = indent ? "\t" : String.Empty;
            return Line(stream, $"{indention}{name} = {value};");
        }

        public static Stream Attribute(this Stream stream, string name, string value, bool indent = true)
        {
            return InternalAttribute(stream, name, $"\"{value}\"", indent);
        }

        public static Stream Attribute(this Stream stream, string name, int value, bool indent = true)
        {
            return InternalAttribute(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
        }

        public static Stream Attribute(this Stream stream, string name, double value, bool indent = true)
        {
            return InternalAttribute(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
        }

        public static Stream Attribute(this Stream stream, string name, bool value, bool indent = true)
        {
            return InternalAttribute(stream, name, value.ToString().ToLowerInvariant(), indent);
        }

        public static Stream Blocks(this Stream stream, IEnumerable<IUwmfEntry> blocks)
        {
            foreach (var block in blocks)
            {
                block.WriteTo(stream);
            }
            return stream;
        }

        public static Stream Block(this Stream stream, IUwmfEntry block)
        {
            block.WriteTo(stream);
            return stream;
        }
    }
}
