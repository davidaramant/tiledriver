// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Sector : IUwmfEntry
    {
        public string TextureFloor { get; set; }
        public string TextureCeiling { get; set; }

        public Stream WriteTo(Stream stream)
        {
            return stream.
                Line("sector").
                Line("{").
                Attribute("texturefloor", TextureFloor).
                Attribute("textureceiling", TextureCeiling).
                Line("}");
        }
    }
}
