// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.MapMetadata.Writing;

namespace mm
{
    /// <summary>
    /// Turns a metamap into an image
    /// </summary>
    class Program
    {
        record Options(string InputMapPath, ImagePalette Theme, string ImageName, uint Scale);

        public enum ImagePalette
        {
            CarveOutRooms,
            HighlightWalls,
            Full
        }

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="inputMapPath">Input path of metamap</param>
        /// <param name="theme">Theme to use for the image.</param>
        /// <param name="imageName">Name of the image file (by default, use the map name)</param>
        /// <param name="scale">How much to scale the output image.</param>
        static void Main(
            string inputMapPath,
            ImagePalette theme,
            string imageName = null,
            uint scale = 1)
        {
            RunOptionsAndReturnExitCode(new Options(inputMapPath, theme, imageName, scale));
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
            MetaMapImageExporter.Export(map, PickPalette(opts.Theme), imagePath, scale: opts.Scale);

            Console.WriteLine($"Wrote {imagePath} with color theme {opts.Theme} at scale {opts.Scale}x.");
        }
    }
}
