// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;

namespace Tiledriver.Core.FormatModels.Wdc31
{
    public sealed class Header
    {
        public Header(
            string fileVersion,
            int numberOfMaps,
            ushort numberOfMapPlanes,
            ushort maxMapNameLength)
        {
            FileVersion = fileVersion;
            NumberOfMaps = numberOfMaps;
            NumberOfMapPlanes = numberOfMapPlanes;
            MaxMapNameLength = maxMapNameLength;
        }

        public string FileVersion { get; }
        public int NumberOfMaps { get; }
        public ushort NumberOfMapPlanes { get; }
        public ushort MaxMapNameLength { get; }

        public static Header Read(BinaryReader reader)
        {
            return new Header(
                fileVersion: new string(reader.ReadChars(6)),
                numberOfMaps: reader.ReadInt32(),
                numberOfMapPlanes: reader.ReadUInt16(),
                maxMapNameLength: reader.ReadUInt16());
        }
    }
}