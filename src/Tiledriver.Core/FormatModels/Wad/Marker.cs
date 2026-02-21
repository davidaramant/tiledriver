namespace Tiledriver.Core.FormatModels.Wad;

public sealed record Marker(LumpName Name) : ILump
{
	public bool HasData => false;

	public void WriteTo(Stream stream)
	{
		// Do nothing; no data
	}

	public byte[] GetData()
	{
		return [];
	}
}
