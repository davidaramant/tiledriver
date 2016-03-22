// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;
using NUnit.Framework;
using Tiledriver.Core.Uwmf;
using Tiledriver.Core.Uwmf.Parsing;

namespace Tiledriver.Core.Tests.Uwmf.Parsing
{
    [TestFixture]
    public sealed class ParserTests
    {
        [Test]
        public void ShouldRoundTripMinimalMap()
        {
            var map = new Map
            {
                Namespace = "1",
                Width = 2,
                Height = 3,
                TileSize = 4,
                Name = "5",
            };

            using (var stream = new MemoryStream())
            {
                map.WriteTo(stream);

                stream.Position = 0;

                var roundTripped = Parser.Parse(new Lexer(new UwmfCharReader(stream)));

                AssertEqual(roundTripped,map);
            }
        }

        private static void AssertEqual(Map actual, Map expected)
        {
            Assert.That(actual.Namespace, Is.EqualTo(expected.Namespace),
                "Namespace was not equal.");

            Assert.That(actual.Width, Is.EqualTo(expected.Width),
                "Width was not equal.");

            Assert.That(actual.Height, Is.EqualTo(expected.Height),
                "Height was not equal.");

            Assert.That(actual.TileSize, Is.EqualTo(expected.TileSize),
                "TileSize was not equal.");

            Assert.That(actual.Name, Is.EqualTo(expected.Name),
                "Name was not equal.");
        }
    }
}