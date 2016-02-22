using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Sector : IUwmfEntry
    {
        public string TextureFloor { get; set; }
        public string TextureCeiling { get; set; }

        public StreamWriter Write(StreamWriter writer)
        {
            return writer.
                Line("sector").
                Line("{").
                Attribute("texturefloor", TextureFloor).
                Attribute("textureceiling", TextureCeiling).
                Line("}");
        }
    }
}
