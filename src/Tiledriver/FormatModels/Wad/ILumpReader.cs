namespace Tiledriver.FormatModels.Wad;

public interface ILumpReader
{
	LumpName Name { get; }
	bool HasData { get; }
	Stream GetData();
}
