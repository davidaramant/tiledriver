// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.MapTranslators
{
    public sealed class GameInfoTranslator
    {
        private readonly MapTranslatorInfo _translatorInfo;

        public GameInfoTranslator(MapTranslatorInfo translatorInfo)
        {
            _translatorInfo = translatorInfo;
        }

        public Map Translate(BinaryMap binaryMap)
        {
            return null;
        }
    }
}
