// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Wad;

namespace Tiledriver.Core.Tests.FormatModels.Wad
{
    [TestFixture]
    public sealed class LumpNameTests
    {
        [TestCase("")]
        [TestCase("lower")]
        [TestCase("SPACE ")]
        [TestCase("EXCESSIVE_LENGTH")]
        public void ShouldRejectInvalidNames(string name)
        {
            Assert.Throws<ArgumentException>(() => new LumpName(name));
        }

        [TestCase("NAME")]
        [TestCase("NAME1")]
        [TestCase("SPC[")]
        [TestCase("SPC]")]
        [TestCase("SPC-")]
        [TestCase("SPC_")]
        public void ShouldAcceptValidNames(string name)
        {
            Assert.DoesNotThrow(() => new LumpName(name));
        }
    }
}
