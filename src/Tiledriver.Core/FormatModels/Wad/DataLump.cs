// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Text;

namespace Tiledriver.Core.FormatModels.Wad;

public sealed class DataLump : ILump
{
	private readonly byte[] _data;
	public LumpName Name { get; }
	public bool HasData => true;

	public DataLump(LumpName name, byte[] data)
	{
		Name = name;
		_data = data;
	}

	public DataLump(LumpName name, string text)
		: this(name, Encoding.ASCII.GetBytes(text)) { }

	public static DataLump FromStream(LumpName name, Stream stream)
	{
		using var tempStream = new MemoryStream();
		stream.CopyTo(tempStream);
		return new DataLump(name, tempStream.GetBuffer());
	}

	public static DataLump ReadFromStream(LumpName name, Action<Stream> getData)
	{
		using var stream = new MemoryStream();
		getData(stream);
		return new DataLump(name, stream.GetBuffer());
	}

	public void WriteTo(Stream stream)
	{
		stream.Write(_data, 0, _data.Length);
	}

	public byte[] GetData()
	{
		return _data;
	}
}
