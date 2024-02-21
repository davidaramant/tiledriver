// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Moq;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.MapInfo.Reading;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.MapInfo.Reading
{
	public sealed class MapDeclarationReaderTests
	{
		[Fact]
		public void ShouldParseWolfCommonMapInfo()
		{
			using var stream = TestFile.MapInfo.wolfcommon;
			var lookup = MapDeclarationReader.Read(stream, Mock.Of<IResourceProvider>());
		}

		[Fact]
		public void ShouldParseWolf3DMapInfo()
		{
			var mockProvider = new Mock<IResourceProvider>();
			mockProvider.Setup(_ => _.Lookup("mapinfo/wolfcommon.txt")).Returns(TestFile.MapInfo.wolfcommon);

			using var stream = TestFile.MapInfo.wolf3d;

			var lookup = MapDeclarationReader.Read(stream, mockProvider.Object);
		}

		[Fact]
		public void ShouldParseSpearMapInfo()
		{
			var mockProvider = new Mock<IResourceProvider>();
			mockProvider.Setup(_ => _.Lookup("mapinfo/wolfcommon.txt")).Returns(TestFile.MapInfo.wolfcommon);

			using var stream = TestFile.MapInfo.spear;

			var lookup = MapDeclarationReader.Read(stream, mockProvider.Object);
		}
	}
}
