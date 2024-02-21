// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Tiledriver.Core.DemoMaps.Wolf3D;
using Tiledriver.Core.FormatModels.Uwmf.Reading;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.Tests.FormatModels.Uwmf.Reading;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Wad
{
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
                    new Marker("ENDMAP")
                };
                WadWriter.SaveTo(lumps, fileInfo.FullName);

                var wad = WadFile.Read(fileInfo.FullName);
                wad.Should().HaveCount(3);

                wad.Select(l => l.Name)
                    .Should()
                    .BeEquivalentTo(
                        new[] { new LumpName("MAP01"), new LumpName("TEXTMAP"), new LumpName("ENDMAP"), },
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
}
