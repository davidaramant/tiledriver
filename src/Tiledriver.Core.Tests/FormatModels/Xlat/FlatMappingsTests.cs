// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.Tests.FormatModels.Xlat
{
    public sealed class FlatMappingsTests
    {
        [Fact]
        public void ShouldMergeMappings()
        {
            var fm1 = new FlatMappings(
                ceilings: new[] { "c1", "c2", "c3" },
                floors: new[] { "f1", "f2" });

            var fm2 = new FlatMappings(
                ceilings: new[] { "c10", "c20" },
                floors: new[] { "f10", "f20", "f30" });

            fm1.Add(fm2);

            fm1.Ceilings.Should().BeEquivalentTo(new[] { "c10", "c20", "c3" });
            fm1.Floors.Should().BeEquivalentTo(new[] { "f10", "f20", "f30" });
        }
    }
}