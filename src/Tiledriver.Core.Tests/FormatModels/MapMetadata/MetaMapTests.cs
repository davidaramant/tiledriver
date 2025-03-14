// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Shouldly;
using Tiledriver.Core.FormatModels.MapMetadata;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.MapMetadata;

public sealed class MetaMapTests
{
	[Fact]
	public void ShouldRoundTripMetaMap()
	{
		var numTypes = Enum.GetValues(typeof(TileType)).Length;

		var m = new MetaMap(4, 5);
		for (int y = 0; y < m.Height; y++)
		{
			for (int x = 0; x < m.Width; x++)
			{
				m[x, y] = (TileType)((y * m.Width + x) % numTypes);
			}
		}

		var tempPath = Path.GetTempFileName();

		try
		{
			m.Save(tempPath);
			var roundTripped = MetaMap.Load(tempPath);

			roundTripped.Width.ShouldBe(m.Width);
			roundTripped.Height.ShouldBe(m.Height);
			for (int y = 0; y < m.Height; y++)
			{
				for (int x = 0; x < m.Width; x++)
				{
					roundTripped[x, y].ShouldBe(m[x, y], $"should have matched at ({x},{y})");
				}
			}
		}
		finally
		{
			if (File.Exists(tempPath))
			{
				File.Delete(tempPath);
			}
		}
	}
}
