using System.IO;
using Tiledriver.Uwmf;

namespace Tiledriver
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var stream = File.OpenWrite("demo.uwmf"))
            {
                DemoMap.CreateWithSparseMap().WriteTo(stream);
            }
        }
    }
}
