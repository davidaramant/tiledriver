using Tiledriver.FormatModels.Wad;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Wad;

public sealed class DataLumpTests
{
	[Fact]
	public void ShouldCaptureBytesFromStream()
	{
		using var stream = new MemoryStream([1, 2, 3, 4]);

		var lump = DataLump.FromStream("DATA", stream);

		using var output = new MemoryStream();
		lump.WriteTo(output);
		Assert.Equal(new byte[] { 1, 2, 3, 4 }, output.ToArray());
	}
}
