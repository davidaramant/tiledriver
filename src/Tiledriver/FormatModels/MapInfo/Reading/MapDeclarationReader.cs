using System.Text;
using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.MapInfo.Reading;

public static class MapDeclarationReader
{
	public static IReadOnlyDictionary<string, Map> Read(Stream stream, IResourceProvider resourceProvider)
	{
		using var reader = new StreamReader(stream, Encoding.ASCII, leaveOpen: true);
		var tokenSource = new TokenSource(MapInfoLexer.Create(reader).Scan(), resourceProvider, MapInfoLexer.Create);
		using var tokenStream = tokenSource.GetEnumerator();
		return MapDeclarationParser.ReadMapDeclarations(tokenStream);
	}
}
