using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.MapInfo.Reading;

public static class MapInfoLexer
{
	public static UnifiedLexer Create(TextReader reader) => new(reader, reportNewlines: true);
}
