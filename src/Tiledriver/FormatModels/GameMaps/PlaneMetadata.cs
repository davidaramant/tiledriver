namespace Tiledriver.FormatModels.GameMaps;

public sealed record PlaneMetadata(uint Offset, ushort CompressedLength)
{
	public override string ToString() => $"Offset: {Offset:x8}, Length: {CompressedLength:x4} ({CompressedLength})";
}
