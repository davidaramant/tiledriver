// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.MapInfo.Reading;

public static class MapInfoLexer
{
	public static UnifiedLexer Create(TextReader reader) => new(reader, reportNewlines: true);
}
