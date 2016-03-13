// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Text;
using Tiledriver.Core.Uwmf.Parsing.Extensions;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public sealed class Lexer : ILexer
    {
        private readonly IUwmfCharReader _reader;

        public Lexer(IUwmfCharReader reader)
        {
            _reader = reader;
        }

        public Identifier ReadIdentifier()
        {
            const string eofMessage = "Unexpected end of file when reading identifier.";
            var buffer = new StringBuilder();

            MovePastWhitespaceAndComments(eofMessage);

            if (!_reader.Current.Char.IsValidIdentifierStartChar())
            {
                throw new ParsingException(_reader.Current.Position, "Invalid start of identifier.");
            }

            buffer.Append(_reader.Current.Char);
            _reader.MustReadChar(eofMessage);

            while (_reader.Current.Char.IsValidIdentifierChar())
            {
                buffer.Append(_reader.Current.Char);
                _reader.MustReadChar(eofMessage);
            }

            return new Identifier(buffer.ToString());
        }

        public int ReadIntAssignment()
        {
            throw new NotImplementedException();
        }

        public double ReadDoubleAssignment()
        {
            throw new NotImplementedException();
        }

        public bool ReadBoolAssignment()
        {
            throw new NotImplementedException();
        }

        public string ReadStringAssignment()
        {
            throw new NotImplementedException();
        }

        public void VerifyStartOfBlock()
        {
            throw new NotImplementedException();
        }

        public ExpressionType DetermineIfAssignmentOrStartBlock()
        {
            const string eofMessage = "Unexpected end of file after identifier.";

            MovePastWhitespaceAndComments(eofMessage);

            switch (_reader.Current.Char)
            {
                case '=':
                    return ExpressionType.Assignment;
                case '{':
                    return ExpressionType.StartBlock;
                default:
                    throw new ParsingException(_reader.Current.Position, "Unexpected character after identifier.");
            }
        }

        public ExpressionType DetermineIfIdentifierOrEndBlock()
        {
            throw new NotImplementedException();
        }

        public void MovePastAssignment()
        {
            throw new NotImplementedException();
        }

        private void MovePastWhitespaceAndComments(string eofMessage)
        {
            _reader.MustReadChar(eofMessage);

            while (char.IsWhiteSpace(_reader.Current.Char))
            {
                _reader.MustReadChar(eofMessage);
            }

            if (_reader.Current.Char.IsStartOfComment())
            {
                SkipComment();
                _reader.MustReadChar(eofMessage);

                while (char.IsWhiteSpace(_reader.Current.Char))
                {
                    _reader.MustReadChar(eofMessage);
                }
            }
        }

        /// <summary>
        /// Skips a comment.  Does NOT move the current character past the end of it!
        /// </summary>
        private void SkipComment()
        {
            const string eofMessage = "Unexpected end of file reading comment.";

            _reader.MustReadChar(eofMessage);

            switch (_reader.Current.Char)
            {
                case '/':
                    while (_reader.Current.Char != '\n' && !_reader.Current.IsEndOfFile)
                    {
                        _reader.MaybeReadChar();
                    }
                    return;

                case '*':
                    bool foundEndStar = false;

                    while (true)
                    {
                        if (!foundEndStar)
                        {
                            while (_reader.Current.Char != '*')
                            {
                                _reader.MustReadChar(eofMessage);
                            }
                            foundEndStar = true;
                        }
                        else
                        {
                            switch (_reader.Current.Char)
                            {
                                case '/':
                                    return;
                                case '*':
                                    _reader.MustReadChar(eofMessage);
                                    break;
                                default:
                                    foundEndStar = false;
                                    break;
                            }
                        }
                    }

                default:
                    throw new ParsingException(_reader.Current.Position, "Malformed comment.");
            }
        }
    }
}