using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.Xlat.Reading;

public static class XlatLexer
{
	public static ITokenScanner Create(TextReader reader) =>
		new TokenScanner(reader, new TokenScannerOptions(AllowDollarIdentifiers: true, AllowPipes: true));
}
