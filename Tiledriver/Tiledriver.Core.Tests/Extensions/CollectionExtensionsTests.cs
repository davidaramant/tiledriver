// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using NUnit.Framework;
using Tiledriver.Core.Extensions;

namespace Tiledriver.Core.Tests.Extensions
{
    [TestFixture]
    public sealed class CollectionExtensionsTests
    {
        [Test]
        public void ShouldCondenseSequenceToDictionary()
        {
            var list = new (int,string)[]
            {
                (1,"one"),
                (2,"two"),
                (1,"one again")
            };

            var d = list.CondenseToDictionary(t => t.Item1, t => t.Item2);

            Assert.That(d, Has.Count.EqualTo(2), "Did not condense list");
            Assert.That(d[1],Is.EqualTo("one again"),"Did not use latest value for key.");
            Assert.That(d[2], Is.EqualTo("two"), "Did not include value.");
        }
    }
}
