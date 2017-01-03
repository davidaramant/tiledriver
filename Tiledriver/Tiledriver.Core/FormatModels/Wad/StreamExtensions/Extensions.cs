// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Text;

namespace Tiledriver.Core.FormatModels.Wad.StreamExtensions
{
    public static class Extensions
    {
        public static void WriteInt(this Stream stream, int value)
        {
            stream.WriteArray(BitConverter.GetBytes(value));
        }

        public static void WriteText(this Stream stream, string text)
        {
            WriteText(stream, text, text.Length);
        }

        public static void WriteText(this Stream stream, string text, int totalLength)
        {
            WriteArray(stream, Encoding.ASCII.GetBytes(text), totalLength);
        }

        public static void WriteArray(this Stream stream, byte[] bytes)
        {
            WriteArray(stream, bytes, bytes.Length);
        }

        public static void WriteArray(this Stream stream, byte[] bytes, int totalLength)
        {
            stream.Write(bytes, 0, bytes.Length);
            var padding = totalLength - bytes.Length;
            if (padding != 0)
            {
                stream.Write(new byte[padding], 0, padding);
            }
        }
    }
}
