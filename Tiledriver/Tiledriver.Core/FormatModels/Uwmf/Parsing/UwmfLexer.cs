// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Globalization;
using System.IO;
using System.Linq;
using Piglet.Lexer;

namespace Tiledriver.Core.FormatModels.Uwmf.Parsing
{
    public sealed class UwmfLexer : IUwmfLexer
    {
        public const int EndOfInputTokenNumber = -1;

        private readonly ILexer<Token> _builder = BuildLexer();

        private readonly ILexerInstance<Token> _lexer;

        public UwmfLexer(TextReader reader)
        {
            _lexer = _builder.Begin(reader);
        }

        private Token ReadToken()
        {
            var pair = _lexer.Next();
            if (pair.Item1 == EndOfInputTokenNumber)
            {
                return Token.EndOfFile;
            }
            return pair.Item2;
        }

        public Token MustReadTokenOfTypes(params TokenType[] expectedTypes)
        {
            var token = ReadToken();
            if (!expectedTypes.Contains(token.Type))
            {
                throw new UwmfParsingException($"Line {_lexer.CurrentLineNumber}: Expected {string.Join(", ", expectedTypes)} but got {token.Type}");
            }
            return token;
        }

        public static ILexer<Token> BuildLexer()
        {
            return LexerFactory<Token>.Configure(configurator =>
            {
                // These have to go first so they don't turn into identifiers
                configurator.Token(@"true", f => Token.BooleanTrue);
                configurator.Token(@"false", f => Token.BooleanFalse);

                configurator.Token(@"\{", f => Token.OpenParen);
                configurator.Token(@"\}", f => Token.CloseParen);
                configurator.Token(@",", f => Token.Comma);
                configurator.Token(@"=", f => Token.Equal);
                configurator.Token(@";", f => Token.Semicolon);

                configurator.Token(@"[A-Za-z_]+[A-Za-z0-9_]*", Token.Identifier);

                configurator.Token(@"[+-]?\d+[eE][+-]?\d+", f => Token.Double(double.Parse(f)));
                configurator.Token(@"[+-]?\d+\.\d+[eE][+-]?\d+", f => Token.Double(double.Parse(f)));
                configurator.Token(@"[+-]?\d+\.\d+", f => Token.Double(double.Parse(f)));

                // Hex Integer
                configurator.Token(@"0x[0-9A-Fa-f]+", f => Token.Integer(int.Parse(f.Substring(2, f.Length - 2), NumberStyles.HexNumber)));
                configurator.Token(@"[+-]?[0-9]+", f => Token.Integer(int.Parse(f)));

                configurator.Token("\"(\\\\.|[^\"])*\"", f => Token.String(f.Substring(1, f.Length - 2)));

                // Ignores all white space
                configurator.Ignore(@"\s+");
            });
        }
    }
}