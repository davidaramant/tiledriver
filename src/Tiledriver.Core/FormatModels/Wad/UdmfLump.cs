// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.FormatModels.Udmf.Writing;

namespace Tiledriver.Core.FormatModels.Wad;

public sealed record UdmfLump(LumpName Name, MapData Map) : ILump
{
	public bool HasData => true;

	public void WriteTo(Stream stream) => Map.WriteTo(stream);

	public byte[] GetData() => throw new NotImplementedException();
}
