// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Piglet.Lexer;
using Tiledriver.Core.Extensions.Strings;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public sealed class MapInfoLexer
    {
        // TODO: Using a Piglet lexer might be overkill for parsing a single line
        // It also does not validate that stuff is comma separated
        private static readonly ILexer<string> ParameterLexer = LexerFactory<string>.Configure(configurator =>
        {
            // Quoted strings
            configurator.Token("\"(\\\\.|[^\"])*\"", f => f);

            // Ignore freestanding commas outside of quoted strings
            configurator.Ignore(",");

            // Returns a string for each parameter found (non-whitespace other than commas)
            configurator.Token(@"[^\s,]+", f => f);

            // Ignores all white space
            configurator.Ignore(@"\s+");
        });
        private readonly IResourceProvider _resourceProvider;

        public MapInfoLexer(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        public IEnumerable<IMapInfoElement> Analyze([NotNull] TextReader reader)
        {
            var cachingReader = new MapInfoTextReader(reader);

            string line;
            while ((line = cachingReader.ReadLine()) != null)
            {
                if (line.ToLowerInvariant().StartsWith("include"))
                {
                    foreach (var child in ParseInclude(line))
                    {
                        yield return child;
                    }
                }
                else
                {
                    yield return ParseElement(line, cachingReader);
                }
            }
        }

        private IEnumerable<IMapInfoElement> ParseInclude(string line)
        {
            var lumpPath = line.Remove(0, "include".Length).Trim().Substring(1).RemoveLast(1);

            using (var ms = _resourceProvider.Lookup(lumpPath))
            using (var reader = new StreamReader(ms, Encoding.ASCII))
            {
                // The ToArray is needed because otherwise the stream is closed
                return Analyze(reader).ToArray();
            }
        }

        private IMapInfoElement ParseElement(string line, MapInfoTextReader reader)
        {
            if (line.Contains("="))
            {
                return ParseProperty(line);
            }

            var nextLine = reader.PeekLine();
            if (nextLine == "{")
            {
                return ParseBlock(line, reader);
            }
            else if (line.Contains("{}"))
            {
                // HACK: Handle empty, one-line blocks better
                var name = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries)[0];
                return new MapInfoBlock(new Identifier(name), Enumerable.Empty<string>(), Enumerable.Empty<IMapInfoElement>());
            }
            else
            {
                return new MapInfoProperty(new Identifier(line));
            }
        }

        private MapInfoProperty ParseProperty(string line)
        {
            var firstEqualsIndex = line.IndexOf("=", StringComparison.Ordinal);

            return new MapInfoProperty(
                new Identifier(line.Substring(0, firstEqualsIndex).Trim()),
                ParseCommaSeparatedParameters(line.Substring(firstEqualsIndex + 1)));
        }

        private MapInfoBlock ParseBlock(string startLine, MapInfoTextReader reader)
        {
            var parts = startLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var name = parts[0];
            var metadata = parts.Skip(1).ToArray();

            var children = new List<IMapInfoElement>();

            reader.MustReadOpenParen();

            string line;
            while ((line = reader.MustReadLine()) != "}")
            {
                children.Add(ParseElement(line, reader));
            }

            return new MapInfoBlock(new Identifier(name), metadata, children);
        }

        public static IEnumerable<string> ParseCommaSeparatedParameters(string input)
        {
            return ParameterLexer.Tokenize(input).Select(token => token.Item2);
        }
    }
}