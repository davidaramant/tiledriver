using System.Text;

namespace Tiledriver.FormatModels.Xlat.Reading;

public static class XlatReader
{
	public static MapTranslation Read(Stream xlatStream, IResourceProvider resourceProvider)
	{
		using var textReader = new StreamReader(xlatStream, Encoding.ASCII);
		var lexer = XlatLexer.Create(textReader);
		return XlatParser.Parse(lexer.Scan(), resourceProvider);
	}
}
