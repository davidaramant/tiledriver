using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Zone : IUwmfEntry
    {
        public Stream WriteTo(Stream stream)
        {
            return stream.
                Line("zone").
                Line("{").
                Line("}");
        }
    }
}
