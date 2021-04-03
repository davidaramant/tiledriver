// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Functional.Maybe;

namespace Tiledriver.Core.Extensions.MaybeFileSystem
{
    public static class MaybeFileSystemExtensions
    {
        public static Maybe<string> VerifyPathExists(this Maybe<string> path) => 
            path.SelectMaybe(
                p => System.IO.Directory.Exists(p) 
                ? p.ToMaybe() 
                : Maybe<string>.Nothing);
    }
}
