// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using System.Text;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.Udmf.Reading
{
	public static class UdmfReader
	{
		public static MapData Read(Stream stream)
		{
			using var textReader = new StreamReader(stream, Encoding.ASCII, leaveOpen: true);
			return UdmfSemanticAnalyzer.ReadMapData(UdmfParser.Parse(new UnifiedLexer(textReader).Scan()));
		}
	}
}
