// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Functional.Maybe;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat.Parsing;
using Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Parsing
{
    [TestFixture]
    public sealed class XlatParserTests
    {
        #region Global Expressions

        [Test]
        public void ShouldEnableLightLevels()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Simple(
                    name: new Identifier( "enable"),
                    oldnum: Maybe<ushort>.Nothing,
                    qualifiers:new [] { new Identifier("lightlevels"), })
            });

            Assert.That(translator.EnableLightLevels, Is.True, "Did not parse 'enable lightlevels'");
        }

        #endregion Global Expressions

        #region Tiles section

        [Test]
        public void ShouldParseTileInTiles()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("tiles"), new []
                {
                    Expression.PropertyList(
                        name:new Identifier("tile").ToMaybe(),
                        oldnum:((ushort)123).ToMaybe(),
                        qualifiers:Enumerable.Empty<Identifier>(),
                        properties:new[]
                        {
                            new Assignment("textureEast", Token.String("tex1") ),
                            new Assignment("textureNorth", Token.String("tex2") ),
                            new Assignment("textureSouth", Token.String("tex3") ),
                            new Assignment("textureWest", Token.String("tex4") ),
                        }),
                })
            });

            var tile = translator.TileMappings.Tiles.Lookup((ushort)123).OrElse(() => new AssertionException("Did not include tile."));

            Assert.That(tile.TextureEast, Is.EqualTo("tex1"), "Did not set Texture East");
            Assert.That(tile.TextureNorth, Is.EqualTo("tex2"), "Did not set Texture North");
            Assert.That(tile.TextureSouth, Is.EqualTo("tex3"), "Did not set Texture South");
            Assert.That(tile.TextureWest, Is.EqualTo("tex4"), "Did not set Texture West");
        }

        [Test]
        public void ShouldParseZoneInTiles()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("tiles"), new []
                {
                    Expression.ValueList(
                        name:new Identifier("zone").ToMaybe(),
                        oldnum:((ushort)123).ToMaybe(),
                        qualifiers:Enumerable.Empty<Identifier>(),
                        values:Enumerable.Empty<Token>()),
                })
            });

            Assert.That(translator.TileMappings.Zones.ContainsKey(123), Is.True, "Did not parse sound zone.");
        }

        #endregion Tiles section

        #region Things section

        #endregion Things section

        #region Flats section

        [Test]
        public void ShouldParseFlatsSection()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("flats"), new []
                {
                    Expression.ValueList(
                        name:new Identifier("ceiling").ToMaybe(),
                        oldnum:Maybe<ushort>.Nothing,
                        qualifiers:Enumerable.Empty<Identifier>(),
                        values:new []
                        {

                        Token.String("flat1"),
                        Token.String("flat2"),
                        }),
                    Expression.ValueList(
                        name:new Identifier("floor").ToMaybe(),
                        oldnum:Maybe<ushort>.Nothing,
                        qualifiers:Enumerable.Empty<Identifier>(),
                        values:new []
                        {

                        Token.String("flat3"),
                        Token.String("flat4"),
                        }),
                })
            });

            Assert.That(translator.FlatMappings.Ceiling, Is.EquivalentTo(new[] { "flat1", "flat2" }), "Did not parse ceiling.");
            Assert.That(translator.FlatMappings.Floor, Is.EquivalentTo(new[] { "flat3", "flat4" }), "Did not parse floor.");
        }

        #endregion Flats section
    }
}