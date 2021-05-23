// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.FormatModels.Common
{
    public sealed record Texture(string Name)
    {
        public bool IsColor => Name.StartsWith("#");

        public static readonly Texture None = new("-");
        public static Texture SolidColor(Color color) => new($"#{color.R:X2}{color.G:X2}{color.B:X2}");

        public static implicit operator Texture(string name) => new(name);
    }
}