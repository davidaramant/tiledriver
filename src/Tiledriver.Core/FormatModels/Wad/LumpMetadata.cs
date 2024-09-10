// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.FormatModels.Wad.StreamExtensions;

namespace Tiledriver.Core.FormatModels.Wad;

public sealed record LumpMetadata(int Position, int Size, LumpName Name)
{
	public void WriteTo(Stream stream)
	{
		stream.WriteInt(Position);
		stream.WriteInt(Size);
		stream.WriteText(Name.ToString(), totalLength: LumpName.MaxLength);
	}

	public static LumpMetadata ReadFrom(Stream stream)
	{
		return new LumpMetadata(
			Position: stream.ReadInt(),
			Size: stream.ReadInt(),
			Name: stream.ReadText(LumpName.MaxLength).TrimEnd((char)0)
		);
	}
}
