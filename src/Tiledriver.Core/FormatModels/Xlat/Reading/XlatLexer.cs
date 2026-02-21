using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.Xlat.Reading;

public static class XlatLexer
{
	public static UnifiedLexer Create(TextReader reader) => new(reader, allowDollarIdentifiers: true, allowPipes: true);
}
