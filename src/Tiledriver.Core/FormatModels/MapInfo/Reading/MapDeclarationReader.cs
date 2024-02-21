// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.IO;
using System.Text;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.MapInfo.Reading
{
	public static class MapDeclarationReader
	{
		public static IReadOnlyDictionary<string, Map> Read(Stream stream, IResourceProvider resourceProvider)
		{
			using var reader = new StreamReader(stream, Encoding.ASCII, leaveOpen: true);
			var tokenSource = new TokenSource(
				MapInfoLexer.Create(reader).Scan(),
				resourceProvider,
				MapInfoLexer.Create
			);
			using var tokenStream = tokenSource.GetEnumerator();
			return MapDeclarationParser.ReadMapDeclarations(tokenStream);
		}
	}
}
