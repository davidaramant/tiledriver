// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using NUnit.Framework;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.Tests.FormatModels.Xlat
{
    [TestFixture]
    public sealed class FlatMappingsTests
    {
        [Test]
        public void ShouldMergeMappings()
        {
            var fm1 = new FlatMappings(
                ceiling: new[] { "c1", "c2", "c3" },
                floor: new[] { "f1", "f2" });

            var fm2 = new FlatMappings(
                ceiling: new[] { "c10", "c20" },
                floor: new[] { "f10", "f20", "f30" });

            fm1.Add(fm2);

            Assert.That(fm1.Ceiling, Is.EquivalentTo(new[] { "c10", "c20", "c3" }), "Did not merge ceilings.");
            Assert.That(fm1.Floor, Is.EquivalentTo(new[] { "f10", "f20", "f30" }), "Did not merge floors.");
        }
    }
}