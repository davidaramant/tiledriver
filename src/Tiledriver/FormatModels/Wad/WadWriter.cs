using Tiledriver.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.FormatModels.Wad;

public static class WadWriter
{
	public static void SaveTo(IEnumerable<ILump> lumps, string filePath, WadType type = WadType.Pwad)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		using var fs = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
		WriteTo(lumps, fs, type);
	}

	public static void WriteTo(IEnumerable<ILump> lumps, Stream stream, WadType type = WadType.Pwad)
	{
		if (!stream.CanWrite)
		{
			throw new ArgumentException("Stream must be writable.", nameof(stream));
		}

		if (!stream.CanSeek)
		{
			throw new ArgumentException("Stream must be seekable.", nameof(stream));
		}

		var lumpList = lumps.ToList();

		stream.WriteText(
			type switch
			{
				WadType.Iwad => "IWAD",
				WadType.Pwad => "PWAD",
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
			}
		);
		stream.WriteInt(lumpList.Count);

		// Fill in this position after writing the data
		var directoryOffsetPosition = checked((int)stream.Position);
		stream.Position += 4;

		var metadata = new List<LumpMetadata>();
		foreach (var lump in lumpList)
		{
			var startOfLump = checked((int)stream.Position);

			lump.WriteTo(stream);

			metadata.Add(
				new LumpMetadata(
					Position: startOfLump,
					Size: checked((int)stream.Position) - startOfLump,
					Name: lump.Name
				)
			);
		}

		var startOfDirectory = checked((int)stream.Position);

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
