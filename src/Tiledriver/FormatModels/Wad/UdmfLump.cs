using Tiledriver.FormatModels.Udmf;
using Tiledriver.FormatModels.Udmf.Writing;

namespace Tiledriver.FormatModels.Wad;

public sealed record UdmfLump(LumpName Name, MapData Map) : ILump
{
	public bool HasData => true;

	public void WriteTo(Stream stream) => Map.WriteTo(stream);
}
