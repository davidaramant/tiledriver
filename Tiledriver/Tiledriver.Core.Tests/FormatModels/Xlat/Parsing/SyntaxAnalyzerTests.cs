// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Linq;
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
            var input = @"enable lightlevels;";

            var syntaxAnalzer = new SyntaxAnalyzer();
            var result = syntaxAnalzer.Analyze(new XlatLexer(new StringReader(input))).ToArray();

            Assert.That(result, Has.Length.EqualTo(1));
            var expression = result.First();
            Assert.That(expression.Name.Value, Is.EqualTo(new Identifier("enable")));
            Assert.That(expression.Qualifiers.First(), Is.EqualTo(new Identifier("lightlevels")));
        }
    }
}