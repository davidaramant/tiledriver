// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Functional.Maybe;

namespace Tiledriver.Core.Uwmf.Parsing.Syntax
{
    public static class SyntaxExtensions
    {
        public static Maybe<string> TryAsString(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.String:
                case TokenType.Identifier:
                    return ((string) token.Value).ToMaybe();
                default:
                    return Maybe<string>.Nothing;
            }
        }

        public static Maybe<int> TryAsInt(this Token token)
        {
            if (token.Type == TokenType.Integer)
            {
                return ((int) token.Value).ToMaybe();
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