using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;

namespace Tiledriver
{
    public static class WadFile
    {
        // HACK: HERE BE DRAGONS!
        // This entire thing is an absolute abomination but it's good enough for now.
        public static void Save(Map map, string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
            {
                WriteArray(fileStream, Encoding.ASCII.GetBytes("PWAD"));
                WriteArray(fileStream, BitConverter.GetBytes(3));
                // Fill in the directory offset later
                fileStream.Position += 4;

                int firstEntryPosition = (int)fileStream.Position;

                // This is idiotic thanks to the StreamWriter disposing the underlying stream
                // TODO: Replace StreamWriter with something useable
                using (var tempStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(tempStream))
                {
                    map.Write(streamWriter);
                    streamWriter.Flush();
                    tempStream.Position = 0;
                    WriteArray(fileStream, tempStream.ToArray());
                }

                int directoryPosition = (int)fileStream.Position;

                // MAP01 marker
                WriteArray(fileStream, BitConverter.GetBytes(0));
                WriteArray(fileStream, BitConverter.GetBytes(0));
                WritePaddedArray(fileStream, Encoding.ASCII.GetBytes("MAP01"), 8);

                // TEXTMAP
                WriteArray(fileStream, BitConverter.GetBytes(firstEntryPosition));
                WriteArray(fileStream, BitConverter.GetBytes(directoryPosition - firstEntryPosition));
                WritePaddedArray(fileStream, Encoding.ASCII.GetBytes("TEXTMAP"), 8);

                // ENDMAP marker
                WriteArray(fileStream, BitConverter.GetBytes(0));
                WriteArray(fileStream, BitConverter.GetBytes(0));
                WritePaddedArray(fileStream, Encoding.ASCII.GetBytes("ENDMAP"), 8);

                // Go back and set the directory position
                fileStream.Position = firstEntryPosition - 4;
                WriteArray(fileStream, BitConverter.GetBytes(directoryPosition));
            }
        }

        private static void WriteArray(FileStream fileStream, byte[] bytes)
        {
            fileStream.Write(bytes, 0, bytes.Length);
        }

        private static void WritePaddedArray(FileStream fileStream, byte[] bytes, int length)
        {
            var result = new byte[length];
            Array.Copy(bytes, result, bytes.Length);
            WriteArray(fileStream, result);
        }
    }
}
