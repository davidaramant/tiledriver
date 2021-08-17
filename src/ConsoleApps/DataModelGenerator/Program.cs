// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using Tiledriver.DataModelGenerator.MapInfo;
using Tiledriver.DataModelGenerator.Udmf;
using Tiledriver.DataModelGenerator.Uwmf;
using Tiledriver.DataModelGenerator.Xlat;

namespace Tiledriver.DataModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = FindSolutionPath();

            var corePath = Path.Combine(basePath, "Tiledriver.Core");
            var formatModelsPath = Path.Combine(corePath, "FormatModels");
            
            var udmfPath = Path.Combine(formatModelsPath, "Udmf");
            var udmfWritingPath = Path.Combine(udmfPath, "Writing");
            var udmfReadingPath = Path.Combine(udmfPath, "Reading");

            var uwmfPath = Path.Combine(formatModelsPath, "Uwmf");
            var uwmfWritingPath = Path.Combine(uwmfPath, "Writing");
            var uwmfReadingPath = Path.Combine(uwmfPath, "Reading");
            
            var xlatPath = Path.Combine(formatModelsPath, "Xlat");
            var xlatReadingPath = Path.Combine(xlatPath, "Reading");

            var mapInfoPath = Path.Combine(formatModelsPath, "MapInfo");
            var mapInfoReadingPath = Path.Combine(mapInfoPath, "Reading");

            UwmfModelGenerator.WriteToPath(uwmfPath);
            UwmfWriterGenerator.WriteToPath(uwmfWritingPath);
            UwmfSemanticAnalyzerGenerator.WriteToPath(uwmfReadingPath);

            UdmfModelGenerator.WriteToPath(udmfPath);
            UdmfWriterGenerator.WriteToPath(udmfWritingPath);
            //UdmfSemanticAnalyzerGenerator.WriteToPath(udmfReadingPath);

            XlatModelGenerator.WriteToPath(xlatPath);
            XlatParserGenerator.WriteToPath(xlatReadingPath);

            MapInfoModelGenerator.WriteToPath(mapInfoPath);
            MapInfoParserGenerator.WriteToPath(mapInfoReadingPath);
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
