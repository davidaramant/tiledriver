// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public sealed class MapInfoTextReader
    {
        private readonly TextReader _reader;
        private string _nextLine;
        private bool _hasNextLine = false;

        public MapInfoTextReader(TextReader reader)
        {
            _reader = reader;
        }

        public string PeekLine()
        {
            if (_hasNextLine)
            {
                return _nextLine;
            }
            _nextLine = ReadLine();
            _hasNextLine = true;
            return _nextLine;
        }

        public string ReadLine()
        {
            if (_hasNextLine)
            {
                _hasNextLine = false;
                return _nextLine;
            }

            string line;
            do
            {
                line = _reader.ReadLine()?.Trim();
            } while (!IsLineInteresting(line));

            return line;
        }

        private static bool IsLineInteresting(string line)
        {
            if (line == null)
                return true;

            return !(
                line.StartsWith("//") || 
                line == string.Empty );
        }

        public string MustReadLine()
        {
            var line = ReadLine();
            if (line == null)
            {
                throw new ParsingException("Unexpected end of input.");
            }
            return line;
        }

        public void MustReadOpenParen()
        {
            var line = MustReadLine();
            if (line != "{")
            {
                throw new ParsingException("Expected open paren, but got: " + line);
            }
        }
    }
}