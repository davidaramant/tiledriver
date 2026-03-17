using Tiledriver.FormatModels.Uwmf;
using Tiledriver.FormatModels.Uwmf.Writing;

namespace Tiledriver.FormatModels.Wad;

public sealed record UwmfLump(LumpName Name, MapData Map) : ILump
{
	public bool HasData => true;

	public void WriteTo(Stream stream) => Map.WriteTo(stream);

	public byte[] GetData() => throw new NotImplementedException();
}
