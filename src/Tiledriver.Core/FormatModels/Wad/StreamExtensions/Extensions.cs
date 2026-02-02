// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Text;

namespace Tiledriver.Core.FormatModels.Wad.StreamExtensions;

public static class Extensions
{
	public static void WriteInt(this Stream stream, int value) => stream.WriteArray(BitConverter.GetBytes(value));

	public static void WriteText(this Stream stream, string text) => stream.WriteText(text, text.Length);

	public static void WriteText(this Stream stream, string text, int totalLength) =>
		stream.WriteArray(Encoding.ASCII.GetBytes(text), totalLength);

	public static void WriteArray(this Stream stream, byte[] bytes) => stream.WriteArray(bytes, bytes.Length);

	public static void WriteArray(this Stream stream, byte[] bytes, int totalLength)
	{
		stream.Write(bytes, 0, bytes.Length);
		var padding = totalLength - bytes.Length;
		if (padding != 0)
		{
			stream.Write(new byte[padding], 0, padding);
		}
	}

	public static string ReadText(this Stream stream, int length) => Encoding.ASCII.GetString(stream.ReadArray(length));

	public static int ReadInt(this Stream stream) => BitConverter.ToInt32(stream.ReadArray(4), 0);

	public static byte[] ReadArray(this Stream stream, int length)
	{
		var data = new byte[length];
		stream.ReadExactly(data, 0, length);
		return data;
	}
}
