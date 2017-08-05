// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.FormatModels.Wdc31
{
    public sealed class Header
    {
        public Header(
            string fileVersion, 
            long numberOfMaps, 
            int numberOfMapPlanes, 
            int maxMapNameLength)
        {
            FileVersion = fileVersion;
            NumberOfMaps = numberOfMaps;
            NumberOfMapPlanes = numberOfMapPlanes;
            MaxMapNameLength = maxMapNameLength;
        }

        public string FileVersion { get; }
        public long NumberOfMaps { get; }
        public int NumberOfMapPlanes { get; }
        public int MaxMapNameLength { get; }
    }
}