using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Plane : IUwmfEntry
    {
        public int Depth { get; set; }

        public StreamWriter Write(StreamWriter writer)
        {
            return writer.
                Line("plane").
                Line("{").
                Attribute("depth", Depth).
                Line("}");
        }
    }
}
