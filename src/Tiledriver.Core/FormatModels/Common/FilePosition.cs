// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.Common;

public sealed record FilePosition(int Line, int Column)
{
	public static readonly FilePosition StartOfFile = new(1, 1);

	public FilePosition NextChar() => new(Line, Column + 1);

	public FilePosition NextLine() => new(Line + 1, 1);

	public override string ToString() => $"Line: {Line}, Col: {Column}";
}
