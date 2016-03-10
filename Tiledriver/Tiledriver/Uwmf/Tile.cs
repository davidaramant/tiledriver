﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Tile : IUwmfEntry
    {
        public string TextureNorth { get; set; }
        public string TextureSouth { get; set; }
        public string TextureWest { get; set; }
        public string TextureEast { get; set; }

        public bool OffsetVertical { get; set; }
        public bool OffsetHorizontal { get; set; }

        public bool BlockingNorth { get; set; } = true;
        public bool BlockingSouth { get; set; } = true;
        public bool BlockingWest { get; set; } = true;
        public bool BlockingEast { get; set; } = true;

        public Stream WriteTo(Stream stream)
        {
            return stream.
                Line("tile").
                Line("{").
                Attribute("texturenorth", TextureNorth).
                Attribute("texturesouth", TextureSouth).
                Attribute("texturewest", TextureWest).
                Attribute("textureeast", TextureEast).
                Attribute("offsetvertical", OffsetVertical).
                Attribute("offsethorizontal", OffsetHorizontal).
                Attribute("blockingnorth", BlockingNorth).
                Attribute("blockingsouth", BlockingSouth).
                Attribute("blockingwest", BlockingWest).
                Attribute("blockingeast", BlockingEast).
                Line("}");
        }
    }
}
