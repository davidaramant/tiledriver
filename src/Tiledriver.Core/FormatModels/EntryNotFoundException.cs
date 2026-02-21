namespace Tiledriver.Core.FormatModels;

public sealed class EntryNotFoundException : Exception
{
	public EntryNotFoundException(string path)
		: base(path) { }
}
