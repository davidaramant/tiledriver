// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using Tiledriver.Core.Uwmf.Parsing;

namespace Tiledriver.Core.Tests.Uwmf.Parsing
{
    /// <summary>
    /// Simpler implementation of <see cref="IUwmfCharReader"/> for testing purposes.
    /// </summary>
    internal sealed class TestStringReader : IUwmfCharReader
    {
        private readonly string _input;
        private bool _hasInitialized = false;
        private int _index;

        public TestStringReader(string input)
        {
            _input = input;
        }

        UwmfChar IUwmfCharReader.Current
        {
            get
            {
                if (_index < _input.Length)
                {
                    return UwmfChar.Exists(_input[_index], CharPosition.StartOfFile);
                }
                return UwmfChar.EndOfFile(CharPosition.StartOfFile);
            }
        }
        void IUwmfCharReader.MaybeReadChar()
        {
            if (_hasInitialized)
            {
                _index++;
            }
            else
            {
                _hasInitialized = true;
            }
        }

        void IUwmfCharReader.MustReadChar(string endOfFileMessage)
        {
            ((IUwmfCharReader)this).MaybeReadChar();
        }
    }
}