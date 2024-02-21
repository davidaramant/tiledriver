// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using FluentAssertions;
using Tiledriver.Core.Extensions.Collections;
using Xunit;

namespace Tiledriver.Core.Tests.Extensions;

public sealed class CollectionExtensionTests
{
    [Fact]
    public void ShouldReturnDefaultFromGetValueOrIfValueDoesNotExist()
    {
        var dict = new Dictionary<int, int>();
        dict.GetValueOr(0, 1).Should().Be(1);
    }
}
