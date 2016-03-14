// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Globalization;
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
            MovePastWhitespaceAndComments(eofMessage);

            if (!_reader.Current.Char.IsValidIdentifierStartChar())
            {
                throw new ParsingException(_reader.Current.Position, "Invalid start of identifier.");
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

        public int ReadIntegerAssignment()
        {
            const string eofMessage = "Unexpected end of file when reading integer.";

            MovePastWhitespaceAndComments(eofMessage);
            var buffer = new StringBuilder();

            var startPosition = _reader.Current.Position;

            while (!_reader.Current.Char.IsEndOfAssignment())
            {
                if (!_reader.Current.Char.IsIntegerChar())
                {
                    throw new ParsingException(
                        _reader.Current.Position,
                        $"Unexpected character while reading integer: {_reader.Current.Char}.");
                }
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }
            _reader.Advance();

            var integerString = buffer.ToString();
            int result;
            if (integerString.StartsWith("0x", StringComparison.InvariantCulture))
            {
                if (int.TryParse(integerString.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
                {
                    return result;
                }
                throw new ParsingException(startPosition, "Malformed hex integer.");
            }

            if (int.TryParse(integerString, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            throw new ParsingException(startPosition, "Malformed integer.");
        }

        public double ReadFloatingPointAssignment()
        {
            const string eofMessage = "Unexpected end of file when reading floating point number.";

            MovePastWhitespaceAndComments(eofMessage);

            var startPosition = _reader.Current.Position;
            var buffer = new StringBuilder();

            while (!_reader.Current.Char.IsEndOfAssignment())
            {
                if (!_reader.Current.Char.IsFloatingPointChar())
                {
                    throw new ParsingException(
                        _reader.Current.Position,
                        $"Unexpected character while reading floating point number: {_reader.Current.Char}.");
                }
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }
            _reader.Advance();

            var doubleString = buffer.ToString();
            double result;

            if (double.TryParse(doubleString, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            throw new ParsingException(startPosition, "Malformed floating point number.");
        }

        public bool ReadBooleanAssignment()
        {
            const string eofMessage = "Unexpected end of file when reading Boolean.";

            MovePastWhitespaceAndComments(eofMessage);

            var startPosition = _reader.Current.Position;
            var buffer = new StringBuilder();

            while (!_reader.Current.Char.IsEndOfAssignment())
            {
                if (!_reader.Current.Char.IsBooleanChar())
                {
                    throw new ParsingException(
                        _reader.Current.Position,
                        $"Unexpected character while reading Boolean: {_reader.Current.Char}.");
                }
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }
            _reader.Advance();

            var boolString = buffer.ToString();
            bool result;

            if (bool.TryParse(boolString, out result))
            {
                return result;
            }
            throw new ParsingException(startPosition, "Malformed Boolean.");
        }

        public string ReadStringAssignment()
        {
            const string eofMessage = "Unexpected end of file when reading string.";

            MovePastWhitespaceAndComments(eofMessage);

            var startPosition = _reader.Current.Position;
            var buffer = new StringBuilder();

            if (_reader.Current.Char != '"')
            {
                throw new ParsingException(startPosition, "Unexpected character when expecting string.");
            }

            _reader.AdvanceAndVerifyNotEoF(eofMessage);

            while (_reader.Current.Char != '"')
            {
                // TODO: Does ECWolf support escaped quotes?
                buffer.Append(_reader.Current.Char);
                _reader.AdvanceAndVerifyNotEoF(eofMessage);
            }

            _reader.AdvanceAndVerifyNotEoF(eofMessage);

            if (_reader.Current.Char != ';')
            {
                throw new ParsingException(_reader.Current.Position, "No end of expression found after string.");
            }
            _reader.Advance();

            return buffer.ToString();
        }

        public ExpressionType DetermineIfAssignmentOrStartBlock()
        {
            const string eofMessage = "Unexpected end of file after identifier.";

            MovePastWhitespaceAndComments(eofMessage);

            switch (_reader.Current.Char)
            {
                case '=':
                    _reader.AdvanceAndVerifyNotEoF("Unexpected end of file after assignment.");
                    return ExpressionType.Assignment;
                case '{':
                    _reader.AdvanceAndVerifyNotEoF("Unexpected end of file after start of block.");
                    return ExpressionType.StartBlock;
                default:
                    throw new ParsingException(_reader.Current.Position, "Unexpected character after identifier.");
            }
        }

        public ExpressionType DetermineIfIdentifierOrEndBlock()
        {
            MovePastWhitespaceAndComments("Unexpected end of file inside block");

            if (_reader.Current.Char == '}')
            {
                _reader.Advance();
                return ExpressionType.EndBlock;
            }
            if (_reader.Current.Char.IsValidIdentifierStartChar())
            {
                // Do not advance the reader.
                return ExpressionType.Identifier;
            }

            throw new ParsingException(_reader.Current.Position, "Unexpected character inside block.");
        }

        public void MovePastAssignment()
        {
            const string eofMessage = "Unexpected end of file inside assignment of unknown identifier.";
            MovePastWhitespaceAndComments(eofMessage);

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

        private void MovePastWhitespaceAndComments(string eofMessage)
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
                    throw new ParsingException(_reader.Current.Position, "Malformed comment.");
            }
        }
    }
}