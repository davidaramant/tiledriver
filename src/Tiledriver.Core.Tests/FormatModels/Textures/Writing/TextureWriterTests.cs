// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Textures.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Textures.Writing;

public sealed class TextureWriterTests
{
	[Fact]
	public void ShouldWriteLadderTexture()
	{
		var texture = new CompositeTexture(
			"ALADDER2",
			72,
			256,
			Patches: ImmutableArray.Create(
				new Patch("RW45_1", 0, 0, Rotate: PatchRotation.Rotate90),
				new Patch("RW45_1", 0, 64, Rotate: PatchRotation.Rotate90),
				new Patch("RW45_1", 0, 128, Rotate: PatchRotation.Rotate90),
				new Patch("RW45_1", 0, 172, Rotate: PatchRotation.Rotate90)
			)
		);

		var actual = GetText(texture);

		var expected = Assemble(
			"Texture ALADDER2, 72, 256",
			"{",
			"\tPatch RW45_1, 0, 0",
			"\t{",
			"\t\tRotate 90",
			"\t}",
			"\tPatch RW45_1, 0, 64",
			"\t{",
			"\t\tRotate 90",
			"\t}",
			"\tPatch RW45_1, 0, 128",
			"\t{",
			"\t\tRotate 90",
			"\t}",
			"\tPatch RW45_1, 0, 172",
			"\t{",
			"\t\tRotate 90",
			"\t}",
			"}"
		);

		actual.Should().Be(expected);
	}

	[Fact]
	public void ShouldWriteExampleTexture2()
	{
		var texture = new CompositeTexture(
			"SWEXPLUP",
			512,
			512,
			XScale: 4,
			YScale: 4,
			Patches: ImmutableArray.Create(
				new Patch("AG_512_2", 0, 0),
				new Patch("MSW1_UP", 64, 288, Style: RenderStyle.CopyAlpha)
			)
		);

		var actual = GetText(texture);

		var expected = Assemble(
			"Texture SWEXPLUP, 512, 512",
			"{",
			"\tXScale 4.0",
			"\tYScale 4.0",
			"\tPatch AG_512_2, 0, 0",
			"\tPatch MSW1_UP, 64, 288",
			"\t{",
			"\t\tStyle CopyAlpha",
			"\t}",
			"}"
		);

		actual.Should().Be(expected);
	}

	[Fact]
	public void ShouldWriteTextureOffset()
	{
		var texture = new CompositeTexture(
			"Name",
			10,
			10,
			Offset: new TextureOffset(5, 6),
			Patches: ImmutableArray<Patch>.Empty
		);

		var actual = GetText(texture);

		var expected = Assemble("Texture Name, 10, 10", "{", "\tOffset 5, 6", "}");

		actual.Should().Be(expected);
	}

	[Fact]
	public void ShouldWritePredefinedTranslation()
	{
		var texture = new CompositeTexture(
			"Name",
			10,
			10,
			Patches: ImmutableArray.Create(new Patch("Name2", 0, 0, Translation: new Translation.Blue()))
		);

		var actual = GetText(texture);

		var expected = Assemble(
			"Texture Name, 10, 10",
			"{",
			"\tPatch Name2, 0, 0",
			"\t{",
			"\t\tTranslation Blue",
			"\t}",
			"}"
		);

		actual.Should().Be(expected);
	}

	[Fact]
	public void ShouldWriteCustomTranslation()
	{
		var texture = new CompositeTexture(
			"Name",
			10,
			10,
			Patches: ImmutableArray.Create(new Patch("Name2", 0, 0, Translation: new Translation.Custom("SomeString")))
		);

		var actual = GetText(texture);

		var expected = Assemble(
			"Texture Name, 10, 10",
			"{",
			"\tPatch Name2, 0, 0",
			"\t{",
			"\t\tTranslation \"SomeString\"",
			"\t}",
			"}"
		);

		actual.Should().Be(expected);
	}

	[Fact]
	public void ShouldWriteDesaturateTranslation()
	{
		var texture = new CompositeTexture(
			"Name",
			10,
			10,
			Patches: ImmutableArray.Create(new Patch("Name2", 0, 0, Translation: new Translation.Desaturate(20)))
		);

		var actual = GetText(texture);

		var expected = Assemble(
			"Texture Name, 10, 10",
			"{",
			"\tPatch Name2, 0, 0",
			"\t{",
			"\t\tTranslation Desaturate, 20",
			"\t}",
			"}"
		);

		actual.Should().Be(expected);
	}

	[Fact]
	public void ShouldNotWritePatchParentsIfNoContent()
	{
		var texture = new CompositeTexture("Name", 10, 10, Patches: ImmutableArray.Create(new Patch("Name2", 0, 0)));

		var actual = GetText(texture);

		var expected = Assemble("Texture Name, 10, 10", "{", "\tPatch Name2, 0, 0", "}");

		actual.Should().Be(expected);
	}

	private static string GetText(CompositeTexture compositeTexture)
	{
		using var ms = new MemoryStream();
		using (var writer = new StreamWriter(ms, Encoding.ASCII, leaveOpen: true))
		{
			TexturesWriter.Write(compositeTexture, writer);
		}

		ms.Position = 0;
		using var reader = new StreamReader(ms);
		return reader.ReadToEnd();
	}

	private static string Assemble(params string[] lines)
	{
		return string.Join(Environment.NewLine, lines) + Environment.NewLine;
	}
}
