// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading
{
    public abstract record Token(FilePosition Location);

    public abstract record ValueToken<T>(FilePosition Location, T Value) : Token(Location)
    {
        public override string ToString() => $"{GetType().Name}: {Value}";
    }

    public sealed record IntegerToken(FilePosition Location, int Value) : ValueToken<int>(Location, Value);
    public sealed record FloatToken(FilePosition Location, double Value) : ValueToken<double>(Location, Value);
    public sealed record BooleanToken(FilePosition Location, bool Value) : ValueToken<bool>(Location, Value);
    public sealed record StringToken(FilePosition Location, string Value) : ValueToken<string>(Location, Value);

    public sealed record IdentifierToken(FilePosition Location, Identifier Id) : Token(Location);

    public sealed record EqualsToken(FilePosition Location) : Token(Location);
    public sealed record SemicolonToken(FilePosition Location) : Token(Location);
    public sealed record OpenBraceToken(FilePosition Location) : Token(Location);
    public sealed record CloseBraceToken(FilePosition Location) : Token(Location);
    public sealed record CommaToken(FilePosition Location) : Token(Location);
}