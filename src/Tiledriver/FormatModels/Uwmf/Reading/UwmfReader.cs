using System.Text;
using Tiledriver.FormatModels.Udmf.Reading;

namespace Tiledriver.FormatModels.Uwmf.Reading;

public static class UwmfReader
{
	public static MapData Read(Stream stream)
	{
		using var textReader = new StreamReader(stream, Encoding.ASCII, leaveOpen: true);
		return new UwmfParser(new DirectLexer(textReader)).Parse();
	}
}
