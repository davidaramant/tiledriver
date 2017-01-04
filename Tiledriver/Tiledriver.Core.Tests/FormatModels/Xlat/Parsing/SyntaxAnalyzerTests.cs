// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Functional.Maybe;
using Moq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Parsing
{
    [TestFixture]
    public sealed class SyntaxAnalyzerTests
    {
        [Test]
        public void ShouldParseGlobalExpression()
        {
            var result = Analyze(@"enable lightlevels;");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "enable",
                qualifiers: new[] { Token.Identifier("lightlevels") });
        }

        [Test]
        public void ShouldParseEmptyBlock()
        {
            var result = Analyze(@"block { }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 0);
        }

        [Test]
        public void ShouldParseBlockWithSimpleExpression()
        {
            var result = Analyze(@"block { thing with stuff; }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                qualifiers: new[] { Token.Identifier("with"), Token.Identifier("stuff") });
        }

        [Test]
        public void ShouldParseBlockWithSimpleExpressionWithOldNum()
        {
            var result = Analyze(@"block { thing 44 with stuff; }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                oldnum: 44,
                qualifiers: new[] { Token.Identifier("with"), Token.Identifier("stuff") });
        }

        [Test]
        public void ShouldParseBlockWithExpressionWithEmptyList()
        {
            var result = Analyze(@"block { thing 44 {} }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                oldnum: 44);
        }

        [Test]
        public void ShouldParseBlockWithExpressionWithStringListOfSingleValue()
        {
            var result = Analyze("block { thing { \"string\" } }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                values: new[] { Token.String("string") });
        }

        [Test]
        public void ShouldParseBlockWithExpressionWithStringListOfMultipleValues()
        {
            var result = Analyze("block { thing { \"string\", \"string2\" } }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                values: new[] { Token.String("string"), Token.String("string2") });
        }

        [Test]
        public void ShouldParseBlockWithComplicatedExpression()
        {
            var result = Analyze("block { modzone 107 changetrigger \"Exit_Normal\"" +
                                "	{" +
                                "		action = \"Exit_Secret\";" +
                                "		playeruse = true;" +
                                "		activatenorth = false;" +
                                "		activatesouth = false;" +
                                "	} }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "modzone",
                oldnum: 107,
                qualifiers: new[] { Token.Identifier("changetrigger"), Token.String("Exit_Normal") },
                properties: new[]
                {
                    new Assignment("action",Token.String("Exit_Secret")),
                    new Assignment("playeruse",Token.BooleanTrue),
                    new Assignment("activatenorth",Token.BooleanFalse),
                    new Assignment("activatesouth",Token.BooleanFalse),
                });
        }

        [Test]
        public void ShouldParseBlockWithValueLists()
        {
            var result = Analyze(
                "block { " +
                "	{19,  $Player1Start,     4, 0, 0}" +
                "	{108, Guard,             4, HOLOWALL, 1}" +
                "	{112, Guard,             4, PATHING|HOLOWALL, 1}" +
                "}");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 3);

            AssertExpression(expression.SubExpressions.ElementAt(0),
                values: new[]
                {
                    Token.Integer(19), Token.Comma,
                    Token.Meta, Token.Identifier("Player1Start"), Token.Comma,
                    Token.Integer(4), Token.Comma,
                    Token.Integer(0), Token.Comma,
                    Token.Integer(0)
                });

            AssertExpression(expression.SubExpressions.ElementAt(1),
                values: new[]
                {
                    Token.Integer(108), Token.Comma,
                    Token.Identifier("Guard"), Token.Comma,
                    Token.Integer(4), Token.Comma,
                    Token.Identifier("HOLOWALL"), Token.Comma,
                    Token.Integer(1)
                });

            AssertExpression(expression.SubExpressions.ElementAt(2),
                values: new[]
                {
                    Token.Integer(112), Token.Comma,
                    Token.Identifier("Guard"), Token.Comma,
                    Token.Integer(4), Token.Comma,
                    Token.Identifier("PATHING"), Token.Pipe, Token.Identifier("HOLOWALL"), Token.Comma,
                    Token.Integer(1)
                });
        }

        [Test]
        public void ShouldParseBlockWithAssignment()
        {
            var result = Analyze("block { thing { dog = 1; } }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                properties: new[]
                {
                    new Assignment(new Identifier("dog"), Token.Integer(1) )
                });
        }

        [Test]
        public void ShouldParseBlockWithAssignments()
        {
            var result = Analyze("block { thing { dog = 1; cat = \"meow\"; cow = false; } }");

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                properties: new[]
                {
                    new Assignment(new Identifier("dog"), Token.Integer(1) ),
                    new Assignment(new Identifier("cat"), Token.String("meow") ),
                    new Assignment(new Identifier("cow"), Token.BooleanFalse )
                });
        }

        [Test]
        public void ShouldAnalyzeRealXlat()
        {
            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "Xlat", "Parsing", "wolf3d.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                var syntaxAnalzer = new XlatSyntaxAnalyzer(Mock.Of<IResourceProvider>());
                var result = syntaxAnalzer.Analyze(lexer).ToArray();
            }
        }

        [Test]
        public void ShouldAnalyzeRealXlatWithInclude()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider.
                Setup(_ => _.Lookup(It.IsAny<string>()))
                .Returns( File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "Xlat", "Parsing", "wolf3d.txt")));

            using (var stream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "FormatModels", "Xlat", "Parsing", "spear.txt")))
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                var syntaxAnalzer = new XlatSyntaxAnalyzer(mockProvider.Object);
                var result = syntaxAnalzer.Analyze(lexer).ToArray();
            }
        }

        private static Expression[] Analyze(string input)
        {
            var syntaxAnalzer = new XlatSyntaxAnalyzer(Mock.Of<IResourceProvider>());
            return syntaxAnalzer.Analyze(new XlatLexer(new StringReader(input))).ToArray();
        }

        private static void AssertExpression(
            Expression expression,
            string name = null,
            short? oldnum = null,
            IEnumerable<Token> qualifiers = null,
            IEnumerable<Assignment> properties = null,
            IEnumerable<Token> values = null,
            int numberOfSubExpressions = 0)
        {
            if (name != null)
            {
                Assert.That(expression.Name.HasValue, Is.True, "Should have included a name.");
                Assert.That(expression.Name.Value, Is.EqualTo(new Identifier(name)), "Wrong name.");
            }
            else
            {
                Assert.That(expression.Name.HasValue, Is.False, "Not expecting a name.");
            }

            if (oldnum != null)
            {
                Assert.That(expression.Oldnum.HasValue, Is.True, "Should have included an old num.");
                Assert.That(expression.Oldnum.Value, Is.EqualTo(oldnum.Value), "Wrong old num.");
            }
            else
            {
                Assert.That(expression.Oldnum.HasValue, Is.False, "Not expecting an old num.");
            }

            var expectedQualifiers =
                (qualifiers ?? Enumerable.Empty<Token>()).ToArray();
            Assert.That(expression.Qualifiers.ToArray(), Is.EquivalentTo(expectedQualifiers), "Qualifers were not as expected.");

            var expectedProperties =
                (properties ?? Enumerable.Empty<Assignment>()).ToArray();
            Assert.That(expression.GetAssignments().Count(), Is.EqualTo(expectedProperties.Length), "Incorrect number of assignments.");

            foreach (var expectedProperty in expectedProperties)
            {
                Assert.That(expression.GetValueFor(expectedProperty.Name), Is.EqualTo(expectedProperty.Value.ToMaybe()),
                    $"Incorrect value for {expectedProperty.Name}");
            }

            var expectedValues = (values ?? Enumerable.Empty<Token>()).ToArray();
            Assert.That(expression.Values.ToArray(), Is.EquivalentTo(expectedValues), "Values were not as expected.");

            Assert.That(expression.SubExpressions.Count(), Is.EqualTo(numberOfSubExpressions), "Incorrect number of sub expressions.");
        }
    }
}