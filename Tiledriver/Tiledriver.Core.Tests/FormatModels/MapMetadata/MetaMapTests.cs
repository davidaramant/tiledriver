// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.MapMetadata;

namespace Tiledriver.Core.Tests.FormatModels.MapMetadata
{
    [TestFixture]
    public sealed class MetaMapTests
    {
        [Test]
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

                Assert.That(roundTripped.Width, Is.EqualTo(m.Width), "Width");
                Assert.That(roundTripped.Height, Is.EqualTo(m.Height), "Height");
                for (int y = 0; y < m.Height; y++)
                {
                    for (int x = 0; x < m.Width; x++)
                    {
                        Assert.That(roundTripped[x, y], Is.EqualTo(m[x, y]), $"({x},{y})");
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
}