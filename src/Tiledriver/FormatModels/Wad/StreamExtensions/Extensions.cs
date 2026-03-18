using System.Buffers;
using System.Buffers.Binary;
using System.Text;

namespace Tiledriver.FormatModels.Wad.StreamExtensions;

public static class Extensions
{
	private const int StackAllocThreshold = 256;
	private static readonly byte[] ZeroPaddingBuffer = new byte[256];

	public static void WriteInt(this Stream stream, int value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(int)];
		BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
		stream.Write(buffer);
	}

	public static void WriteText(this Stream stream, string text)
	{
		stream.WriteText(text, text.Length);
	}

	public static void WriteText(this Stream stream, string text, int totalLength)
	{
		if (totalLength < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(totalLength));
		}

		var byteCount = Encoding.ASCII.GetByteCount(text);
		if (byteCount > totalLength)
		{
			throw new ArgumentException(
				"totalLength cannot be smaller than the encoded text length.",
				nameof(totalLength)
			);
		}

		if (byteCount <= StackAllocThreshold)
		{
			Span<byte> buffer = stackalloc byte[byteCount];
			var written = Encoding.ASCII.GetBytes(text.AsSpan(), buffer);
			stream.Write(buffer[..written]);
		}
		else
		{
			var rented = ArrayPool<byte>.Shared.Rent(byteCount);
			try
			{
				var written = Encoding.ASCII.GetBytes(text.AsSpan(), rented);
				stream.Write(rented.AsSpan(0, written));
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(rented);
			}
		}

		WritePadding(stream, totalLength - byteCount);
	}

	public static void WriteArray(this Stream stream, byte[] bytes) => stream.WriteArray(bytes, bytes.Length);

	public static void WriteArray(this Stream stream, byte[] bytes, int totalLength)
	{
		if (totalLength < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(totalLength));
		}

		if (bytes.Length > totalLength)
		{
			throw new ArgumentException(
				"totalLength cannot be smaller than the byte array length.",
				nameof(totalLength)
			);
		}

		stream.Write(bytes);
		WritePadding(stream, totalLength - bytes.Length);
	}

	public static string ReadText(this Stream stream, int length)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(length));
		}

		if (length == 0)
		{
			return string.Empty;
		}

		if (length <= StackAllocThreshold)
		{
			Span<byte> buffer = stackalloc byte[length];
			stream.ReadExactly(buffer);
			return Encoding.ASCII.GetString(buffer);
		}

		var rented = ArrayPool<byte>.Shared.Rent(length);
		try
		{
			var buffer = rented.AsSpan(0, length);
			stream.ReadExactly(buffer);
			return Encoding.ASCII.GetString(buffer);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(rented);
		}
	}

	public static int ReadInt(this Stream stream)
	{
		Span<byte> buffer = stackalloc byte[sizeof(int)];
		stream.ReadExactly(buffer);
		return BinaryPrimitives.ReadInt32LittleEndian(buffer);
	}

	public static byte[] ReadArray(this Stream stream, int length)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(length));
		}

		var data = new byte[length];
		stream.ReadExactly(data);
		return data;
	}

	private static void WritePadding(Stream stream, int padding)
	{
		if (padding <= 0)
		{
			return;
		}

		while (padding > 0)
		{
			var chunkSize = Math.Min(padding, ZeroPaddingBuffer.Length);
			stream.Write(ZeroPaddingBuffer.AsSpan(0, chunkSize));
			padding -= chunkSize;
		}
	}
}
