// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using Tiledriver.DataModelGenerator.Uwmf;

namespace Tiledriver.DataModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = FindSolutionPath();

            var corePath = Path.Combine(basePath, "Tiledriver.Core2"); // HACK: Point to Core 2 for now
            var formatModelsPath = Path.Combine(corePath, "FormatModels");
            var uwmfPath = Path.Combine(formatModelsPath, "Uwmf");
            var uwmfWritingPath = Path.Combine(uwmfPath, "Writing");
            var uwmfReadingPath = Path.Combine(uwmfPath, "Reading");

            UwmfModelGenerator.WriteToPath(uwmfPath);
            UwmfModelGenerator.WriteToPath(uwmfWritingPath);
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
