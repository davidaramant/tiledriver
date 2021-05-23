// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace WangscapeTilesetChopper.Model
{
    internal record TileDefinition(
        [property: JsonPropertyName("corners")]
        ImmutableArray<string> CornerTextures,
        string Filename,
        int X,
        int Y)
    {
        internal Corners ParseCorners()
        {
            var corners = Corners.None;

            if (CornerTextures[0] == "variant")
            {
                corners |= Corners.TopLeft;
            }
            if (CornerTextures[1] == "variant")
            {
                corners |= Corners.TopRight;
            }
            if (CornerTextures[2] == "variant")
            {
                corners |= Corners.BottomRight;
            }
            if (CornerTextures[3] == "variant")
            {
                corners |= Corners.BottomLeft;
            }

            return corners;
        }

        internal string GetFileName() => $"tile{(int)ParseCorners():D2}.png";
    }
}