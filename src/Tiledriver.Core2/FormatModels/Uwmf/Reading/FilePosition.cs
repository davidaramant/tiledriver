// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.FormatModels.Uwmf.Reading
{
    public readonly struct FilePosition
    {
        public int Line { get; }
        public int Column { get; }

        private FilePosition(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public static readonly FilePosition StartOfFile = new (1, 1);
        public FilePosition NextChar() => new (Line, Column + 1);
        public FilePosition NextLine() => new (Line + 1, 1);

        public override string ToString() => $"Line: {Line}, Col: {Column}";
    }
}
