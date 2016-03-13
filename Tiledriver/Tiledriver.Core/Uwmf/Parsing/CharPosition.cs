// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Diagnostics;

namespace Tiledriver.Core.Uwmf.Parsing
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class CharPosition
    {
        public int Line { get; }
        public int Column { get; }

        public static readonly CharPosition StartOfFile = new CharPosition(line: 1, column: 1);

        private CharPosition(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public CharPosition NextChar()
        {
            return new CharPosition(line: Line, column: Column + 1);
        }

        public CharPosition NextLine()
        {
            return new CharPosition(line: Line + 1, column: 1);
        }

        public override string ToString()
        {
            return $"Line {Line}, Column {Column}";
        }
    }
}