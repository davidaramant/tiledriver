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

        private static StreamWriter InternalAttribute(this StreamWriter writer, string name, string value)
        {
            writer.WriteLine($"{name} = {value};");
            return writer;
        }

        public static StreamWriter Attribute(this StreamWriter writer, string name, string value)
        {
            return InternalAttribute(writer, name, $"\"{value}\"");
        }

        public static StreamWriter Attribute(this StreamWriter writer, string name, int value)
        {
            return InternalAttribute(writer, name, value.ToString(CultureInfo.InvariantCulture));
        }

        public static StreamWriter Attribute(this StreamWriter writer, string name, double value)
        {
            return InternalAttribute(writer, name, value.ToString(CultureInfo.InvariantCulture));
        } 

        public static StreamWriter Attribute(this StreamWriter writer, string name, bool value)
        {
            return InternalAttribute(writer, name, value.ToString().ToLowerInvariant());
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
