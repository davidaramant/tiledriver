// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;

namespace CreateLightingImages
{
    class Program
    {
        static void Main(
            string paletteFile,
            string inputDirectory,
            string outputDirectory,
            int darkLevels,
            int overbrightLevels)
        {
            if (!File.Exists(paletteFile))
            {
                throw new ArgumentException($"Could not find file {paletteFile}");
            }

            if (!Directory.Exists(inputDirectory))
            {
                throw new ArgumentException($"Could not find directory {inputDirectory}");
            }

            // Load palette
            // Create palette translations
            // Loop over all files
            // * Get relative path to inputDirectory
            // * Loop over all translations
            // ** Shift colors
            // ** Save to equivalent relative path in outputDirectory
        }
    }
}
