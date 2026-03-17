using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.MapInfo.Reading;

public static class MapInfoLexer
{
	public static UnifiedLexer Create(TextReader reader) => new(reader, reportNewlines: true);
}
