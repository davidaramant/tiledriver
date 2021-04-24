﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.MapInfos.Reading
{
    public static class MapInfoLexer
    {
        public static UnifiedLexer Create(TextReader reader) => new(reader, reportNewlines: true);
    }
}