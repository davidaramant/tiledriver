/*
** UwmfCharReader.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

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
            Advance();
        }

        public void Advance()
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


        public void AdvanceAndVerifyNotEoF(string endOfFileMessage)
        {
            Advance();

            if (Current.IsEndOfFile)
            {
                throw new ParsingException(Current.Position, endOfFileMessage);
            }
        }
    }
}