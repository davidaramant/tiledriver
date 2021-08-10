// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tiledriver.Core.FormatModels.Textures.Writing
{
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
            var output = new WriterUtil(writer);

            var optional = texture.Optional ? "optional " : "";

            output
                .Line($"{texture.Namespace} {optional}{texture.Name}, {texture.Width}, {texture.Height}")
                .OpenBrace()
                .OptionalLine("XScale", texture.XScale, 1)
                .OptionalLine("YScale", texture.YScale, 1)
                .OptionalLine(nameof(texture.Offset), texture.Offset, new TextureOffset(), offset => $"{offset.Horizontal}, {offset.Vertical}")
                .Flag(nameof(texture.WorldPanning), texture.WorldPanning)
                .Flag(nameof(texture.NoDecals), texture.NoDecals);

            foreach (var patch in texture.Patches)
            {
                string FormatTranslation(Translation? translation)
                {
                    return translation switch
                    {
                        Translation.Desaturate d => $"Desaturate, {d.Amount}",
                        Translation.Custom c => $"\"{c.Block}\"",
                        null => throw new Exception("Should not allow null here"),
                        _ => translation.GetType().Name,
                    };
                }

                output
                    .Line($"{patch.Namespace} {patch.Name}, {patch.XOrigin}, {patch.YOrigin}")
                    .OpenBrace()
                    .Flag(nameof(patch.FlipX), patch.FlipX)
                    .Flag(nameof(patch.FlipY), patch.FlipY)
                    .Flag(nameof(patch.UseOffsets), patch.UseOffsets)
                    .OptionalLine(nameof(patch.Rotate), patch.Rotate, PatchRotation.None, rotation => ((int)rotation).ToString())
                    .OptionalLine(nameof(patch.Translation), patch.Translation, null, FormatTranslation)
                    .OptionalLine(nameof(patch.Blend), patch.Blend, null,
                        blend => "\"" + blend.Color + "\"" + (blend.Alpha.HasValue ? $", {blend.Alpha}" : string.Empty))
                    .OptionalLine(nameof(patch.Alpha), patch.Alpha, 1)
                    .OptionalLine(nameof(patch.Style), patch.Style, RenderStyle.Copy, style => style.ToString())
                    .CloseBrace();
            }

            output.CloseBrace();
        }

        private sealed class WriterUtil
        {
            private readonly StreamWriter _writer;
            private int _indentationLevel = 0;

            private int IndentationLevel
            {
                get => _indentationLevel;
                set
                {
                    _indentationLevel = value;
                    Indentation = new string('\t', value);
                }
            }

            private string Indentation { get; set; } = "";

            public WriterUtil(StreamWriter writer) => _writer = writer;

            public WriterUtil Line(string line)
            {
                _writer.WriteLine(Indentation + line);
                return this;
            }

            public WriterUtil OpenBrace()
            {
                Line("{");
                IndentationLevel++;
                return this;
            }

            public WriterUtil CloseBrace()
            {
                IndentationLevel--;
                return Line("}");
            }

            public WriterUtil OptionalLine(string name, double value, double defaultValue)
            {
                const double TOLERANCE = 0.1;
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

            public WriterUtil Flag(string name, bool value)
            {
                if (value)
                {
                    Line(name);
                }

                return this;
            }
        }
    }
}