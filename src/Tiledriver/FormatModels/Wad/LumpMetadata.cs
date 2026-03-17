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
		new(
			Position: stream.ReadInt(),
			Size: stream.ReadInt(),
			Name: stream.ReadText(LumpName.MaxLength).TrimEnd((char)0)
		);
}
