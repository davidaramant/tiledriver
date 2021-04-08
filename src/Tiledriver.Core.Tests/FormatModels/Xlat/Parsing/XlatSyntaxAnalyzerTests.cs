// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Functional.Maybe;
using Moq;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat.Parsing;

namespace Tiledriver.Core.Tests.FormatModels.Xlat.Parsing
{
    public sealed class XlatSyntaxAnalyzerTests
    {
        [Fact]
        public void ShouldParseGlobalExpression()
        {
            var result = Analyze(@"enable lightlevels;");

            result.Should().HaveCount(1);
            var expression = result.First();

            AssertExpression(expression,
                name: "enable",
                qualifiers: new[] { Token.Identifier("lightlevels") });
        }

        [Fact]
        public void ShouldParseEmptyBlock()
        {
            var result = Analyze(@"block { }");

            result.Should().HaveCount(1);
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 0);
        }

        [Fact]
        public void ShouldParseBlockWithSimpleExpression()
        {
            var result = Analyze(@"block { thing with stuff; }");

            result.Should().HaveCount(1);
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                qualifiers: new[] { Token.Identifier("with"), Token.Identifier("stuff") });
        }

        [Fact]
        public void ShouldParseBlockWithSimpleExpressionWithOldNum()
        {
            var result = Analyze(@"block { thing 44 with stuff; }");

            result.Should().HaveCount(1);
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

        [Fact]
        public void ShouldParseBlockWithExpressionWithEmptyList()
        {
            var result = Analyze(@"block { thing 44 {} }");

            result.Should().HaveCount(1);
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                oldnum: 44);
        }

        [Fact]
        public void ShouldParseBlockWithExpressionWithStringListOfSingleValue()
        {
            var result = Analyze("block { thing { \"string\" } }");

            result.Should().HaveCount(1);
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                values: new[] { Token.String("string") });
        }

        [Fact]
        public void ShouldParseBlockWithExpressionWithStringListOfMultipleValues()
        {
            var result = Analyze("block { thing { \"string\", \"string2\" } }");

            result.Should().HaveCount(1);
            var expression = result.First();

            AssertExpression(expression,
                name: "block",
                numberOfSubExpressions: 1);

            var subExp = expression.SubExpressions.First();
            AssertExpression(subExp,
                name: "thing",
                values: new[] { Token.String("string"), Token.String("string2") });
        }

        [Fact]
        public void ShouldParseBlockWithComplicatedExpression()
        {
            var result = Analyze("block { modzone 107 changetrigger \"Exit_Normal\"" +
                                "	{" +
                                "		action = \"Exit_Secret\";" +
                                "		playeruse = true;" +
                                "		activatenorth = false;" +
                                "		activatesouth = false;" +
                                "	} }");

            result.Should().HaveCount(1);
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

        [Fact]
        public void ShouldParseBlockWithValueLists()
        {
            var result = Analyze(
                "block { " +
                "	{19,  $Player1Start,     4, 0, 0}" +
                "	{108, Guard,             4, HOLOWALL, 1}" +
                "	{112, Guard,             4, PATHING|HOLOWALL, 1}" +
                "}");

            result.Should().HaveCount(1);
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

        [Fact]
        public void ShouldParseBlockWithAssignment()
        {
            var result = Analyze("block { thing { dog = 1; } }");

            result.Should().HaveCount(1);
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

        [Fact]
        public void ShouldParseBlockWithAssignments()
        {
            var result = Analyze("block { thing { dog = 1; cat = \"meow\"; cow = false; } }");

            result.Should().HaveCount(1);
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

        [Fact]
        public void ShouldAnalyzeRealXlat()
        {
            using (var stream = TestFile.Xlat.wolf3d)
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                var lexer = new XlatLexer(textReader);
                var syntaxAnalzer = new XlatSyntaxAnalyzer(Mock.Of<IResourceProvider>());
                var result = syntaxAnalzer.Analyze(lexer).ToArray();
            }
        }

        [Fact]
        public void ShouldAnalyzeRealXlatWithInclude()
        {
            var mockProvider = new Mock<IResourceProvider>();
            mockProvider.
                Setup(_ => _.Lookup(It.IsAny<string>()))
                .Returns(TestFile.Xlat.wolf3d);

            using (var stream = TestFile.Xlat.spear)
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
                expression.Name.HasValue.Should().BeTrue();
                expression.Name.Value.Should().Be(new Identifier(name));
            }
            else
            {
                expression.Name.HasValue.Should().BeFalse();
            }

            if (oldnum != null)
            {
                expression.Oldnum.HasValue.Should().BeTrue();
                expression.Oldnum.Value.Should().Be((ushort)oldnum.Value);
            }
            else
            {
                expression.Oldnum.HasValue.Should().BeFalse();
            }

            var expectedQualifiers =
                (qualifiers ?? Enumerable.Empty<Token>()).ToArray();
            expression.Qualifiers.ToArray().Should().BeEquivalentTo(expectedQualifiers);

            var expectedProperties =
                (properties ?? Enumerable.Empty<Assignment>()).ToArray();
            expression.GetAssignments().Count().Should().Be(expectedProperties.Length);

            foreach (var expectedProperty in expectedProperties)
            {
                expression.GetValueFor(expectedProperty.Name).Should().Be(expectedProperty.Value.ToMaybe());
            }

            var expectedValues = (values ?? Enumerable.Empty<Token>()).ToArray();
            expression.Values.ToArray().Should().BeEquivalentTo(expectedValues);

            expression.SubExpressions.Count().Should().Be(numberOfSubExpressions);
        }
    }
}