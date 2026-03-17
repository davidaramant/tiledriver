using System.Text;
using Shouldly;
using Tiledriver.FormatModels.Wad;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Wad;

public sealed class DataLumpTests
{
	[Fact]
	public void FromStreamShouldReturnOnlyWrittenBytes()
	{
		byte[] expected = Encoding.ASCII.GetBytes("ABC");
		using var stream = new MemoryStream(capacity: 64);
		stream.Write(expected);
		stream.Position = 0;

		var lump = DataLump.FromStream("TEST", stream);

		lump.GetData().ShouldBe(expected);
	}

	[Fact]
	public void ReadFromStreamShouldReturnOnlyWrittenBytes()
	{
		byte[] expected = Encoding.ASCII.GetBytes("XYZ");

		var lump = DataLump.ReadFromStream("TEST", stream => stream.Write(expected));

		lump.GetData().ShouldBe(expected);
	}
}
