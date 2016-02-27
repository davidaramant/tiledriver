using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                DemoMap.CreateWithRandomRegions(),
                wadFileName);

            Process.Start(
                ecWolfPath, 
                $"--file \"{wadFileName}\" --normal --nowait --tedlevel map01");
        }
    }
}
