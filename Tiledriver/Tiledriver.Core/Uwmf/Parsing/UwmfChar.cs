// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Diagnostics;

namespace Tiledriver.Core.Uwmf.Parsing
{
    [DebuggerDisplay("{ToString()}")]
    public struct UwmfChar
    {
        public readonly char Char;
        public readonly bool IsEndOfFile;
        public readonly CharPosition Position;

        private UwmfChar(char c, bool endOfFile, CharPosition position)
        {
            Char = c;
            IsEndOfFile = endOfFile;
            Position = position;
        }

        public static UwmfChar Exists(char c, CharPosition position)
        {
            return new UwmfChar(c: c, endOfFile: false, position: position);
        }

        public static UwmfChar EndOfFile(CharPosition position)
        {
            return new UwmfChar(c: '�', endOfFile: true, position: position);
        }

        public override string ToString()
        {
            if (IsEndOfFile)
            {
                return $"{Position}: End of File";
            }
            return $"{Position}: {Char}";
        }
    }
}