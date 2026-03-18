using System.Collections;
using Tiledriver.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.FormatModels.Wad;

public sealed class WadReader : IReadOnlyList<ILumpReader>, IDisposable
{
	private readonly List<ILumpReader> _lumps = [];
	private readonly Stream _stream;
	private readonly bool _leaveOpen;
	private readonly Lock _syncRoot = new();
	private bool _disposed;

	public WadType Type { get; }
	public int Count => _lumps.Count;

	public ILumpReader this[int index] => _lumps[index];

	private WadReader(WadType type, Stream stream, bool leaveOpen, IEnumerable<ILumpReader> lumps)
	{
		Type = type;
		_stream = stream;
		_leaveOpen = leaveOpen;
		_lumps.AddRange(lumps);
	}

	public static WadReader Read(string filePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
		return Read(File.OpenRead(filePath), leaveOpen: false);
	}

	public static WadReader Read(Stream stream, bool leaveOpen = false)
	{
		if (!stream.CanRead)
		{
			throw new ArgumentException("Stream must be readable.", nameof(stream));
		}

		if (!stream.CanSeek)
		{
			throw new ArgumentException("Stream must be seekable.", nameof(stream));
		}

		var streamLength = stream.Length;
		if (streamLength < 12)
		{
			throw new InvalidWadFileException("WAD header is incomplete.");
		}

		stream.Position = 0;

		var (type, numLumps, directoryPosition) = ReadHeader(stream);
		ValidateDirectoryRange(streamLength, numLumps, directoryPosition);

		stream.Position = directoryPosition;

		var directory = Enumerable.Range(0, numLumps).Select(_ => LumpMetadata.ReadFrom(stream)).ToList();
		ValidateLumpRanges(directory, streamLength);

		return Create(type, stream, leaveOpen, directory);
	}

	private static WadReader Create(WadType type, Stream stream, bool leaveOpen, List<LumpMetadata> directory)
	{
		var wadFile = new WadReader(type, stream, leaveOpen, []);
		var lumps = directory.Select<LumpMetadata, ILumpReader>(info => new LumpReader(
			wadFile,
			info.Name,
			info.Position,
			info.Size
		));
		wadFile._lumps.AddRange(lumps);
		return wadFile;
	}

	private static (WadType Type, int NumLumps, int DirectoryPosition) ReadHeader(Stream stream)
	{
		var identification = stream.ReadText(4);
		var type = identification switch
		{
			"IWAD" => WadType.Iwad,
			"PWAD" => WadType.Pwad,
			_ => throw new InvalidWadFileException($"Unknown WAD format '{identification}'."),
		};

		var numLumps = stream.ReadInt();
		if (numLumps < 0)
		{
			throw new InvalidWadFileException("Number of lumps cannot be negative.");
		}

		var directoryPosition = stream.ReadInt();
		if (directoryPosition < 0)
		{
			throw new InvalidWadFileException("Directory offset cannot be negative.");
		}

		return (type, numLumps, directoryPosition);
	}

	public IEnumerator<ILumpReader> GetEnumerator() => _lumps.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		if (!_leaveOpen)
		{
			_stream.Dispose();
		}
	}

	private static void ValidateDirectoryRange(long streamLength, int numLumps, int directoryPosition)
	{
		var directoryLength = (long)numLumps * 16;
		var directoryEnd = directoryPosition + directoryLength;

		if (directoryEnd > streamLength)
		{
			throw new InvalidWadFileException("Directory extends beyond the end of the file.");
		}
	}

	private static void ValidateLumpRanges(IEnumerable<LumpMetadata> directory, long streamLength)
	{
		foreach (var lump in directory)
		{
			if (lump.Size < 0)
			{
				throw new InvalidWadFileException($"Lump '{lump.Name}' has a negative size.");
			}

			if (lump.Size == 0)
			{
				continue;
			}

			if (lump.Position < 0)
			{
				throw new InvalidWadFileException($"Lump '{lump.Name}' has a negative offset.");
			}

			var lumpEnd = (long)lump.Position + lump.Size;
			if (lumpEnd > streamLength)
			{
				throw new InvalidWadFileException($"Lump '{lump.Name}' extends beyond the end of the file.");
			}
		}
	}

	private int ReadLumpData(int position, long relativePosition, Span<byte> buffer)
	{
		ObjectDisposedException.ThrowIf(_disposed, this);

		if (buffer.IsEmpty)
		{
			return 0;
		}

		lock (_syncRoot)
		{
			_stream.Position = position + relativePosition;
			return _stream.Read(buffer);
		}
	}

	private sealed class LumpReader : ILumpReader
	{
		private readonly WadReader _owner;
		private readonly int _position;
		private readonly int _size;

		public LumpReader(WadReader owner, LumpName name, int position, int size)
		{
			_owner = owner;
			Name = name;
			_position = position;
			_size = size;
		}

		public LumpName Name { get; }
		public bool HasData => _size > 0;

		public Stream GetData() => new LumpStream(_owner, _position, _size);
	}

	private sealed class LumpStream : Stream
	{
		private readonly WadReader _owner;
		private readonly int _start;
		private readonly int _length;
		private long _position;
		private bool _disposed;

		public LumpStream(WadReader owner, int start, int length)
		{
			_owner = owner;
			_start = start;
			_length = length;
		}

		public override bool CanRead => !_disposed;
		public override bool CanSeek => !_disposed;
		public override bool CanWrite => false;
		public override long Length
		{
			get
			{
				ThrowIfDisposed();
				return _length;
			}
		}

		public override long Position
		{
			get
			{
				ThrowIfDisposed();
				return _position;
			}
			set
			{
				ThrowIfDisposed();
				if (value < 0 || value > _length)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				_position = value;
			}
		}

		public override void Flush()
		{
			ThrowIfDisposed();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			ThrowIfDisposed();
			return Read(buffer.AsSpan(offset, count));
		}

		public override int Read(Span<byte> buffer)
		{
			ThrowIfDisposed();
			ObjectDisposedException.ThrowIf(_owner._disposed, _owner);

			var remaining = _length - _position;
			if (remaining <= 0)
			{
				return 0;
			}

			var count = (int)Math.Min(buffer.Length, remaining);
			var bytesRead = _owner.ReadLumpData(_start, _position, buffer[..count]);
			_position += bytesRead;
			return bytesRead;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			ThrowIfDisposed();

			var newPosition = origin switch
			{
				SeekOrigin.Begin => offset,
				SeekOrigin.Current => _position + offset,
				SeekOrigin.End => _length + offset,
				_ => throw new ArgumentOutOfRangeException(nameof(origin)),
			};

			if (newPosition < 0 || newPosition > _length)
			{
				throw new IOException("Attempted to seek outside the bounds of the lump.");
			}

			_position = newPosition;
			return _position;
		}

		public override void SetLength(long value) => throw new NotSupportedException();

		public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

		protected override void Dispose(bool disposing)
		{
			_disposed = true;
			base.Dispose(disposing);
		}

		private void ThrowIfDisposed()
		{
			ObjectDisposedException.ThrowIf(_disposed, this);
		}
	}
}
