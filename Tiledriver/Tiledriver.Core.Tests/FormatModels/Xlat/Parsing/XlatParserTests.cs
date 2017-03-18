// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
using System.Text;
using Functional.Maybe;
using Moq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat;
using Tiledriver.Core.FormatModels.Xlat.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Parsing
{
    [TestFixture]
    public sealed class XlatParserTests
    {
        [Test]
        public void ShouldParseRealXlat()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "Xlat", "Parsing", "wolf3d.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                var syntaxAnalzer = new XlatSyntaxAnalyzer(Mock.Of<IResourceProvider>());
                var result = syntaxAnalzer.Analyze(lexer);
                var translator = XlatParser.Parse(result);
            }
        }

        #region Global Expressions

        [Test]
        public void ShouldEnableLightLevels()
        {
            var translator = XlatParser.Parse(new[]
            {
                Expression.Simple(
                    name: new Identifier( "enable"),
                    oldnum: Maybe<ushort>.Nothing,
                    qualifiers:new [] { Token.Identifier("lightlevels"), })
            });

            Assert.That(translator.EnableLightLevels, Is.True, "Did not parse 'enable lightlevels'");
        }

        #endregion Global Expressions

        #region Tiles section

        [Test]
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

            var mz1 = translator.TileMappings.AmbushModzones.Lookup((ushort)1).OrElse(() => new AssertionException("Did not include modzone."));
            Assert.That(mz1.Fillzone, Is.False, "Did not parse modzone without ambush flag.");

            var mz2 = translator.TileMappings.AmbushModzones.Lookup((ushort)2).OrElse(() => new AssertionException("Did not include modzone."));
            Assert.That(mz2.Fillzone, Is.True, "Did not parse modzone with ambush flag.");
        }

        [Test]
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

            var trigger = translator.TileMappings.ChangeTriggerModzones.Lookup((ushort)123).OrElse(() => new AssertionException("Did not include modzone."));

            Assert.That(trigger.Fillzone, Is.True, "Did not parse Fillzone");
            Assert.That(trigger.Action, Is.EqualTo("someoriginalaction"), "Did not parse action.");
            Assert.That(trigger.TriggerTemplate.Action, Is.EqualTo("someaction"), "Did not parse trigger action.");
            Assert.That(trigger.TriggerTemplate.ActivateEast, Is.False, "Did not parse trigger activateeast.");
        }

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

            var tile = translator.TileMappings.Tiles.Lookup((ushort)123).OrElse(() => new AssertionException("Did not include tile."));

            Assert.That(tile.TextureEast, Is.EqualTo("tex1"), "Did not set Texture East");
            Assert.That(tile.TextureNorth, Is.EqualTo("tex2"), "Did not set Texture North");
            Assert.That(tile.TextureSouth, Is.EqualTo("tex3"), "Did not set Texture South");
            Assert.That(tile.TextureWest, Is.EqualTo("tex4"), "Did not set Texture West");
        }

        [Test]
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

            var trigger = translator.TileMappings.TriggerTemplates.Lookup((ushort)123).OrElse(() => new AssertionException("Did not include trigger."));

            Assert.That(trigger.Action, Is.EqualTo("someaction"), "Did not set action");
            Assert.That(trigger.ActivateEast, Is.False, "Did not set Activate East");
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
                        qualifiers:Enumerable.Empty<Token>(),
                        values:Enumerable.Empty<Token>()),
                })
            });

            Assert.That(translator.TileMappings.Zones.ContainsKey(123), Is.True, "Did not parse sound zone.");
        }

        #endregion Tiles section

        #region Things section

        [Test]
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

            Assert.That(translator.LookupThingMapping(123), Is.TypeOf<Elevator>(), "Did not parse elevator");
        }

        [Test]
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

            IThingMapping mapping = null;
            Assert.DoesNotThrow(()=>mapping = translator.LookupThingMapping(123),"Did not have mapping.");
            Assert.That(mapping,Is.TypeOf<TriggerTemplate>(),"Wrong type parsed.");

            var trigger = (TriggerTemplate)mapping;

            Assert.That(trigger.OldNum, Is.EqualTo(123), "Did not set oldNum");
            Assert.That(trigger.Action, Is.EqualTo("someaction"), "Did not set action");
            Assert.That(trigger.ActivateEast, Is.False, "Did not set Activate East");
        }

        [Test]
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

            IThingMapping mapping = null;
            Assert.DoesNotThrow(() => mapping = translator.LookupThingMapping(23), "Did not have mapping.");
            Assert.That(mapping, Is.TypeOf<ThingTemplate>(), "Wrong type parsed.");

            var thingTemplate = (ThingTemplate)mapping;

            Assert.That(thingTemplate.OldNum, Is.EqualTo(23), "Did not part OldNum");
            Assert.That(thingTemplate.Actor, Is.EqualTo("Puddle"), "Did not parse actor.");
            Assert.That(thingTemplate.Angles, Is.EqualTo(4), "Did not parse angles.");
            Assert.That(thingTemplate.Pathing, Is.False, "Did not parse pathing.");
            Assert.That(thingTemplate.Holowall, Is.False, "Did not parse holowall.");
            Assert.That(thingTemplate.Minskill, Is.EqualTo(2), "Did not parse minskill.");
        }

        [Test]
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

            IThingMapping mapping = null;
            Assert.DoesNotThrow(() => mapping = translator.LookupThingMapping(23), "Did not have mapping.");
            Assert.That(mapping, Is.TypeOf<ThingTemplate>(), "Wrong type parsed.");

            var thingTemplate = (ThingTemplate)mapping;

            Assert.That(thingTemplate.OldNum, Is.EqualTo(23), "Did not part OldNum");
            Assert.That(thingTemplate.Actor, Is.EqualTo("$Puddle"), "Did not parse actor.");
            Assert.That(thingTemplate.Angles, Is.EqualTo(4), "Did not parse angles.");
            Assert.That(thingTemplate.Pathing, Is.False, "Did not parse pathing.");
            Assert.That(thingTemplate.Holowall, Is.False, "Did not parse holowall.");
            Assert.That(thingTemplate.Minskill, Is.EqualTo(2), "Did not parse minskill.");
        }


        [Test]
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

            IThingMapping mapping = null;
            Assert.DoesNotThrow(() => mapping = translator.LookupThingMapping(23), "Did not have mapping.");
            Assert.That(mapping, Is.TypeOf<ThingTemplate>(), "Wrong type parsed.");

            var thingTemplate = (ThingTemplate)mapping;

            Assert.That(thingTemplate.Actor, Is.EqualTo("Puddle"), "Did not parse actor.");
            Assert.That(thingTemplate.Angles, Is.EqualTo(4), "Did not parse angles.");
            Assert.That(thingTemplate.Pathing, Is.False, "Did not parse pathing.");
            Assert.That(thingTemplate.Holowall, Is.True, "Did not parse holowall.");
            Assert.That(thingTemplate.Ambush, Is.False, "Did not parse ambush.");
            Assert.That(thingTemplate.Minskill, Is.EqualTo(2), "Did not parse minskill.");

            Assert.DoesNotThrow(() => mapping = translator.LookupThingMapping(124), "Did not have mapping.");
            Assert.That(mapping, Is.TypeOf<ThingTemplate>(), "Wrong type parsed.");

            thingTemplate = (ThingTemplate)mapping;

            Assert.That(thingTemplate.Actor, Is.EqualTo("Puddle"), "Did not parse actor.");
            Assert.That(thingTemplate.Angles, Is.EqualTo(5), "Did not parse angles.");
            Assert.That(thingTemplate.Pathing, Is.True, "Did not parse pathing.");
            Assert.That(thingTemplate.Holowall, Is.True, "Did not parse holowall.");
            Assert.That(thingTemplate.Ambush, Is.False, "Did not parse ambush.");
            Assert.That(thingTemplate.Minskill, Is.EqualTo(3), "Did not parse minskill.");

            Assert.DoesNotThrow(() => mapping = translator.LookupThingMapping(225), "Did not have mapping.");
            Assert.That(mapping, Is.TypeOf<ThingTemplate>(), "Wrong type parsed.");

            thingTemplate = (ThingTemplate)mapping;

            Assert.That(thingTemplate.Actor, Is.EqualTo("Puddle"), "Did not parse actor.");
            Assert.That(thingTemplate.Angles, Is.EqualTo(6), "Did not parse angles.");
            Assert.That(thingTemplate.Pathing, Is.True, "Did not parse pathing.");
            Assert.That(thingTemplate.Holowall, Is.False, "Did not parse holowall.");
            Assert.That(thingTemplate.Ambush, Is.True, "Did not parse ambush.");
            Assert.That(thingTemplate.Minskill, Is.EqualTo(4), "Did not parse minskill.");
        }

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

            Assert.That(translator.FlatMappings.Ceilings, Is.EquivalentTo(new[] { "flat1", "flat2" }), "Did not parse ceiling.");
            Assert.That(translator.FlatMappings.Floors, Is.EquivalentTo(new[] { "flat3", "flat4" }), "Did not parse floor.");
        }

        #endregion Flats section
    }
}