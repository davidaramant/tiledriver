using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Tiledriver.Core.GameMaps;

namespace Tiledriver.GameMapsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = @"C:\Program Files (x86)\Steam\steamapps\common\Wolfenstein 3D\base";
            var mapHeadPath = Path.Combine(basePath, "MAPHEAD.WL6");
            var gameMapsPath = Path.Combine(basePath, "GAMEMAPS.WL6");

            var mapHeadFileSize = (int)(new FileInfo(mapHeadPath).Length);
            var gameMapsFileSize = (int)(new FileInfo(gameMapsPath).Length);

            OffsetData offsetData;
            using (var headerStream = File.OpenRead(mapHeadPath))
            {
                offsetData = OffsetData.ReadOffsets(headerStream);
            }

            using (var stream = File.Open(gameMapsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var headerBuffer = new byte[42];
                var headers = offsetData.Offsets.Select(offset =>
                {
                    stream.Position = offset;
                    stream.Read(headerBuffer, 0, headerBuffer.Length);
                    return MapHeader.Parse(headerBuffer);
                }).ToArray();

                using (var output = File.Open("output.txt", FileMode.Create))
                using (var streamWriter = new StreamWriter(output))
                {
                    var totalSavedBytes = 0;

                    foreach (var header in headers)
                    {
                        streamWriter.WriteLine(header.Name);
                        streamWriter.WriteLine($"Size: {header.Width}x{header.Height}");

                        foreach (var indexedPlaneInfo in new[] { header.Plane0Info, header.Plane1Info, header.Plane2Info }.Select(((metadata, i) => new { metadata, i })))
                        {
                            var planeInfo = indexedPlaneInfo.metadata;
                            var index = indexedPlaneInfo.i;

                            streamWriter.WriteLine($"Plane {index}: {planeInfo}");

                            var buffer = new byte[planeInfo.CompressedLength];
                            stream.Position = planeInfo.Offset;
                            stream.Read(buffer, 0, buffer.Length);

                            var uncarmacked = Expander.DecompressCarmack(buffer);

                            streamWriter.WriteLine($"UnCarmacked Size: {uncarmacked.Length} bytes");

                            var finalSize = BitConverter.ToUInt16(uncarmacked, 0);
                            var rlewData = new byte[uncarmacked.Length - 2];
                            Buffer.BlockCopy(uncarmacked, 2, rlewData, 0, rlewData.Length);

                            var final = Expander.DecompressRlew(offsetData.RlewMarker, rlewData, finalSize);

                            streamWriter.WriteLine($"Uncompressed {final.Length} bytes");
                            var savedSpace = final.Length - planeInfo.CompressedLength;
                            streamWriter.WriteLine($"Saved {savedSpace} bytes!");

                            totalSavedBytes += savedSpace;
                        }
                        streamWriter.WriteLine(new string('-', 80));
                    }

                    var compressedSize = mapHeadFileSize + gameMapsFileSize;
                    var uncompressedSize = compressedSize + totalSavedBytes;

                    streamWriter.WriteLine($"### Total saved space: {totalSavedBytes} bytes ({totalSavedBytes.Bytes().Humanize("MB")})");
                    streamWriter.WriteLine($"### Total Compressed Size: {compressedSize.Bytes().Humanize("MB")}");
                    streamWriter.WriteLine($"### Total Uncompressed Size: {uncompressedSize.Bytes().Humanize("MB")}");
                    streamWriter.WriteLine($"### Compressed size: {(double)compressedSize/uncompressedSize:p}");
                }
            }
        }

        static void WL(object o)
        {
            Console.Out.WriteLine(o);
        }
    }
}
