// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tiledriver.Core.FormatModels.Common.Reading
{
    /// <summary>
    /// A token stream that can handle include statements
    /// </summary>
    /// <remarks>
    /// This class makes the somewhat dangerous assumption that the identifier "include" is only
    /// ever used as a top-level construct to pull in another file. Perhaps it should be made a
    /// bit more explicit, but this works for all the formats Tiledriver currently parses.
    /// </remarks>
    public sealed class TokenSource : IEnumerable<Token>
    {
        private readonly IEnumerable<Token> _tokens;
        private readonly IResourceProvider _provider;
        private readonly Func<TextReader, UnifiedLexer> _createLexer;

        public TokenSource(
            IEnumerable<Token> tokens,
            IResourceProvider provider,
            Func<TextReader, UnifiedLexer> createLexer)
        {
            _tokens = tokens;
            _provider = provider;
            _createLexer = createLexer;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            using var enumerator = _tokens.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current is IdentifierToken include && include.Id.ToLower() == "include")
                {
                    if (!enumerator.MoveNext() || enumerator.Current is not StringToken)
                    {
                        throw new ParsingException($"Malformed include near {include.Location}");
                    }

                    StringToken pathToken = (StringToken)enumerator.Current;

                    var path = pathToken.Value;

                    using var includeStream = _provider.Lookup(path);
                    using var reader = new StreamReader(includeStream, Encoding.ASCII, leaveOpen: true);
                    var lexer = _createLexer(reader);
                    var nestedTokenStream = new TokenSource(lexer.Scan(), _provider, _createLexer);
                    foreach (var token in nestedTokenStream)
                    {
                        yield return token;
                    }
                }
                else
                {
                    yield return enumerator.Current;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}