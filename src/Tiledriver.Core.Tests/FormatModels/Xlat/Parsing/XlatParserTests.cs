// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using System.Text;
using Functional.Maybe;
using Moq;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat;
using Tiledriver.Core.FormatModels.Xlat.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Parsing
{
    public sealed class XlatParserTests
    {
        [Fact]
        public void ShouldParseWolf3DXlat()
        {
            using (var stream = TestFile.Xlat.wolf3d)
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                var syntaxAnalzer = new XlatSyntaxAnalyzer(Mock.Of<IResourceProvider>());
                var result = syntaxAnalzer.Analyze(lexer);
                var translator = XlatParser.Parse(result);
            }
        }

        [Fact]
        public void ShouldParseSpearXlat()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider.Setup(_ => _.Lookup("xlat/wolf3d.txt"))
                .Returns(TestFile.Xlat.wolf3d);

            using (var stream = TestFile.Xlat.spear)
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                var syntaxAnalzer = new XlatSyntaxAnalyzer(mockProvider.Object);
                var result = syntaxAnalzer.Analyze(lexer);
                var translator = XlatParser.Parse(result);
            }
        }

        #region Global Expressions

        [Fact]
        public void ShouldEnableLightLevels()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Simple(
                    name: new Identifier( "enable"),
                    oldnum: Maybe<ushort>.Nothing,
                    qualifiers:new [] { Token.Identifier("lightlevels"), })
            });

            translator.EnableLightLevels.Should().BeTrue();
        }

        #endregion Global Expressions

        #region Tiles section

        [Fact]
        public void ShouldParseAmbushModzoneInTiles()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("tiles"), new []
                {
                    Expression.Simple(
                        name:new Identifier("modzone"),
                        oldnum:((ushort)1).ToMaybe(),
                        qualifiers:new [] {Token.Identifier("ambush"), }),
                    Expression.Simple(
                        name:new Identifier("modzone"),
                        oldnum:((ushort)2).ToMaybe(),
                        qualifiers:new [] { Token.Identifier("fillzone"), Token.Identifier("ambush"), }),
                })
            });

            var mz1 = translator.TileMappings.AmbushModzones.Single(amz => amz.OldNum == 1);
            mz1.Fillzone.Should().BeFalse("modzone without ambush flag should be parsed.");

            var mz2 = translator.TileMappings.AmbushModzones.Single(amz => amz.OldNum == 2);
            mz2.Fillzone.Should().BeTrue("modzone with ambush flag should be parsed.");
        }

        [Fact]
        public void ShouldParseChangeTriggerModzoneInTiles()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("tiles"), new []
                {
                    Expression.PropertyList(
                        name:new Identifier("modzone").ToMaybe(),
                        oldnum:((ushort)123).ToMaybe(),
                        qualifiers:new [] { Token.Identifier("fillzone"), Token.Identifier("changetrigger"), Token.String("someoriginalaction") },
                        properties:new[]
                        {
                            new Assignment("action", Token.String("someaction") ),
                            new Assignment("activateEast", Token.BooleanFalse ),
                        }),
                })
            });

            var trigger = translator.TileMappings.ChangeTriggerModzones.Single();

            trigger.OldNum.Should().Be(123);
            trigger.Fillzone.Should().BeTrue();
            trigger.Action.Should().Be("someoriginalaction");
            trigger.TriggerTemplate.Action.Should().Be("someaction");
            trigger.TriggerTemplate.ActivateEast.Should().BeFalse();
        }

        [Fact]
        public void ShouldParseTileInTiles()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("tiles"), new []
                {
                    Expression.PropertyList(
                        name:new Identifier("tile").ToMaybe(),
                        oldnum:((ushort)123).ToMaybe(),
                        qualifiers:Enumerable.Empty<Token>(),
                        properties:new[]
                        {
                            new Assignment("textureEast", Token.String("tex1") ),
                            new Assignment("textureNorth", Token.String("tex2") ),
                            new Assignment("textureSouth", Token.String("tex3") ),
                            new Assignment("textureWest", Token.String("tex4") ),
                        }),
                })
            });

            var tile = translator.TileMappings.TileTemplates.Single();

            tile.OldNum.Should().Be(123);
            tile.TextureEast.Should().Be("tex1");
            tile.TextureNorth.Should().Be("tex2");
            tile.TextureSouth.Should().Be("tex3");
            tile.TextureWest.Should().Be("tex4");
        }

        [Fact]
        public void ShouldParseTriggerInTiles()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("tiles"), new []
                {
                    Expression.PropertyList(
                        name:new Identifier("trigger").ToMaybe(),
                        oldnum:((ushort)123).ToMaybe(),
                        qualifiers:Enumerable.Empty<Token>(),
                        properties:new[]
                        {
                            new Assignment("action", Token.String("someaction") ),
                            new Assignment("activateEast", Token.BooleanFalse ),
                        }),
                })
            });

            var trigger = translator.TileMappings.TriggerTemplates.Single();

            trigger.OldNum.Should().Be(123);
            trigger.Action.Should().Be("someaction");
            trigger.ActivateEast.Should().BeFalse();
        }

        [Fact]
        public void ShouldParseZoneInTiles()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("tiles"), new []
                {
                    Expression.ValueList(
                        name:new Identifier("zone").ToMaybe(),
                        oldnum:((ushort)123).ToMaybe(),
                        qualifiers:Enumerable.Empty<Token>(),
                        values:Enumerable.Empty<Token>()),
                })
            });

            translator.TileMappings.ZoneTemplates.Single().OldNum.Should().Be(123);
        }

        #endregion Tiles section

        #region Things section

        [Fact]
        public void ShouldParseElevatorInThings()
        {
            var translator = XlatParser.Parse(new[]
{
                Expression.Block(new Identifier("things"), new []
                {
                    Expression.ValueList(
                        name:new Identifier("elevator").ToMaybe(),
                        oldnum:((ushort)123).ToMaybe(),
                        qualifiers:Enumerable.Empty<Token>(),
                        values:Enumerable.Empty<Token>()),
                })
            });

            translator.LookupThingMapping(123).Should().BeOfType<Elevator>();
        }

        [Fact]
        public void ShouldParseTriggerInThings()
        {
            var translator = XlatParser.Parse(new[]
            {
                        Expression.Block(new Identifier("things"), new []
                        {
                            Expression.PropertyList(
                                name:new Identifier("trigger").ToMaybe(),
                                oldnum:((ushort)123).ToMaybe(),
                                qualifiers:Enumerable.Empty<Token>(),
                                properties:new[]
                                {
                                    new Assignment("action", Token.String("someaction") ),
                                    new Assignment("activateEast", Token.BooleanFalse ),
                                }),
                        })
                    });

            IThingMapping mapping = translator.LookupThingMapping(123);
            mapping.Should().BeOfType<TriggerTemplate>();

            var trigger = (TriggerTemplate)mapping;

            trigger.OldNum.Should().Be(123);
            trigger.Action.Should().Be("someaction");
            trigger.ActivateEast.Should().BeFalse();
        }

        [Fact]
        public void ShouldParseSimpleThingDefinitionInThings()
        {
            // 	{23,  Puddle,            4, 0, 2}
            var translator = XlatParser.Parse(new[]
            {
                        Expression.Block(new Identifier("things"), new []
                        {
                            Expression.ValueList(
                                name:Maybe<Identifier>.Nothing,
                                oldnum:Maybe<ushort>.Nothing,
                                qualifiers:Enumerable.Empty<Token>(),
                                values:new[]
                                {
                                    Token.Integer(23), Token.Comma,
                                    Token.Identifier("Puddle"), Token.Comma,
                                    Token.Integer(4), Token.Comma,
                                    Token.Integer(0), Token.Comma,
                                    Token.Integer(2),
                                }),
                        })
                    });

            IThingMapping mapping = translator.LookupThingMapping(23);
            mapping.Should().BeOfType<ThingTemplate>();

            var thingTemplate = (ThingTemplate)mapping;

            thingTemplate.OldNum.Should().Be(23);
            thingTemplate.Type.Should().Be("Puddle");
            thingTemplate.Angles.Should().Be(4);
            thingTemplate.Pathing.Should().BeFalse();
            thingTemplate.Holowall.Should().BeFalse();
            thingTemplate.Minskill.Should().Be(2);
        }

        [Fact]
        public void ShouldParseThingDefinitionWithMetaInThings()
        {
            // 	{23,  $Puddle,            4, 0, 2}
            var translator = XlatParser.Parse(new[]
            {
                        Expression.Block(new Identifier("things"), new []
                        {
                            Expression.ValueList(
                                name:Maybe<Identifier>.Nothing,
                                oldnum:Maybe<ushort>.Nothing,
                                qualifiers:Enumerable.Empty<Token>(),
                                values:new[]
                                {
                                    Token.Integer(23), Token.Comma,
                                    Token.Meta, Token.Identifier("Puddle"), Token.Comma,
                                    Token.Integer(4), Token.Comma,
                                    Token.Integer(0), Token.Comma,
                                    Token.Integer(2),
                                }),
                        })
                    });

            IThingMapping mapping = translator.LookupThingMapping(23);
            mapping.Should().BeOfType<ThingTemplate>();

            var thingTemplate = (ThingTemplate)mapping;

            thingTemplate.OldNum.Should().Be(23);
            thingTemplate.Type.Should().Be("$Puddle");
            thingTemplate.Angles.Should().Be(4);
            thingTemplate.Pathing.Should().BeFalse();
            thingTemplate.Holowall.Should().BeFalse();
            thingTemplate.Minskill.Should().Be(2);
        }


        [Fact]
        public void ShouldParseThingDefinitionWitFlagsInThings()
        {
            // 	{23,  Puddle,            4, HOLOWALL, 2}
            // 	{124,  Puddle,            5, PATHING|HOLOWALL, 3}
            // 	{225,  Puddle,            6, AMBUSH|PATHING, 4}
            var translator = XlatParser.Parse(new[]
            {
                        Expression.Block(new Identifier("things"), new []
                        {
                            Expression.ValueList(
                                name:Maybe<Identifier>.Nothing,
                                oldnum:Maybe<ushort>.Nothing,
                                qualifiers:Enumerable.Empty<Token>(),
                                values:new[]
                                {
                                    Token.Integer(23), Token.Comma,
                                    Token.Identifier("Puddle"), Token.Comma,
                                    Token.Integer(4), Token.Comma,
                                    Token.Identifier("HOLOWALL"), Token.Comma,
                                    Token.Integer(2),
                                }),
                            Expression.ValueList(
                                name:Maybe<Identifier>.Nothing,
                                oldnum:Maybe<ushort>.Nothing,
                                qualifiers:Enumerable.Empty<Token>(),
                                values:new[]
                                {
                                    Token.Integer(124), Token.Comma,
                                    Token.Identifier("Puddle"), Token.Comma,
                                    Token.Integer(5), Token.Comma,
                                    Token.Identifier("PATHING"),Token.Pipe,Token.Identifier("HOLOWALL"), Token.Comma,
                                    Token.Integer(3),
                                }),
                            Expression.ValueList(
                                name:Maybe<Identifier>.Nothing,
                                oldnum:Maybe<ushort>.Nothing,
                                qualifiers:Enumerable.Empty<Token>(),
                                values:new[]
                                {
                                    Token.Integer(225), Token.Comma,
                                    Token.Identifier("Puddle"), Token.Comma,
                                    Token.Integer(6), Token.Comma,
                                    Token.Identifier("AMBUSH"),Token.Pipe,Token.Identifier("PATHING"), Token.Comma,
                                    Token.Integer(4),
                                }),
                        })
                    });

            IThingMapping mapping = translator.LookupThingMapping(23);            
            mapping.Should().BeOfType<ThingTemplate>();

            var thingTemplate = (ThingTemplate)mapping;

            thingTemplate.Type.Should().Be("Puddle");
            thingTemplate.Angles.Should().Be(4);
            thingTemplate.Pathing.Should().BeFalse();
            thingTemplate.Holowall.Should().BeTrue();
            thingTemplate.Ambush.Should().BeFalse();
            thingTemplate.Minskill.Should().Be(2);

            mapping = translator.LookupThingMapping(124);     
            mapping.Should().BeOfType<ThingTemplate>();

            thingTemplate = (ThingTemplate)mapping;

            thingTemplate.Type.Should().Be("Puddle");
            thingTemplate.Angles.Should().Be(5);
            thingTemplate.Pathing.Should().BeTrue();
            thingTemplate.Holowall.Should().BeTrue();
            thingTemplate.Ambush.Should().BeFalse();
            thingTemplate.Minskill.Should().Be(3);

            mapping = translator.LookupThingMapping(225);     
            mapping.Should().BeOfType<ThingTemplate>();

            thingTemplate = (ThingTemplate)mapping;

            thingTemplate.Type.Should().Be("Puddle");
            thingTemplate.Angles.Should().Be(6);
            thingTemplate.Pathing.Should().BeTrue();
            thingTemplate.Holowall.Should().BeFalse();
            thingTemplate.Ambush.Should().BeTrue();
            thingTemplate.Minskill.Should().Be(4);
        }

        #endregion Things section

        #region Flats section

        [Fact]
        public void ShouldParseFlatsSection()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Block(new Identifier("flats"), new []
                {
                    Expression.ValueList(
                        name:new Identifier("ceiling").ToMaybe(),
                        oldnum:Maybe<ushort>.Nothing,
                        qualifiers:Enumerable.Empty<Token>(),
                        values:new []
                        {

                        Token.String("flat1"),
                        Token.String("flat2"),
                        }),
                    Expression.ValueList(
                        name:new Identifier("floor").ToMaybe(),
                        oldnum:Maybe<ushort>.Nothing,
                        qualifiers:Enumerable.Empty<Token>(),
                        values:new []
                        {

                        Token.String("flat3"),
                        Token.String("flat4"),
                        }),
                })
            });

            translator.FlatMappings.Ceilings.Should().BeEquivalentTo(new[] { "flat1", "flat2" });
            translator.FlatMappings.Floors.Should().BeEquivalentTo(new[] { "flat3", "flat4" });
        }

        #endregion Flats section
    }
}