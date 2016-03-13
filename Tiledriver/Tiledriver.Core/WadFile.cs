// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.IO;
using System.Text;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.Core
{
    public static class WadFile
    {
        // HACK: This is very simplistic but good enough for now
        public static void Save(Map map, string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
            {
                WriteArray(fileStream, Encoding.ASCII.GetBytes("PWAD"));
                WriteArray(fileStream, BitConverter.GetBytes(3));
                // Fill in the directory offset later  
                fileStream.Position += 4;

                int firstEntryPosition = (int)fileStream.Position;

                map.WriteTo(fileStream);

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
