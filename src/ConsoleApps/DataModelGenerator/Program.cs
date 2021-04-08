using System;
using System.IO;

namespace Tiledriver.DataModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"SLN path: {FindSolutionPath()}");
        }

        private static string FindSolutionPath()
        {
            var path = "..";

            while (!File.Exists(Path.Combine(path, "Tiledriver.sln")))
            {
                path = Path.Combine(path, "..");
            }

            return Path.GetFullPath(path);
        }
    }
}
