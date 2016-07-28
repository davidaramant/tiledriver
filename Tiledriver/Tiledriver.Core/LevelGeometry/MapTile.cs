/*
** MapTile.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry
{
    public enum MapTileType
    {
        Null,
        EmptySpace,
        Textured
    }

    public sealed class MapTile
    {
        public int? Tag { get; }
        public TileTheme Theme { get; }
        public MapTileType Type { get; }

        private MapTile(int? tag, TileTheme theme, MapTileType type)
        {
            Tag = tag;
            Theme = theme;
            Type = type;
        }

        public static readonly MapTile EmptyTile = new MapTile(tag: null, theme: null, type: MapTileType.EmptySpace);
        public static readonly MapTile NullTile = new MapTile(tag: null, theme: null, type: MapTileType.Null);

        public static MapTile Textured(TileTheme theme, int? tag = null)
        {
            return new MapTile(tag: tag, theme: theme, type: MapTileType.Textured);
        }
    }
}