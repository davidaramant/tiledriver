// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Text;

namespace Tiledriver.Core.GameMaps
{
    public sealed class MapHeader
    {
        public MapHeader(
            PlaneMetadata plane0Info,
            PlaneMetadata plane1Info,
            PlaneMetadata plane2Info,
            ushort width,
            ushort height,
            string name)
        {
            Plane0Info = plane0Info;
            Plane1Info = plane1Info;
            Plane2Info = plane2Info;
            Width = width;
            Height = height;
            Name = name;
        }

        public PlaneMetadata Plane0Info { get; }
        public PlaneMetadata Plane1Info { get; }
        public PlaneMetadata Plane2Info { get; }
        public ushort Width { get; }
        public ushort Height { get; }
        public string Name { get; }

        public static MapHeader Parse(byte[] buffer)
        {
            // The documentation claims the header can be RLEW compressed, but ignore that for now.  
            // I don't think the Wolf3D maps are compressed, at least.
            if (buffer.Length != 42)
            {
                throw new ArgumentException("Invalid buffer size.");
            }

            var plane0Offset = BitConverter.ToUInt32(buffer, 0);
            var plane1Offset = BitConverter.ToUInt32(buffer, 4);
            var plane2Offset = BitConverter.ToUInt32(buffer, 8);
            var plane0Length = BitConverter.ToUInt16(buffer, 12);
            var plane1Length = BitConverter.ToUInt16(buffer, 14);
            var plane2Length = BitConverter.ToUInt16(buffer, 16);
            var width = BitConverter.ToUInt16(buffer, 18);
            var height = BitConverter.ToUInt16(buffer, 20);

            var nameLength = 0;
            for (var i = 22; i < 22 + 16; i++)
            {
                if (buffer[i] == 0)
                {
                    break;
                }
                nameLength++;
            }

            var name = Encoding.ASCII.GetString(buffer, 22, nameLength);

            return new MapHeader(
                plane0Info: new PlaneMetadata(offset: plane0Offset, compressedLength: plane0Length),
                plane1Info: new PlaneMetadata(offset: plane1Offset, compressedLength: plane1Length),
                plane2Info: new PlaneMetadata(offset: plane2Offset, compressedLength: plane2Length),
                width: width,
                height: height,
                name: name);
        }
    }

}