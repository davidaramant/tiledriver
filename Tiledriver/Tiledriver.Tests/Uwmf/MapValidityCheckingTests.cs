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
        public void ShouldNotThrowWithValidMap()
        {
            var map = DemoMap.Create();
            Assert.DoesNotThrow(map.CheckSemanticValidity);
        }

        [Test]
        public void ShouldThrowIfThereAreTooFewPlanemapEntries()
        {
            var map = DemoMap.Create();
            map.Planemaps.First().Entries.RemoveAt(0);
            Assert.Throws<MapConstructionException>(map.CheckSemanticValidity);
        }

        [Test]
        public void ShouldThrowIfThereAreTooManyPlanemapEntries()
        {
            var map = DemoMap.Create();

            var entries = map.Planemaps.First().Entries;

            entries.Add(entries.First());
            Assert.Throws<MapConstructionException>(map.CheckSemanticValidity);
        }
    }
}
