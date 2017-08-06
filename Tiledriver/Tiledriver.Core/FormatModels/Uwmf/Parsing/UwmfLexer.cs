// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Globalization;
using System.IO;
using JetBrains.Annotations;
using Piglet.Lexer;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Uwmf.Parsing
{
    public sealed class UwmfLexer : BaseLexer
    {
        public UwmfLexer([NotNull]TextReader reader) :
            base(Definition.Begin(reader))
        {
        }

        public static readonly ILexer<Token> Definition = LexerFactory<Token>.Configure(configurator =>
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

                 // Ignores comments
                 configurator.Ignore(@"//[^\n]*");
                 // Ignores all white space
                 configurator.Ignore(@"\s+");
             });
    }
}