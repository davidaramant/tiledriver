﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;

namespace Tiledriver.Wolf3D
{
    public sealed class PrefabTile
    {
        public TileId Id { get; }
        public Tile Definition { get; }

        private PrefabTile(int id, Tile definition)
        {
            Id = (TileId) id;
            Definition = definition;
        }

        public static PrefabTile GrayStone = new PrefabTile(
            id:0,
            definition:new Tile
            {
                TextureNorth = "GSTONEA1",
                TextureSouth = "GSTONEA1",
                TextureEast = "GSTONEA2",
                TextureWest = "GSTONEA2",
            });

        public static IEnumerable<PrefabTile> GetAll()
        {
            yield return GrayStone;
        }
    }
}
