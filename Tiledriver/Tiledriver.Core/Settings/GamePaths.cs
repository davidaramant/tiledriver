// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Functional.Maybe;

namespace Tiledriver.Core.Settings
{
    public sealed class GamePaths
    {
        public GamePaths(Maybe<string> wolf3D, Maybe<string> spearOfDestiny)
        {
            Wolf3D = wolf3D;
            SpearOfDestiny = spearOfDestiny;
        }

        public Maybe<string> Wolf3D { get; }
        public Maybe<string> SpearOfDestiny { get; }
    }
}
