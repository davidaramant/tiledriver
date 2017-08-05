// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;

namespace Tiledriver.Core.FormatModels.Wdc31
{
    public sealed class Wdc31Bundle
    {
        private const string FileVersion = "WDC3.1";
        private readonly Header _header;

        public static Wdc31Bundle Load(Stream mapStream)
        {
            throw new NotImplementedException();
        }
    }
}