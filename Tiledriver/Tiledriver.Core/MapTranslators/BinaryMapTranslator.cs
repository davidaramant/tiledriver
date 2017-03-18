// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.MapTranslators
{
    public sealed class BinaryMapTranslator
    {
        private readonly MapTranslatorInfo _translatorInfo;

        public BinaryMapTranslator(MapTranslatorInfo translatorInfo)
        {
            _translatorInfo = translatorInfo;
        }

        public Map Translate(BinaryMap binaryMap)
        {
            throw new NotImplementedException();
        }
    }
}
