// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using CommandLine;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.SimpleMapImage;

namespace mm
{
    /// <summary>
    /// Turns a metamap into an image
    /// </summary>
    class Program
    {
        sealed class Options
        {
            [Value(0, Required = true, HelpText = "Input path of metamap")]
            public string InputMapPath { get; set; }

            [Option(Default = ImagePalette.CarveOutRooms, HelpText = "Theme to use for the image.")]
            public ImagePalette Theme { get; set; }

            [Option("imagename", HelpText = "Name of the image file (by default, use the map name)")]
            public string ImageName { get; set; }

            [Option('s', "scale", Default = 1, Required = false, HelpText = "How much to scale the output image.")]
            public uint Scale { get; set; }
        }

        public enum ImagePalette
        {
            CarveOutRooms,
            HighlightWalls,
            Full
        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(RunOptionsAndReturnExitCode);
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            var map = MetaMap.Load(opts.InputMapPath);

            var imageName = opts.ImageName;
            if (!string.IsNullOrEmpty(imageName))
            {
                if (!imageName.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageName += ".png";
                }
            }
            else
            {
                imageName = Path.GetFileNameWithoutExtension(opts.InputMapPath) + ".png";
            }

            MapPalette PickPalette(ImagePalette palette)
            {
                switch (palette)
                {
                    case ImagePalette.HighlightWalls: return MapPalette.HighlightWalls;
                    case ImagePalette.Full: return MapPalette.Full;

                    case ImagePalette.CarveOutRooms:
                    default:
                        return MapPalette.CarveOutRooms;
                }
            }

            var imagePath = Path.Combine(Path.GetDirectoryName(opts.InputMapPath), imageName);
            SimpleMapImageExporter.Export(map, PickPalette(opts.Theme), imagePath, scale:opts.Scale);

            Console.WriteLine($"Wrote {imagePath} with color theme {opts.Theme} at scale {opts.Scale}x.");
        }
    }
}
