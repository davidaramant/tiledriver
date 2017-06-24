// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Tiledriver.Core.Extensions.Strings;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public sealed class MapInfoLexer
    {
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
            if (nextLine == null || nextLine == "}")
            {
                return new MapInfoProperty(new Identifier(line));
            }
            else if (nextLine == "{")
            {
                return ParseBlock(line, reader);
            }
            else
            {
                throw new ParsingException("Unknown construct in MapInfo.");
            }
        }

        private MapInfoProperty ParseProperty(string line)
        {
            var firstEqualsIndex = line.IndexOf("=", StringComparison.Ordinal);

            return new MapInfoProperty(
                new Identifier(line.Substring(0, firstEqualsIndex).Trim()),
                line.Substring(firstEqualsIndex + 1).Split(',').Select(v => v.Trim()));
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
    }
}