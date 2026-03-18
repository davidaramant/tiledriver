namespace Tiledriver.FormatModels.Wad;

public sealed class InvalidWadFileException : Exception
{
	public InvalidWadFileException(string message)
		: base(message) { }

	public InvalidWadFileException(string message, Exception innerException)
		: base(message, innerException) { }
}
