// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Wad;

namespace Tiledriver.Core.Tests.FormatModels.Wad
{
    [TestFixture]
    public sealed class WadFileTests
    {
        [Test]
        public void ShouldCreateWadFile()
        {
            var fileInfo = new FileInfo(Path.GetTempFileName());
            try
            {
                var wad = new WadFile();
                wad.Append(new Marker("MAP01"));
                wad.Append(new UwmfLump("TEXTMAP", DemoMap.Create()));
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
    }
}
