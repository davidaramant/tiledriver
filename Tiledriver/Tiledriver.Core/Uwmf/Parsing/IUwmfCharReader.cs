// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

namespace Tiledriver.Core.Uwmf.Parsing
{
    public interface IUwmfCharReader
    {
        UwmfChar Current { get; }

        void MaybeReadChar();
        void MustReadChar(string endOfFileMessage);
    }
}