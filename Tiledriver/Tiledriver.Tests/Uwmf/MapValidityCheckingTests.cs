using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tiledriver.Uwmf;

namespace Tiledriver.Tests.Uwmf
{
    [TestFixture]
    public sealed class MapValidityCheckingTests
    {
        [Test]
        public void ShouldThrowIfThereAreTooFewPlanemapEntries()
        {
            var map = new Map
            {
                Width = 4,
                Height = 4,
                Planes = { new Plane { Depth = 64 } },
                Planemaps = { new Planemap(new[] { new PlanemapEntry(TileId.NotSpecified, SectorId.NotSpecified, ZoneId.NotSpecified), }) }
            };
            Assert.Throws<MapConstructionException>(() => map.WriteTo(null));
        }
    }
}
