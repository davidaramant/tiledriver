// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Uwmf.Parsing.Syntax
{
    public static class SyntaxExtensions
    {
        public static Token MustReadValueToken(this ILexer lexer)
        {
            return lexer.MustReadTokenOfTypes(
                TokenType.BooleanFalse,
                TokenType.BooleanTrue,
                TokenType.Double,
                TokenType.Integer,
                TokenType.String);
        }

        public static Maybe<Identifier> TryAsIdentifier(this Token token)
        {
            if (token.Type == TokenType.Identifier)
            {
                return new Identifier((string)token.Value).ToMaybe();
            }
            return Maybe<Identifier>.Nothing;
        }

        public static Maybe<string> TryAsString(this Token token)
        {
            if (token.Type == TokenType.String)
            {
                return ((string)token.Value).ToMaybe();
            }
            return Maybe<string>.Nothing;
        }

        public static Maybe<int> TryAsInt(this Token token)
        {
            if (token.Type == TokenType.Integer)
            {
                return ((int)token.Value).ToMaybe();
            }
            return Maybe<int>.Nothing;
        }

        public static Maybe<double> TryAsDouble(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.Integer:
                    return ((double)(int)token.Value).ToMaybe();
                case TokenType.Double:
                    return ((double)token.Value).ToMaybe();
                default:
                    return Maybe<double>.Nothing;
            }
        }

        public static Maybe<bool> TryAsBool(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.BooleanFalse:
                    return false.ToMaybe();
                case TokenType.BooleanTrue:
                    return true.ToMaybe();
                default:
                    return Maybe<bool>.Nothing;
            }
        }
    }
}