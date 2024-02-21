// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;
using Tiledriver.Core.DemoMaps.Wolf3D;
using Tiledriver.Core.FormatModels.Wad;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Wad
{
    public sealed class WadWriterTests
    {
        [Fact]
        public void ShouldCreateWadFile()
        {
            var fileInfo = new FileInfo(Path.GetTempFileName());
            try
            {
                var lumps = new List<ILump>
                {
                    new Marker("MAP01"),
                    new UwmfLump("TEXTMAP", ThingDemoMap.Create()),
                    new Marker("ENDMAP")
                };
                WadWriter.SaveTo(lumps, fileInfo.FullName);
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
