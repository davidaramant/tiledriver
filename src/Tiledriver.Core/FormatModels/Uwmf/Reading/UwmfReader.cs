// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Text;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading;

public static class UwmfReader
{
	public static MapData Read(Stream stream)
	{
		using var textReader = new StreamReader(stream, Encoding.ASCII, leaveOpen: true);
		return UwmfSemanticAnalyzer.ReadMapData(UwmfParser.Parse(new UnifiedLexer(textReader).Scan()));
	}
}
