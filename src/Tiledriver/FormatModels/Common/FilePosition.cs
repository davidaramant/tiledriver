namespace Tiledriver.FormatModels.Common;

public readonly record struct FilePosition(int Line, int Column)
{
	public static readonly FilePosition StartOfFile = new(1, 1);

	public FilePosition NextChar() => new(Line, Column + 1);

	public FilePosition NextLine() => new(Line + 1, 1);

	public override string ToString() => $"Line: {Line}, Col: {Column}";
}
