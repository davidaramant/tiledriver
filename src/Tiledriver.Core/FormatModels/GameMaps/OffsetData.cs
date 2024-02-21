// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tiledriver.Core.FormatModels.GameMaps;

public sealed class OffsetData
{
	public const int MaxLevelOffsets = 100;

	public ushort RlewMarker { get; }
	public IReadOnlyList<uint> Offsets { get; }

	public OffsetData(ushort rlewMarker, IEnumerable<uint> offsets)
	{
		RlewMarker = rlewMarker;
		Offsets = offsets.TakeWhile(offset => offset != 0 && offset != uint.MaxValue).ToList();
	}

	public static OffsetData ReadOffsets(Stream stream)
	{
		var buffer = new byte[2 + 4 * MaxLevelOffsets];
		stream.Read(buffer, 0, buffer.Length);
		var rlewMarker = BitConverter.ToUInt16(buffer, 0);

		var offsets = new List<uint>();

		for (int i = 0; i < MaxLevelOffsets; i++)
		{
			offsets.Add(BitConverter.ToUInt32(buffer, 2 + 4 * i));
		}

		return new OffsetData(rlewMarker: rlewMarker, offsets: offsets);
	}
}
