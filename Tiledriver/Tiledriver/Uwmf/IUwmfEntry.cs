// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;

namespace Tiledriver.Uwmf
{
    public interface IUwmfEntry
    {
        Stream WriteTo(Stream stream);
    }
}
