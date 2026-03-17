using Shouldly;
using Tiledriver.DemoMaps.Wolf3D;
using Tiledriver.FormatModels.Uwmf.Reading;
using Tiledriver.FormatModels.Wad;
using Tiledriver.Tests.FormatModels.Uwmf.Reading;
using Xunit;

namespace Tiledriver.Tests.FormatModels.Wad;

public sealed class WadFileTests
{
	[Fact]
	public void ShouldReadCreatedWadFile()
	{
		var fileInfo = new FileInfo(Path.GetTempFileName());
		try
		{
			var map = ThingDemoMap.Create();

			var lumps = new List<ILump>
			{
				new Marker("MAP01"),
				new UwmfLump("TEXTMAP", ThingDemoMap.Create()),
				new Marker("ENDMAP"),
			};
			WadWriter.SaveTo(lumps, fileInfo.FullName);

			var wad = WadFile.Read(fileInfo.FullName);
			wad.Count.ShouldBe(3);

			wad.Select(l => l.Name)
				.ShouldBe(
					[new LumpName("MAP01"), new LumpName("TEXTMAP"), new LumpName("ENDMAP")],
					"correct lump names should have been read."
				);

			var mapBytes = wad[1].GetData();
			using var ms = new MemoryStream(mapBytes);
			var roundTripped = UwmfReader.Read(ms);

			UwmfComparison.AssertEqual(roundTripped, map);
		}
		finally
		{
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
		}
	}
}
