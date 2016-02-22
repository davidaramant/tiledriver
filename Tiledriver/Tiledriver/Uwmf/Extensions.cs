using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Tiledriver.Uwmf
{
    // TODO: Maybe all the StreamWriter stuff should use tasks instead
    public static class Extensions
    {
        public static StreamWriter Line(this StreamWriter writer, string value)
        {
            writer.WriteLine(value);
            return writer;
        }

        private static StreamWriter InternalAttribute(this StreamWriter writer, string name, string value, bool indent)
        {
            var indention = indent ? "\t" : String.Empty;
            writer.WriteLine($"{indention}{name} = {value};");
            return writer;
        }

        public static StreamWriter Attribute(this StreamWriter writer, string name, string value, bool indent = true)
        {
            return InternalAttribute(writer, name, $"\"{value}\"", indent);
        }

        public static StreamWriter Attribute(this StreamWriter writer, string name, int value, bool indent = true)
        {
            return InternalAttribute(writer, name, value.ToString(CultureInfo.InvariantCulture), indent);
        }

        public static StreamWriter Attribute(this StreamWriter writer, string name, double value, bool indent = true)
        {
            return InternalAttribute(writer, name, value.ToString(CultureInfo.InvariantCulture), indent);
        } 

        public static StreamWriter Attribute(this StreamWriter writer, string name, bool value, bool indent = true)
        {
            return InternalAttribute(writer, name, value.ToString().ToLowerInvariant(), indent);
        }

        public static StreamWriter Blocks(this StreamWriter writer, IEnumerable<IUwmfEntry> blocks)
        {
            foreach (var block in blocks)
            {
                block.Write(writer);
            }
            return writer;
        }

        public static StreamWriter Block(this StreamWriter writer, IUwmfEntry block)
        {
            block.Write(writer);
            return writer;
        }
    }
}
