using System.IO;

namespace Tiledriver.Uwmf
{
    public interface IUwmfEntry
    {
        StreamWriter Write(StreamWriter writer);
    }
}
