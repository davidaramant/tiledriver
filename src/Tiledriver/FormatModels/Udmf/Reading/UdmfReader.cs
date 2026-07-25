using System.Text;

namespace Tiledriver.FormatModels.Udmf.Reading;

public static class UdmfReader
{
	public static MapData Read(Stream stream)
	{
		using var textReader = new StreamReader(stream, Encoding.ASCII, leaveOpen: true);
		return new UdmfParser(new DirectLexer(textReader)).Parse();
	}
}
