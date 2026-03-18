using Shouldly;
using Tiledriver.DemoMaps.Wolf3D;
using Tiledriver.FormatModels.Uwmf.Reading;
using Tiledriver.FormatModels.Wad;
using Tiledriver.FormatModels.Wad.StreamExtensions;
using Tiledriver.Tests.FormatModels.Uwmf.Reading;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Wad;

public sealed class WadReaderTests
{
	[Fact]
	public void ShouldReadCreatedWadFile()
	{
		var fileInfo = new FileInfo(Path.GetTempFileName());
		try
		{
			var map = ThingDemoMap.Create();

			var lumps = new List<ILump> { new Marker("MAP01"), new UwmfLump("TEXTMAP", map), new Marker("ENDMAP") };
			WadWriter.SaveTo(lumps, fileInfo.FullName);

			using var wad = WadReader.Read(fileInfo.FullName);
			wad.Count.ShouldBe(3);

			wad.Select(l => l.Name)
				.ShouldBe(
					[new LumpName("MAP01"), new LumpName("TEXTMAP"), new LumpName("ENDMAP")],
					"correct lump names should have been read."
				);

			using var mapStream = wad[1].GetData();
			var roundTripped = UwmfReader.Read(mapStream);

			UwmfComparison.AssertEqual(roundTripped, map);
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
	public void ShouldReturnEmptyBytesForMarkerLump()
	{
		using var stream = BuildWad("PWAD", 12, [(0, 0, Name("MARKER"))], []);
		using var wad = WadReader.Read(stream, leaveOpen: true);

		wad[0].HasData.ShouldBeFalse();
		ReadAllBytes(wad[0].GetData()).ShouldBeEmpty();
	}

	[Fact]
	public void ShouldSupportAliasedOffsets()
	{
		using var stream = BuildWad("PWAD", 12, [(44, 4, Name("ONE")), (44, 4, Name("TWO"))], [1, 2, 3, 4]);
		using var wad = WadReader.Read(stream, leaveOpen: true);

		ReadAllBytes(wad[0].GetData()).ShouldBe([1, 2, 3, 4]);
		ReadAllBytes(wad[1].GetData()).ShouldBe([1, 2, 3, 4]);
	}

	[Fact]
	public void ShouldSupportOverlappingOffsets()
	{
		using var stream = BuildWad("PWAD", 12, [(44, 4, Name("ONE")), (46, 4, Name("TWO"))], [1, 2, 3, 4, 5, 6]);
		using var wad = WadReader.Read(stream, leaveOpen: true);

		ReadAllBytes(wad[0].GetData()).ShouldBe([1, 2, 3, 4]);
		ReadAllBytes(wad[1].GetData()).ShouldBe([3, 4, 5, 6]);
	}

	[Fact]
	public void ShouldReadDirectoryThatAppearsBeforeLumpData()
	{
		using var stream = new MemoryStream();
		stream.WriteText("PWAD");
		stream.WriteInt(1);
		stream.WriteInt(12);
		stream.WriteInt(28);
		stream.WriteInt(4);
		stream.WriteText("Data", totalLength: LumpName.MaxLength);
		stream.WriteArray([9, 8, 7, 6]);
		stream.Position = 0;

		using var wad = WadReader.Read(stream, leaveOpen: true);

		wad[0].Name.ShouldBe(new LumpName("Data"));
		ReadAllBytes(wad[0].GetData()).ShouldBe([9, 8, 7, 6]);
	}

	[Fact]
	public void ShouldAllowTrailingBytesAfterDirectory()
	{
		using var stream = new MemoryStream();
		stream.WriteText("PWAD");
		stream.WriteInt(1);
		stream.WriteInt(16);
		stream.WriteArray([5, 4, 3, 2]);
		stream.WriteInt(12);
		stream.WriteInt(4);
		stream.WriteText("TAIL", totalLength: LumpName.MaxLength);
		stream.WriteArray([99, 98, 97]);
		stream.Position = 0;

		using var wad = WadReader.Read(stream, leaveOpen: true);

		ReadAllBytes(wad[0].GetData()).ShouldBe([5, 4, 3, 2]);
	}

	[Fact]
	public void ShouldStopNameAtFirstNulAndPreserveCase()
	{
		using var stream = new MemoryStream();
		stream.WriteText("PWAD");
		stream.WriteInt(1);
		stream.WriteInt(12);
		stream.WriteInt(0);
		stream.WriteInt(0);
		stream.WriteArray([(byte)'A', (byte)'b', 0, (byte)'X', (byte)'Y', (byte)'Z', (byte)'1', (byte)'2']);
		stream.Position = 0;

		using var wad = WadReader.Read(stream, leaveOpen: true);

		wad[0].Name.ShouldBe(new LumpName("Ab"));
	}

	[Fact]
	public void ShouldLeaveStreamOpenWhenRequested()
	{
		using var stream = BuildWad("PWAD", 12, [(0, 0, Name("MARKER"))], []);

		using (var wad = WadReader.Read(stream, leaveOpen: true))
		{
			wad.Count.ShouldBe(1);
		}

		stream.Position = 0;
		stream.ReadByte().ShouldBe((int)'P');
	}

	[Fact]
	public void ShouldDisposeStreamByDefault()
	{
		var stream = BuildWad("PWAD", 12, [(0, 0, Name("MARKER"))], []);

		using (var wad = WadReader.Read(stream))
		{
			wad.Count.ShouldBe(1);
		}

		Assert.Throws<ObjectDisposedException>(() => _ = stream.Position);
	}

	[Fact]
	public void ShouldRejectInvalidHeader()
	{
		using var stream = BuildWad("NOPE", 12, [(0, 0, Name("MARKER"))], []);

		Assert.Throws<InvalidWadFileException>(() => WadReader.Read(stream, leaveOpen: true));
	}

	[Fact]
	public void ShouldRejectUnreadableStream()
	{
		using var stream = new NonReadableMemoryStream();

		Assert.Throws<ArgumentException>(() => WadReader.Read(stream, leaveOpen: true));
	}

	[Fact]
	public void ShouldRejectUnseekableStream()
	{
		using var stream = new NonSeekableMemoryStream();

		Assert.Throws<ArgumentException>(() => WadReader.Read(stream, leaveOpen: true));
	}

	[Fact]
	public void ShouldRejectDirectoryOutsideFile()
	{
		using var stream = new MemoryStream();
		stream.WriteText("PWAD");
		stream.WriteInt(1);
		stream.WriteInt(64);
		stream.Position = 0;

		Assert.Throws<InvalidWadFileException>(() => WadReader.Read(stream, leaveOpen: true));
	}

	[Fact]
	public void ShouldRejectLumpRangeOutsideFile()
	{
		using var stream = new MemoryStream();
		stream.WriteText("PWAD");
		stream.WriteInt(1);
		stream.WriteInt(12);
		stream.WriteInt(100);
		stream.WriteInt(10);
		stream.WriteText("DATA", totalLength: LumpName.MaxLength);
		stream.Position = 0;

		Assert.Throws<InvalidWadFileException>(() => WadReader.Read(stream, leaveOpen: true));
	}

	[Fact]
	public void ShouldRejectDataAccessAfterReaderDisposed()
	{
		var stream = BuildWad("PWAD", 12, [(28, 4, Name("DATA"))], [1, 2, 3, 4]);
		var wad = WadReader.Read(stream);
		var lump = wad[0];

		wad.Dispose();

		using var lumpStream = lump.GetData();
		Assert.Throws<ObjectDisposedException>(() => ReadAllBytes(lumpStream));
	}

	[Fact]
	public void ShouldExposeBoundedStreamLengthAndSeekWithinLump()
	{
		using var stream = BuildWad("PWAD", 12, [(28, 4, Name("DATA"))], [1, 2, 3, 4]);
		using var wad = WadReader.Read(stream, leaveOpen: true);
		using var lumpStream = wad[0].GetData();

		lumpStream.Length.ShouldBe(4);
		lumpStream.Seek(2, SeekOrigin.Begin).ShouldBe(2);
		ReadAllBytes(lumpStream).ShouldBe([3, 4]);
	}

	private static MemoryStream BuildWad(
		string identification,
		int directoryOffset,
		IReadOnlyList<(int Position, int Size, byte[] NameBytes)> entries,
		byte[] data
	)
	{
		var stream = new MemoryStream();
		stream.WriteText(identification);
		stream.WriteInt(entries.Count);
		stream.WriteInt(directoryOffset);

		while (stream.Position < directoryOffset)
		{
			stream.WriteByte(0);
		}

		foreach (var entry in entries)
		{
			stream.WriteInt(entry.Position);
			stream.WriteInt(entry.Size);
			stream.WriteArray(entry.NameBytes, LumpName.MaxLength);
		}

		if (data.Length > 0)
		{
			stream.WriteArray(data);
		}

		stream.Position = 0;
		return stream;
	}

	private static byte[] Name(string value)
	{
		var bytes = new byte[LumpName.MaxLength];
		for (var index = 0; index < value.Length; index++)
		{
			bytes[index] = (byte)value[index];
		}

		return bytes;
	}

	private static byte[] ReadAllBytes(Stream stream)
	{
		using var copy = new MemoryStream();
		stream.CopyTo(copy);
		return copy.ToArray();
	}

	private sealed class NonReadableMemoryStream : MemoryStream
	{
		public override bool CanRead => false;

		public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
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
}
