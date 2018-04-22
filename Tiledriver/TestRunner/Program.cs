using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ShellProgressBar;
using Tiledriver;

namespace TestRunner
{
    /// <summary>
    /// Convenience program to directly launch the output in ECWolf from Visual Studio.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            CreateRandomMaps(34257, @"C:\Users\david\Desktop\Wolf3D Maps\TiledriverV1Maps");
            Console.Out.WriteLine("Done!");
            Console.In.ReadLine();
            return;

            const string inputFile = "ECWolfPath.txt";

            if (!File.Exists(inputFile))
            {
                throw new Exception(
                    $"Could not find {inputFile}.  " +
                    "Create this file in the output directory containing a single line with the full path to ECWolf.exe.  " +
                    "Do not quote the path.");
            } 

            var ecWolfPath = File.ReadAllLines(inputFile).Single();

            var wadFileName = Path.GetFullPath("demo.wad" );

            WadFile.Save(
                DemoMap.CreateRandomWithSparseMap(0),
                wadFileName);

            Process.Start(
                ecWolfPath, 
                $"--file \"{wadFileName}\" --normal --nowait --tedlevel map01");
        }

        private static void CreateRandomMaps(int count, string outputPath)
        {
            int maxDigitWidth = count.ToString().Length;
            var indexFormat = "D" + maxDigitWidth;

            using (var progress = new ProgressBar(count, "Creating maps..."))
            {
                Parallel.For(0, count, index =>
                {
                    var map = DemoMap.CreateRandomWithSparseMap(index);
                    using (var fs = new FileStream(Path.Combine(outputPath, $"Seed{index.ToString(indexFormat)}.uwmf"), FileMode.Create))
                    {
                        map.WriteTo(fs);
                    }

                    progress.Tick();
                });
            }
        }
    }
}
