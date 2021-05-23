// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Xunit;
using Tiledriver.Core.FormatModels.Wad;

namespace Tiledriver.Core.Tests.FormatModels.Wad
{
    public sealed class LumpNameTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("lower")]
        [InlineData("SPACE ")]
        [InlineData("EXCESSIVE_LENGTH")]
        public void ShouldRejectInvalidNames(string name)
        {
            Assert.Throws<ArgumentException>(() => new LumpName(name));
        }

        [Theory]
        [InlineData("NAME")]
        [InlineData("NAME1")]
        [InlineData("SPC[")]
        [InlineData("SPC]")]
        [InlineData("SPC-")]
        [InlineData("SPC_")]
        public void ShouldAcceptValidNames(string name)
        {
            var _ = new LumpName(name);
        }
    }
}
