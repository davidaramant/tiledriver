// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.GameMaps;

namespace Tiledriver.Core.Tests.FormatModels.GameMaps
{
    [TestFixture]
    public sealed class ExpanderTests
    {
        [Test]
        public void ShouldPassThroughRlewBytesWithNoMarker()
        {
            var input = Enumerable.Repeat<byte>(0xFF, 40).ToArray();
            var expanded = Expander.DecompressRlew(0xABCD, input, input.Length);
            Assert.That(expanded, Is.EqualTo(input), "Should not have mutated array.");
        }

        [Test]
        public void ShouldExpandASimpleRlewSubstitution()
        {
            var input = new byte[] { 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF };
            var expected = Enumerable.Repeat<byte>(0xFF, 16).ToArray();
            var expanded = Expander.DecompressRlew(0xABCD, input, expected.Length);
            Assert.That(expanded, Is.EqualTo(expected), "Should have expanded array.");
        }

        [Test]
        public void ShouldExpandRlewWithPrefix()
        {
            var input = new byte[] { 0x1A, 0x2B, 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF };
            var expected = new byte[] { 0x1A, 0x2B }.Concat(Enumerable.Repeat<byte>(0xFF, 16)).ToArray();
            var expanded = Expander.DecompressRlew(0xABCD, input, expected.Length);
            Assert.That(expanded, Is.EqualTo(expected), "Should have expanded array.");
        }

        [Test]
        public void ShouldExpandRlewWithAppendix()
        {
            var input = new byte[] { 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF, 0x1A, 0x2B };
            var expected = Enumerable.Repeat<byte>(0xFF, 16).Concat(new byte[] { 0x1A, 0x2B }).ToArray();
            var expanded = Expander.DecompressRlew(0xABCD, input, expected.Length);
            Assert.That(expanded, Is.EqualTo(expected), "Should have expanded array.");
        }

        [Test]
        public void ShouldExpandMultipleRlewSections()
        {
            var input = new byte[] { 0x55, 0x66, 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF, 0x1A, 0x2B, 0xCD, 0xAB, 0x04, 0x00, 0x11, 0x22, 0x33, 0x44 };
            var expected =
                new byte[] { 0x55, 0x66 }.
                Concat(Repeat(new byte[] { 0xFF, 0xFF }, 8)).
                Concat(new byte[] { 0x1A, 0x2B }).
                Concat(Repeat(new byte[] { 0x11, 0x22 }, 4)).
                Concat(new byte[] { 0x33, 0x44 }).ToArray();

            var expanded = Expander.DecompressRlew(0xABCD, input, expected.Length);
            Assert.That(expanded, Is.EqualTo(expected), "Should have expanded array.");
        }

        [Test]
        public void ShouldUnCarmackSimpleData()
        {
            var input = new byte[] { 0x08, 0x00, 0x00, 0x20, 0xCD, 0xAB, 0x00, 0x10, 0x00, 0x00 };
            var expected = new byte[] { 0x00, 0x20, 0xCD, 0xAB, 0x00, 0x10, 0x00, 0x00 };
            var output = Expander.DecompressCarmack(input);
            Assert.That(output, Is.EqualTo(expected), "Should have decompressed array.");
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