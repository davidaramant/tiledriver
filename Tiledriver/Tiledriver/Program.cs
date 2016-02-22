using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;

namespace Tiledriver
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var fs = File.OpenWrite("test.uwmf"))
            using( var sw = new StreamWriter(fs))
            {
                var map = new Map
                {
                    Name = "Test Output",
                    Width = 64,
                    Height = 64,
                    TileSize = 64,
                };
                map.Write(sw);
            }
        }
    }
}
