// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using FluentAssertions;
using System.IO;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf.Reading;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.Tests.FormatModels.Uwmf.Reading;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Wad
{
    public sealed class WadFileTests
    {
        [Fact]
        public void ShouldCreateWadFile()
        {
            var fileInfo = new FileInfo(Path.GetTempFileName());
            try
            {
                var wad = new WadFile();
                wad.Append(new Marker("MAP01"));
                wad.Append(new UwmfLump("TEXTMAP", ThingDemoMap.Create()));
                wad.Append(new Marker("ENDMAP"));
                wad.SaveTo(fileInfo.FullName);
            }
            finally
            {
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
        }

        [Fact]
        public void ShouldReadCreatedWadFile()
        {
            var fileInfo = new FileInfo(Path.GetTempFileName());
            try
            {
                var map = ThingDemoMap.Create();

                var wad = new WadFile();
                wad.Append(new Marker("MAP01"));
                wad.Append(new UwmfLump("TEXTMAP", map));
                wad.Append(new Marker("ENDMAP"));
                wad.SaveTo(fileInfo.FullName);

                wad = WadFile.Read(fileInfo.FullName);
                wad.Should().HaveCount(3);
                
                wad.Select(l=>l.Name).Should().BeEquivalentTo(
                    new[] { new LumpName("MAP01"), new LumpName("TEXTMAP"), new LumpName("ENDMAP"), },
                    "correct lump names should have been read.");

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
