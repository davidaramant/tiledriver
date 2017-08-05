// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Wdc31
{
    public sealed class MapInfo
    {
        public string MapName { get; }
        public int MapWidth { get; }
        public int MapHeight { get; }
        public ImmutableArray<int> MapData { get; }

        public MapInfo(
            string mapName,
            int mapWidth,
            int mapHeight,
            ImmutableArray<int> mapData)
        {
            MapName = mapName;
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            MapData = mapData;
        }
    }
}