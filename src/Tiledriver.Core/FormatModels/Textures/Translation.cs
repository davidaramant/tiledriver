// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.Textures
{
    public abstract record Translation
    {
        public sealed record Desaturate(int Amount) : Translation;
        public sealed record Blue : Translation;
        public sealed record Gold : Translation;
        public sealed record Green : Translation;
        public sealed record Ice : Translation;
        public sealed record Inverse : Translation;
        public sealed record Red : Translation;
        public sealed record Custom(string Block) : Translation;
    }
}