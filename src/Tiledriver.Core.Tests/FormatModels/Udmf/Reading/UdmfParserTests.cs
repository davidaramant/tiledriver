// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Text;
using Shouldly;
using Tiledriver.Core.DemoMaps.Doom;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;
using Tiledriver.Core.FormatModels.Udmf.Reading;
using Tiledriver.Core.FormatModels.Udmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Udmf.Reading;

public sealed class UdmfParserTests
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

		var results = UdmfParser.Parse(tokenStream).ToArray();
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

		var results = UdmfParser.Parse(tokenStream).ToArray();
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

		var results = UdmfParser.Parse(tokenStream).ToArray();
		results.Length.ShouldBe(1);
		results[0].ShouldBeOfType<Block>().Fields.Length.ShouldBe(1);
	}

	[Fact]
	public void ShouldHandleParsingDemoMap()
	{
		var map = DemoMap.Create();

		using var stream = new MemoryStream();
		map.WriteTo(stream);

		stream.Position = 0;

		using var textReader = new StreamReader(stream, Encoding.ASCII);
		var lexer = new UnifiedLexer(textReader);
		var result = UdmfParser.Parse(lexer.Scan()).ToArray();
	}
}
