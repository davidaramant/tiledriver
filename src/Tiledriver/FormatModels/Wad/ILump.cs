namespace Tiledriver.FormatModels.Wad;

public interface ILump
{
	LumpName Name { get; }
	bool HasData { get; }

	void WriteTo(Stream stream);
	byte[] GetData();
}
