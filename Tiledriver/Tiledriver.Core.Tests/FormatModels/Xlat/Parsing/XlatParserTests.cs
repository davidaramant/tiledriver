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

            Assert.That(translator.EnableLightLevels,Is.True,"Did not parse 'enable lightlevels'");
        }
    }
}