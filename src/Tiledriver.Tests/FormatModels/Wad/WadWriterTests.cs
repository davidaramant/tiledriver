using System.Text;
using Tiledriver.DemoMaps.Wolf3D;
using Tiledriver.FormatModels.Wad;
using Tiledriver.FormatModels.Wad.StreamExtensions;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Wad;

public sealed class WadWriterTests
{
	[Fact]
	public void ShouldCreateWadFile()
	{
		var fileInfo = new FileInfo(Path.GetTempFileName());
		try
		{
			var lumps = new List<ILump>
			{
				new Marker("MAP01"),
				new UwmfLump("TEXTMAP", ThingDemoMap.Create()),
				new Marker("ENDMAP"),
			};
			WadWriter.SaveTo(lumps, fileInfo.FullName);

			using var wad = WadReader.Read(fileInfo.FullName);
			Assert.Equal(WadType.Pwad, wad.Type);
			Assert.Equal(3, wad.Count);
		}
		finally
		{
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
		}
	}

	[Fact]
	public void ShouldWriteRequestedIwadTypeWhenSavingToFile()
	{
		var fileInfo = new FileInfo(Path.GetTempFileName());
		try
		{
			WadWriter.SaveTo([new Marker("START")], fileInfo.FullName, WadType.Iwad);

			using var wad = WadReader.Read(fileInfo.FullName);
			Assert.Equal(WadType.Iwad, wad.Type);
		}
		finally
		{
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
		}
	}

	[Fact]
	public void ShouldWriteRequestedIwadTypeWhenWritingToStream()
	{
		using var stream = new MemoryStream();

		WadWriter.WriteTo([new Marker("START")], stream, WadType.Iwad);

		stream.Position = 0;
		Assert.Equal("IWAD", stream.ReadText(4));
	}

	[Fact]
	public void ShouldWriteExpectedDirectoryMetadata()
	{
		using var stream = new MemoryStream();

		WadWriter.WriteTo([new DataLump("DATA", new byte[] { 1, 2, 3 }), new Marker("END")], stream);

		stream.Position = 0;
		Assert.Equal("PWAD", stream.ReadText(4));
		Assert.Equal(2, stream.ReadInt());
		Assert.Equal(15, stream.ReadInt());
		Assert.Equal(new byte[] { 1, 2, 3 }, ReadBytes(stream, 3));
		Assert.Equal(12, stream.ReadInt());
		Assert.Equal(3, stream.ReadInt());
		Assert.Equal("DATA", Encoding.ASCII.GetString(ReadBytes(stream, LumpName.MaxLength)).TrimEnd('\0'));
		Assert.Equal(15, stream.ReadInt());
		Assert.Equal(0, stream.ReadInt());
		Assert.Equal("END", Encoding.ASCII.GetString(ReadBytes(stream, LumpName.MaxLength)).TrimEnd('\0'));
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	public void ShouldRejectBlankFilePath(string filePath)
	{
		Assert.Throws<ArgumentException>(() => WadWriter.SaveTo([], filePath));
	}

	[Fact]
	public void ShouldRejectNonWritableStream()
	{
		using var stream = new NonWritableMemoryStream();

		Assert.Throws<ArgumentException>(() => WadWriter.WriteTo([], stream));
	}

	[Fact]
	public void ShouldRejectNonSeekableStream()
	{
		using var stream = new NonSeekableMemoryStream();

		Assert.Throws<ArgumentException>(() => WadWriter.WriteTo([], stream));
	}

	private sealed class NonWritableMemoryStream : MemoryStream
	{
		public override bool CanWrite => false;

		public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
	}

	private sealed class NonSeekableMemoryStream : MemoryStream
	{
		public override bool CanSeek => false;

		public override long Seek(long offset, SeekOrigin loc) => throw new NotSupportedException();

		public override long Position
		{
			get => throw new NotSupportedException();
			set => throw new NotSupportedException();
		}
	}

	private static byte[] ReadBytes(Stream stream, int length)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(length));
		}

		var data = new byte[length];
		stream.ReadExactly(data);
		return data;
	}
}
