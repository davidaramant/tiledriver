using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Zone : IUwmfEntry
    {
        public StreamWriter Write(StreamWriter writer)
        {
            return writer.
                Line("zone").
                Line("{").
                Line("}");
        }
    }
}
