// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using FluentAssertions;
using Tiledriver.Core.FormatModels.GameMaps;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.GameMaps;

public sealed class ExpanderTests
{
	[Fact]
	public void ShouldPassThroughRlewBytesWithNoMarker()
	{
		var input = Enumerable.Repeat<byte>(0xFF, 40).ToArray();
		var expanded = Expander.DecompressRlew(0xABCD, input, input.Length);
		expanded.Should().BeEquivalentTo(input, "array should not have been mutated");
	}

	[Fact]
	public void ShouldExpandASimpleRlewSubstitution()
	{
		var input = new byte[] { 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF };
		var expected = Enumerable.Repeat<byte>(0xFF, 16).ToArray();
		var expanded = Expander.DecompressRlew(0xABCD, input, expected.Length);
		expanded.Should().BeEquivalentTo(expected, "array should have been expanded");
	}

	[Fact]
	public void ShouldExpandRlewWithPrefix()
	{
		var input = new byte[] { 0x1A, 0x2B, 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF };
		var expected = new byte[] { 0x1A, 0x2B }
			.Concat(Enumerable.Repeat<byte>(0xFF, 16))
			.ToArray();
		var expanded = Expander.DecompressRlew(0xABCD, input, expected.Length);
		expanded.Should().BeEquivalentTo(expected, "array should have been expanded");
	}

	[Fact]
	public void ShouldExpandRlewWithAppendix()
	{
		var input = new byte[] { 0xCD, 0xAB, 0x08, 0x00, 0xFF, 0xFF, 0x1A, 0x2B };
		var expected = Enumerable.Repeat<byte>(0xFF, 16).Concat(new byte[] { 0x1A, 0x2B }).ToArray();
		var expanded = Expander.DecompressRlew(0xABCD, input, expected.Length);
		expanded.Should().BeEquivalentTo(expected, "array should have been expanded");
	}

	[Fact]
	public void ShouldExpandMultipleRlewSections()
	{
		var input = new byte[]
		{
			0x55,
			0x66,
			0xCD,
			0xAB,
			0x08,
			0x00,
			0xFF,
			0xFF,
			0x1A,
			0x2B,
			0xCD,
			0xAB,
			0x04,
			0x00,
			0x11,
			0x22,
			0x33,
			0x44,
		};
		var expected = new byte[] { 0x55, 0x66 }
			.Concat(Repeat(new byte[] { 0xFF, 0xFF }, 8))
			.Concat(new byte[] { 0x1A, 0x2B })
			.Concat(Repeat(new byte[] { 0x11, 0x22 }, 4))
			.Concat(new byte[] { 0x33, 0x44 })
			.ToArray();

		var expanded = Expander.DecompressRlew(0xABCD, input, expected.Length);
		expanded.Should().BeEquivalentTo(expected, "array should have been expanded");
	}

	[Fact]
	public void ShouldUnCarmackSimpleData()
	{
		var input = new byte[] { 0x08, 0x00, 0x00, 0x20, 0xCD, 0xAB, 0x00, 0x10, 0x00, 0x00 };
		var expected = new byte[] { 0x00, 0x20, 0xCD, 0xAB, 0x00, 0x10, 0x00, 0x00 };
		var output = Expander.DecompressCarmack(input);
		output.Should().BeEquivalentTo(expected, "array should have been decompressed.");
	}

	private static byte[] Repeat(byte[] sequence, int times)
	{
		var result = new byte[times * sequence.Length];

		for (int i = 0; i < times; i++)
		{
			Buffer.BlockCopy(sequence, 0, result, i * sequence.Length, sequence.Length);
		}

		return result;
	}
}
