// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using HsluvS;
using Tiledriver.Core.Extensions.Colors;
using Tiledriver.Core.Extensions.Enumerable;
using Tiledriver.Core.Utils;

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
            var paletteImage = new Bitmap(paletteFile);

            var hslPalette =
                paletteImage.Palette.Entries
                    .AsParallel()
                    .Select(c => HslColor.FromTuple(c.ToHsl()))
                    .ToArray();

            const double minLPercentage = 0.2;

            var translatedColors = hslPalette.Select(hsl => hsl with { L = hsl.L * minLPercentage }).ToArray();

            var indexTranslations = translatedColors.Select((color, index) => hslPalette.MinIndex(color.DistanceTo)).ToArray();


            // Create palette translations
            // Loop over all files
            // * Get relative path to inputDirectory
            // * Loop over all translations
            // ** Shift colors
            // ** Save to equivalent relative path in outputDirectory
        }

        private static double Lerp(double v0, double v1, double t) => (1 - t) * v0 + t * v1;
    }
}
