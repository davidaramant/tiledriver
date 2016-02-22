using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Zone : IUwmfEntry
    {
        public ZoneId Id { get; set; }

        public StreamWriter Write(StreamWriter writer)
        {
            return writer.
                Line("zone").
                Line("{").
                Line("}");
        }
    }
}
