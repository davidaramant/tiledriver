// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tiledriver.Core.FormatModels.Textures.Writing;

public static class TexturesWriter
{
	public static void Write(IEnumerable<CompositeTexture> textures, Stream outputStream)
	{
		using var writer = new StreamWriter(outputStream, Encoding.ASCII, leaveOpen: true);

		foreach (var texture in textures)
		{
			Write(texture, writer);
		}
	}

	public static void Write(CompositeTexture texture, StreamWriter writer)
	{
		var output = new WriterUtil();

		var optional = texture.Optional ? "optional " : "";

		output = output
			.Line($"{texture.Namespace} {optional}{texture.Name}, {texture.Width}, {texture.Height}")
			.OpenBrace()
			.OptionalLine("XScale", texture.XScale, 1)
			.OptionalLine("YScale", texture.YScale, 1)
			.OptionalLine(
				nameof(texture.Offset),
				texture.Offset,
				new TextureOffset(),
				offset => $"{offset.Horizontal}, {offset.Vertical}"
			)
			.Flag(nameof(texture.WorldPanning), texture.WorldPanning)
			.Flag(nameof(texture.NoDecals), texture.NoDecals);

		foreach (var patch in texture.Patches)
		{
			static string FormatTranslation(Translation? translation)
			{
				return translation switch
				{
					Translation.Desaturate d => $"Desaturate, {d.Amount}",
					Translation.Custom c => $"\"{c.Block}\"",
					null => throw new Exception("Should not allow null here"),
					_ => translation.GetType().Name,
				};
			}

			output = output
				.Line($"{patch.Namespace} {patch.Name}, {patch.XOrigin}, {patch.YOrigin}")
				.OpenBrace()
				.Flag(nameof(patch.FlipX), patch.FlipX)
				.Flag(nameof(patch.FlipY), patch.FlipY)
				.Flag(nameof(patch.UseOffsets), patch.UseOffsets)
				.OptionalLine(
					nameof(patch.Rotate),
					patch.Rotate,
					PatchRotation.None,
					rotation => ((int)rotation).ToString()
				)
				.OptionalLine(nameof(patch.Translation), patch.Translation, FormatTranslation)
				.OptionalLine(
					nameof(patch.Blend),
					patch.Blend,
					blend => "\"" + blend.Color + "\"" + (blend.Alpha.HasValue ? $", {blend.Alpha}" : string.Empty)
				)
				.OptionalLine(nameof(patch.Alpha), patch.Alpha, 1)
				.OptionalLine(nameof(patch.Style), patch.Style, RenderStyle.Copy, style => style.ToString())
				.CloseBrace();
		}

		output.CloseBrace().WriteTo(writer);
	}

	private sealed class WriterUtil
	{
		private readonly List<string> _lines = new();
		private readonly WriterUtil? _parent;
		private readonly int _indentationLevel = 0;

		private string Indentation { get; } = "";

		public WriterUtil() { }

		private WriterUtil(WriterUtil parent)
		{
			_parent = parent;
			_indentationLevel = parent._indentationLevel + 1;
			Indentation = new string('\t', _indentationLevel);
		}

		public WriterUtil Line(string line)
		{
			_lines.Add(Indentation + line);
			return this;
		}

		public WriterUtil OpenBrace()
		{
			return new WriterUtil(this);
		}

		public WriterUtil CloseBrace()
		{
			var parent = _parent ?? throw new InvalidOperationException();

			if (_lines.Any())
			{
				parent.Line("{");
				parent._lines.AddRange(_lines);
				parent.Line("}");
			}
			return parent;
		}

		public WriterUtil OptionalLine(string name, double value, double defaultValue)
		{
			const double TOLERANCE = 0.01;
			if (Math.Abs(value - defaultValue) > TOLERANCE)
			{
				Line($"{name} {value:F1}");
			}

			return this;
		}

		public WriterUtil OptionalLine<T>(string name, T value, T defaultValue, Func<T, string> formatter)
		{
			if (!EqualityComparer<T>.Default.Equals(value, defaultValue))
			{
				Line(name + " " + formatter(value));
			}

			return this;
		}

		public WriterUtil OptionalLine<T>(string name, T? value, Func<T, string> formatter)
			where T : class
		{
			if (value != null)
			{
				Line(name + " " + formatter(value));
			}

			return this;
		}

		public WriterUtil Flag(string name, bool value)
		{
			if (value)
			{
				Line(name);
			}

			return this;
		}

		public void WriteTo(StreamWriter writer)
		{
			foreach (var line in _lines)
			{
				writer.WriteLine(line);
			}
		}
	}
}
