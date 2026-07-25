namespace Tiledriver.FormatModels.Common.Reading;

public interface ITokenScanner
{
	IEnumerable<Token> Scan();
}
