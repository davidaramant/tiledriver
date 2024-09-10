// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using FluentAssertions;
using Moq;
using Tiledriver.Core.FormatModels;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Common.Reading;

public sealed class TokenSourceTests
{
	[Fact]
	public void ShouldReturnAllTokensInNormalFile()
	{
		var tokens = new Token[]
		{
			new IdentifierToken(FilePosition.StartOfFile, "id1"),
			new IdentifierToken(FilePosition.StartOfFile, "id2"),
		};

		var stream = new TokenSource(
			tokens,
			Mock.Of<IResourceProvider>(),
			reader => throw new Exception("Shouldn't be called")
		);

		var actualTokens = stream.ToArray();

		actualTokens.Should().BeEquivalentTo(tokens);
	}

	[Fact]
	public void ShouldHandleIncludeStatement()
	{
		var firstFileTokens = new Token[]
		{
			new IdentifierToken(FilePosition.StartOfFile, "include"),
			new StringToken(FilePosition.StartOfFile, "otherFile"),
			new IdentifierToken(FilePosition.StartOfFile, "id1"),
			new IdentifierToken(FilePosition.StartOfFile, "id2"),
		};

		using var otherFileStream = new MemoryStream();
		using (var writer = new StreamWriter(otherFileStream, leaveOpen: true))
		{
			writer.Write("otherId1 otherId2");
		}
		otherFileStream.Position = 0;

		var mockProvider = new Mock<IResourceProvider>();
		mockProvider.Setup(_ => _.Lookup("otherFile")).Returns(otherFileStream);

		var stream = new TokenSource(firstFileTokens, mockProvider.Object, reader => new UnifiedLexer(reader));

		var actualTokens = stream.ToArray();

		actualTokens
			.Should()
			.BeEquivalentTo(
				new Token[]
				{
					new IdentifierToken(FilePosition.StartOfFile, "otherId1"),
					new IdentifierToken(new FilePosition(1, 10), "otherId2"),
					new IdentifierToken(FilePosition.StartOfFile, "id1"),
					new IdentifierToken(FilePosition.StartOfFile, "id2"),
				}
			);
	}
}
