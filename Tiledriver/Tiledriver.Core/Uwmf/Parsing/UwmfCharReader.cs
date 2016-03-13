// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public sealed class UwmfCharReader : IUwmfCharReader
    {
        private readonly Stream _inputStream;
        private CharPosition _position = CharPosition.StartOfFile;

        public UwmfChar Current { get; private set; }

        public UwmfCharReader(Stream inputStream)
        {
            _inputStream = inputStream;
        }

        public void MaybeReadChar()
        {
            int readByte = _inputStream.ReadByte();

            if (readByte != -1)
            {
                var c = (char)readByte;

                var result = UwmfChar.Exists(c, _position);

                _position = c == '\n' ? _position.NextLine() : _position.NextChar();

                Current = result;
            }
            else
            {
                Current = UwmfChar.EndOfFile(_position);
            }
        }


        public void MustReadChar(string endOfFileMessage)
        {
            MaybeReadChar();

            if (Current.IsEndOfFile)
            {
                throw new ParsingException(Current.Position, endOfFileMessage);
            }
        }
    }
}