// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Text;
using Shouldly;
using Tiledriver.Core.DemoMaps.Wolf3D;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;
using Tiledriver.Core.FormatModels.Uwmf.Reading;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;
using Tiledriver.Core.FormatModels.Uwmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Reading;

public sealed class UwmfParserTests
{
	[Fact]
	public void ShouldParseAssignment()
	{
		var tokenStream = new Token[]
		{
			new IdentifierToken(FilePosition.StartOfFile, new Identifier("id")),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 5),
			new SemicolonToken(FilePosition.StartOfFile),
		};

		var results = UwmfParser.Parse(tokenStream).ToArray();
		results.Length.ShouldBe(1);
		results[0].ShouldBeOfType<Assignment>();
	}

	[Fact]
	public void ShouldParseEmptyBlock()
	{
		var tokenStream = new Token[]
		{
			new IdentifierToken(FilePosition.StartOfFile, new Identifier("blockName")),
			new OpenBraceToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile),
		};

		var results = UwmfParser.Parse(tokenStream).ToArray();
		results.Length.ShouldBe(1);
		results[0].ShouldBeOfType<Block>().Fields.ShouldBeEmpty();
	}

	[Fact]
	public void ShouldParseBlock()
	{
		var tokenStream = new Token[]
		{
			new IdentifierToken(FilePosition.StartOfFile, new Identifier("blockName")),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IdentifierToken(FilePosition.StartOfFile, new Identifier("id")),
			new EqualsToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 5),
			new SemicolonToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile),
		};

		var results = UwmfParser.Parse(tokenStream).ToArray();
		results.Length.ShouldBe(1);
		results[0].ShouldBeOfType<Block>().Fields.Length.ShouldBe(1);
	}

	[Fact]
	public void ShouldParseIntTupleList()
	{
		var tokenStream = new Token[]
		{
			new IdentifierToken(FilePosition.StartOfFile, new Identifier("planemap")),
			new OpenBraceToken(FilePosition.StartOfFile),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 1),
			new CommaToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 2),
			new CloseBraceToken(FilePosition.StartOfFile),
			new CommaToken(FilePosition.StartOfFile),
			new OpenBraceToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 1),
			new CommaToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 2),
			new CommaToken(FilePosition.StartOfFile),
			new IntegerToken(FilePosition.StartOfFile, 3),
			new CloseBraceToken(FilePosition.StartOfFile),
			new CloseBraceToken(FilePosition.StartOfFile),
		};

		var results = UwmfParser.Parse(tokenStream).ToArray();
		results.Length.ShouldBe(1);
		results[0].ShouldBeOfType<IntTupleBlock>().Tuples.Length.ShouldBe(2);
	}

	[Fact]
	public void ShouldHandleParsingDemoMap()
	{
		var map = ThingDemoMap.Create();

		using var stream = new MemoryStream();
		map.WriteTo(stream);

		stream.Position = 0;

		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = new UnifiedLexer(textReader);
		var result = UwmfParser.Parse(lexer.Scan()).ToArray();
	}

	[Fact]
	public void ShouldHandleParsingTestFile()
	{
		using var stream = TestFile.Uwmf.TEXTMAP;

		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = new UnifiedLexer(textReader);
		var result = UwmfParser.Parse(lexer.Scan()).ToArray();
	}
}
