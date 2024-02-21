// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.GameMaps;

public sealed record PlaneMetadata(uint Offset, ushort CompressedLength)
{
	public override string ToString() => $"Offset: {Offset:x8}, Length: {CompressedLength:x4} ({CompressedLength})";
}
