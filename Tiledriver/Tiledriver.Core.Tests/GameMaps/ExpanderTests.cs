/*
** ExpanderTests.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

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