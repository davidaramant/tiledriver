using System.IO;

namespace Tiledriver.Uwmf
{
    public interface IUwmfEntry
    {
        Stream WriteTo(Stream stream);
    }
}
