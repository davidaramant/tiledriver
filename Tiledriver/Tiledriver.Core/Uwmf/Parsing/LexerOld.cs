﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Globalization;
using System.Text;
using Tiledriver.Core.Uwmf.Parsing.Extensions;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public sealed class LexerOld : ILexerOld
    {
        private readonly IUwmfCharReader _reader;

        public LexerOld(IUwmfCharReader reader)
        {
            _reader = reader;
        }

        public Identifier ReadIdentifier()
        {
            const string eofMessage = "Unexpected end of file when reading identifier.";
            MovePastWhitespaceAndCommentsAndVerifyNotEoF(eofMessage);

            if (!_reader.Current.Char.IsValidIdentifierStartChar())
            {
                throw new UwmfParsingException(_reader.Current.Position, "Invalid start of identifier.");
            }

            var buffer = new StringBuilder();

            buffer.Append(_reader.Current.Char);
            _reader.AdvanceAndVerifyNotEoF(eofMessage);

            while (_reader.Current.Char.IsValidIdentifierChar())
            {
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }

            return new Identifier(buffer.ToString());
        }

        public int ReadIntegerNumber()
        {
            const string eofMessage = "Unexpected end of file when reading integer.";

            MovePastWhitespaceAndCommentsAndVerifyNotEoF(eofMessage);
            var buffer = new StringBuilder();

            var startPosition = _reader.Current.Position;

            while (_reader.Current.Char.IsIntegerChar())
            {
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }

            var integerString = buffer.ToString();
            int result;
            if (integerString.StartsWith("0x", StringComparison.InvariantCulture))
            {
                if (int.TryParse(integerString.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
                {
                    return result;
                }
                throw new UwmfParsingException(startPosition, "Malformed hex integer.");
            }

            if (int.TryParse(integerString, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            throw new UwmfParsingException(startPosition, "Malformed integer.");
        }

        public double ReadFloatingPointNumber()
        {
            const string eofMessage = "Unexpected end of file when reading floating point number.";

            MovePastWhitespaceAndCommentsAndVerifyNotEoF(eofMessage);

            var startPosition = _reader.Current.Position;
            var buffer = new StringBuilder();

            while (_reader.Current.Char.IsFloatingPointChar())
            {
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }

            var doubleString = buffer.ToString();
            double result;

            if (double.TryParse(doubleString, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            throw new UwmfParsingException(startPosition, "Malformed floating point number.");
        }

        public bool ReadBoolean()
        {
            const string eofMessage = "Unexpected end of file when reading Boolean.";

            MovePastWhitespaceAndCommentsAndVerifyNotEoF(eofMessage);

            var startPosition = _reader.Current.Position;
            var buffer = new StringBuilder();

            while (_reader.Current.Char.IsBooleanChar())
            {
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }

            var boolString = buffer.ToString();
            bool result;

            if (bool.TryParse(boolString, out result))
            {
                return result;
            }
            throw new UwmfParsingException(startPosition, "Malformed Boolean.");
        }

        public string ReadString()
        {
            const string eofMessage = "Unexpected end of file when reading string.";

            MovePastWhitespaceAndCommentsAndVerifyNotEoF(eofMessage);

            var startPosition = _reader.Current.Position;
            var buffer = new StringBuilder();

            if (_reader.Current.Char != '"')
            {
                throw new UwmfParsingException(startPosition, "Unexpected character when expecting string.");
            }

            _reader.AdvanceAndVerifyNotEoF(eofMessage);

            while (_reader.Current.Char != '"')
            {
                // TODO: Does ECWolf support escaped quotes?
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }

            _reader.AdvanceAndVerifyNotEoF(eofMessage);

            return buffer.ToString();
        }

        public TokenTypeOld DetermineNextToken()
        {
            MovePastWhitespaceAndComments();

            switch (_reader.Current.Char)
            {
                case '=':
                    return TokenTypeOld.Assignment;
                case '{':
                    return TokenTypeOld.StartBlock;
                case '}':
                    return TokenTypeOld.EndBlock;
                case ';':
                    return TokenTypeOld.EndOfAssignment;
                case ',':
                    return TokenTypeOld.Comma;
                default:
                    if (_reader.Current.Char.IsValidIdentifierStartChar())
                    {
                        return TokenTypeOld.Identifier;
                    }
                    if (_reader.Current.IsEndOfFile)
                    {
                        return TokenTypeOld.EndOfFile;
                    }

                    return TokenTypeOld.Unknown;
            }
        }

        public void AdvanceOneCharacter()
        {
            _reader.Advance();
        }

        public void MovePastAssignment()
        {
            const string eofMessage = "Unexpected end of file inside assignment of unknown identifier.";
            MovePastWhitespaceAndCommentsAndVerifyNotEoF(eofMessage);

            bool insideString = false;
            while (_reader.Current.Char != ';' || insideString)
            {
                if (_reader.Current.Char == '"')
                {
                    insideString = !insideString;
                }
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }
            _reader.Advance();
        }

        private void MovePastWhitespaceAndCommentsAndVerifyNotEoF(string eofMessage)
        {
            while (char.IsWhiteSpace(_reader.Current.Char))
            {
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }

            if (_reader.Current.Char.IsStartOfComment())
            {
                SkipComment();
                _reader.AdvanceAndVerifyNotEoF(eofMessage);

                while (char.IsWhiteSpace(_reader.Current.Char))
                {
                    _reader.AdvanceAndVerifyNotEoF(eofMessage);
                }
            }
        }

        private void MovePastWhitespaceAndComments()
        {
            while (char.IsWhiteSpace(_reader.Current.Char))
            {
                _reader.Advance();
            }

            if (_reader.Current.Char.IsStartOfComment())
            {
                SkipComment();
                _reader.Advance();

                while (char.IsWhiteSpace(_reader.Current.Char))
                {
                    _reader.Advance();
                }
            }
        }

        /// <summary>
        /// Skips a comment.  Does NOT move the current character past the end of it!
        /// </summary>
        private void SkipComment()
        {
            const string eofMessage = "Unexpected end of file reading comment.";

            _reader.AdvanceAndVerifyNotEoF(eofMessage);

            switch (_reader.Current.Char)
            {
                case '/':
                    while (_reader.Current.Char != '\n' && !_reader.Current.IsEndOfFile)
                    {
                        _reader.Advance();
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
                                _reader.AdvanceAndVerifyNotEoF(eofMessage);
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
                                    _reader.AdvanceAndVerifyNotEoF(eofMessage);
                                    break;
                                default:
                                    foundEndStar = false;
                                    break;
                            }
                        }
                    }

                default:
                    throw new UwmfParsingException(_reader.Current.Position, "Malformed comment.");
            }
        }
    }
}