using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.MapInfo.Reading;

public static class MapInfoLexer
{
	public static ITokenScanner Create(TextReader reader) =>
		new TokenScanner(reader, new TokenScannerOptions(ReportNewlines: true));
}
