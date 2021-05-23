using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.Core.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.Core.FormatModels.Wad
{
    public static class WadWriter
    {
        public static void SaveTo(IEnumerable<ILump> lumps, string filePath)
        {
            using var fs = File.Open(filePath, FileMode.Create);
            WriteTo(lumps, fs);
        }

        public static void WriteTo(IEnumerable<ILump> lumps, Stream stream)
        {
            var lumpList = lumps.ToList();

            stream.WriteText("PWAD");
            stream.WriteInt(lumpList.Count);

            // Fill in this position after writing the data
            int directoryOffsetPosition = (int)stream.Position;
            stream.Position += 4;

            var metadata = new List<LumpMetadata>();
            foreach (var lump in lumpList)
            {
                int startOfLump = (int)stream.Position;

                lump.WriteTo(stream);

                metadata.Add(new LumpMetadata(
                    Position: startOfLump,
                    Size: (int)stream.Position - startOfLump,
                    Name: lump.Name));
            }

            int startOfDirectory = (int)stream.Position;

            // Write directory
            foreach (var lumpMetadata in metadata)
            {
                lumpMetadata.WriteTo(stream);
            }

            // Go back and set the directory position
            stream.Position = directoryOffsetPosition;
            stream.WriteInt(startOfDirectory);
        }
    }
}