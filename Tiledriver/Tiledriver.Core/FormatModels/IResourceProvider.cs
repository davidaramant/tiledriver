// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.FormatModels
{
    public interface IResourceProvider
    {
        byte[] Lookup(string path);
    }
}