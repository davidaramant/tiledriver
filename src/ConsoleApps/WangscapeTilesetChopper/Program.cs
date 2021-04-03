// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using WangscapeTilesetChopper.Model;

namespace WangscapeTilesetChopper
{
    class Program
    {
        static void Main(string inputDirectory, string outputDirectory)
        {
            if (!Directory.Exists(inputDirectory))
            {
                throw new ArgumentException($"Could not find {inputDirectory}");
            }

            var tilesJsonFile = Path.Combine(inputDirectory, "tiles.json");
            var tileSheetFile = Directory.EnumerateFiles(inputDirectory, "*.png").Single();

            // TODO: Is there seriously no better way to read from a file?
            var serializedDefinitions = File.ReadAllText(tilesJsonFile);
            
            var definitions = JsonSerializer.Deserialize<List<TileDefinition>>(
                serializedDefinitions, 
                new JsonSerializerOptions{PropertyNameCaseInsensitive = true}) ?? throw new ArgumentException("Bad format for tiles.json");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using var tileSheet = new Bitmap(tileSheetFile);

            foreach (var definition in definitions)
            {
                using var tile = tileSheet.Clone(definition.GetCropArea(), tileSheet.PixelFormat);
                tile.Save(Path.Combine(outputDirectory, definition.GetFileName()));
            }
        }
    }
}
