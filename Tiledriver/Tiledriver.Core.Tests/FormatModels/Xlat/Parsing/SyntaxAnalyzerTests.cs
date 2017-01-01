// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Functional.Maybe;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat.Parsing;
using Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax;

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
                qualifiers: new[] { "lightlevels" });
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

        private static Expression[] Analyze(string input)
        {
            var syntaxAnalzer = new SyntaxAnalyzer();
            return syntaxAnalzer.Analyze(new XlatLexer(new StringReader(input))).ToArray();
        }

        private static void AssertExpression(
            Expression expression,
            string name = null,
            short? oldnum = null,
            IEnumerable<string> qualifiers = null,
            IEnumerable<KeyValuePair<Identifier, Token>> properties = null,
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
                (qualifiers ?? Enumerable.Empty<string>()).Select(qName => new Identifier(qName)).ToArray();
            Assert.That(expression.Qualifiers.ToArray(), Is.EquivalentTo(expectedQualifiers), "Qualifers were not as expected.");

            var expectedProperties =
                (properties ?? Enumerable.Empty<KeyValuePair<Identifier, Token>>()).ToArray();
            Assert.That(expression.GetAssignments().Count(), Is.EqualTo(expectedProperties.Length), "Incorrect number of assignments.");

            foreach (var expectedProperty in expectedProperties)
            {
                Assert.That(expression.GetValueFor(expectedProperty.Key), Is.EqualTo(expectedProperty.Value.ToMaybe()),
                    $"Incorrect value for {expectedProperty.Key}");
            }

            var expectedValues = (values ?? Enumerable.Empty<Token>()).ToArray();
            Assert.That(expression.Values.ToArray(), Is.EquivalentTo(expectedValues), "Values were not as expected.");

            Assert.That(expression.SubExpressions.Count(), Is.EqualTo(numberOfSubExpressions), "Incorrect number of sub expressions.");
        }
    }
}