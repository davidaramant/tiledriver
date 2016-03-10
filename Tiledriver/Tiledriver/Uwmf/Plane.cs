// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class Plane : IUwmfEntry
    {
        public int Depth { get; set; }

        public Stream WriteTo(Stream stream)
        {
            return stream.
                Line("plane").
                Line("{").
                Attribute("depth", Depth).
                Line("}");
        }
    }
}
