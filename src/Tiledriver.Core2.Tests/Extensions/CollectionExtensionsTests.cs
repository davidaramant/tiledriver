// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Xunit;
using FluentAssertions;
using Tiledriver.Core.Extensions.Collections;

namespace Tiledriver.Core.Tests.Extensions
{
    public sealed class CollectionExtensionsTests
    {
        [Fact]
        public void ShouldCondenseSequenceToDictionary()
        {
            var list = new (int,string)[]
            {
                (1,"one"),
                (2,"two"),
                (1,"one again")
            };

            var d = list.CondenseToDictionary(t => t.Item1, t => t.Item2);

            d.Should().HaveCount(2, "list should have been condensed.");
            d[1].Should().Be("one again", "latest value for key should have been used.");
            d[2].Should().Be("two", "value should have been included.");
        }
    }
}
