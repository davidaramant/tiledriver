// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.GameMaps;

namespace Tiledriver.Core.Tests.GameMaps
{
    [TestFixture]
    public sealed class ExpanderTests
    {
        [Test]
        public void ShouldPassThroughBytesWithNoMarker()
        {
            var input = Enumerable.Repeat<byte>(0xFF, 40).ToArray();
            var expanded = Expander.DecompressRlew(0xABCD, input);
            Assert.That(expanded, Is.EqualTo(input), "Should not have mutated array.");
        }

        [Test]
        public void ShouldExpandASimpleSubstitution()
        {
            var input = new byte[] { 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF };
            var expanded = Expander.DecompressRlew(0xABCD, input);
            Assert.That(expanded, Is.EqualTo(Enumerable.Repeat<byte>(0xFF, 16).ToArray()), "Should have expanded array.");
        }

        [Test]
        public void ShouldExpandWithPrefix()
        {
            var input = new byte[] { 0x1A, 0x2B, 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF };
            var expanded = Expander.DecompressRlew(0xABCD, input);
            Assert.That(expanded, Is.EqualTo(new byte[] { 0x1A, 0x2B }.Concat(Enumerable.Repeat<byte>(0xFF, 16)).ToArray()), "Should have expanded array.");
        }

        [Test]
        public void ShouldExpandWithAppendix()
        {
            var input = new byte[] { 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF, 0x1A, 0x2B };
            var expanded = Expander.DecompressRlew(0xABCD, input);
            Assert.That(expanded, Is.EqualTo(Enumerable.Repeat<byte>(0xFF, 16).Concat(new byte[] { 0x1A, 0x2B }).ToArray()), "Should have expanded array.");
        }

        [Test]
        public void ShouldExpandMultipleSections()
        {
            var input = new byte[] { 0x55, 0x66, 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF, 0x1A, 0x2B, 0xCD, 0xAB, 0x04, 0x00, 0x11, 0x22, 0x33, 0x44 };
            var expected =
                new byte[] { 0x55, 0x66 }.
                Concat(Repeat(new byte[] { 0xFF, 0xFF }, 8)).
                Concat(new byte[] { 0x1A, 0x2B }).
                Concat(Repeat(new byte[] { 0x11, 0x22 }, 4)).
                Concat(new byte[] { 0x33, 0x44 }).ToArray();

            var expanded = Expander.DecompressRlew(0xABCD, input);
            Assert.That(expanded, Is.EqualTo(expected), "Should have expanded array.");
        }

        private static byte[] Repeat(byte[] sequence, int times)
        {
            var result = new byte[times * sequence.Length];

            for (int i = 0; i < times; i++)
            {
                Buffer.BlockCopy(sequence, 0, result, i * sequence.Length, sequence.Length);
            }

            return result;
        }
    }
}