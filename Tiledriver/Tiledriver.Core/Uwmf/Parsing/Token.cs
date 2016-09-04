// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public enum TokenType
    {
        EndOfFile,
        Identifier,
        Equal,
        Semicolon,
        String,
        Integer,
        Double,
        BooleanTrue,
        BooleanFalse,
        OpenParen,
        CloseParen,
        Comma,
    }

    [DebuggerDisplay("{ToString()}")]
    public sealed class Token : IEquatable<Token>
    {
        public object Value { get; }
        public TokenType Type { get; }

        private Token(TokenType type, object value)
        {
            Type = type;
            Value = value;
        }

        public bool IsValue =>
            Type == TokenType.BooleanFalse ||
            Type == TokenType.BooleanTrue ||
            Type == TokenType.Double ||
            Type == TokenType.Integer ||
            Type == TokenType.String;

        public string ValueAsString
        {
            get
            {
                if (!(Type == TokenType.String || Type == TokenType.Identifier))
                {
                    throw new InvalidOperationException($"Tried to access a {Type} token value as a string.");
                }
                return (string)Value;
            }
        }

        public int ValueAsInt
        {
            get
            {
                if (Type != TokenType.Integer)
                {
                    throw new InvalidOperationException($"Tried to access a {Type} token value as an integer.");
                }
                return (int)Value;
            }
        }

        public double ValueAsDouble
        {
            get
            {
                switch (Type)
                {
                    case TokenType.Double:
                        return (double)Value;
                    case TokenType.Integer:
                        return (int)Value;
                    default:
                        throw new InvalidOperationException($"Tried to access a {Type} token value as a double.");
                }
            }
        }

        public bool ValueAsBool
        {
            get
            {
                if (!(Type == TokenType.BooleanTrue || Type == TokenType.BooleanFalse))
                {
                    throw new InvalidOperationException($"Tried to access a {Type} token value as a boolean.");
                }
                return (bool)Value;
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case TokenType.Identifier:
                case TokenType.String:
                case TokenType.Integer:
                case TokenType.Double:
                    return $"{Type}: {Value}";

                default:
                    return Type.ToString();
            }
        }

        public static Token Identifier(string id)
        {
            return new Token(TokenType.Identifier, id);
        }
        public static Token String(string value)
        {
            return new Token(TokenType.String, value);
        }
        public static Token Integer(int value)
        {
            return new Token(TokenType.Integer, value);
        }
        public static Token Double(double value)
        {
            return new Token(TokenType.Double, value);
        }

        public static readonly Token BooleanTrue = new Token(TokenType.BooleanTrue, null);
        public static readonly Token BooleanFalse = new Token(TokenType.BooleanFalse, null);
        public static readonly Token Equal = new Token(TokenType.Equal, null);
        public static readonly Token Semicolon = new Token(TokenType.Semicolon, null);
        public static readonly Token OpenParen = new Token(TokenType.OpenParen, null);
        public static readonly Token CloseParen = new Token(TokenType.CloseParen, null);
        public static readonly Token Comma = new Token(TokenType.Comma, null);
        public static readonly Token EndOfFile = new Token(TokenType.EndOfFile, null);

        #region Equality members

        public bool Equals(Token other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Value, other.Value) && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Token && Equals((Token)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Value?.GetHashCode() ?? 0) * 397) ^ (int)Type;
            }
        }
        #endregion Equality members
    }
}