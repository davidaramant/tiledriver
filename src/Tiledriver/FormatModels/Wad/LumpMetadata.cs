using System.Text;
using Tiledriver.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.FormatModels.Wad;

public sealed record LumpMetadata(int Position, int Size, LumpName Name)
{
	public void WriteTo(Stream stream)
	{
		stream.WriteInt(Position);
		stream.WriteInt(Size);
		stream.WriteText(Name.ToString(), totalLength: LumpName.MaxLength);
	}

	public static LumpMetadata ReadFrom(Stream stream) =>
		new(Position: stream.ReadInt(), Size: stream.ReadInt(), Name: ReadName(stream));

	private static LumpName ReadName(Stream stream)
	{
		Span<byte> rawName = stackalloc byte[LumpName.MaxLength];
		stream.ReadExactly(rawName);

		var terminatorIndex = rawName.IndexOf((byte)0);
		var nameLength = terminatorIndex >= 0 ? terminatorIndex : rawName.Length;

		for (var index = 0; index < nameLength; index++)
		{
			if (rawName[index] > 0x7f)
			{
				throw new InvalidWadFileException("Lump names must contain only 7-bit ASCII characters.");
			}
		}

		return new LumpName(Encoding.ASCII.GetString(rawName[..nameLength]));
	}
}
