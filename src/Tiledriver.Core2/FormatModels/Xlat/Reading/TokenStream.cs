// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.Xlat.Reading
{
    public static partial class XlatParser
    {
        private sealed class TokenStream : IEnumerable<Token>
        {
            private readonly IEnumerable<Token> _tokens;
            private readonly IResourceProvider _provider;

            public TokenStream(IEnumerable<Token> tokens, IResourceProvider provider)
            {
                _tokens = tokens;
                _provider = provider;
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
                        var lexer = XlatLexer.Create(reader);
                        var nestedTokenStream = new TokenStream(lexer.Scan(), _provider);
                        foreach (var token in nestedTokenStream)
                        {
                            yield return token;
                        }
                    }

                    yield return enumerator.Current;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}