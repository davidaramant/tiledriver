using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;

namespace Tiledriver.Wolf3D
{
    public sealed class TileTheme
    {
        public TileId Id { get; }
        public Tile Definition { get; }

        private TileTheme(int id, Tile definition)
        {
            Id = (TileId)id;
            Definition = definition;
        }

        public static TileTheme GrayStone = new TileTheme(
            id: 0,
            definition: new Tile
            {
                TextureNorth = "GSTONEA1",
                TextureSouth = "GSTONEA1",
                TextureEast = "GSTONEA2",
                TextureWest = "GSTONEA2",
            });

        public static TileTheme DoorFacingNorthSouth = new TileTheme(
            id: 1,
            definition: new Tile
            {
                TextureNorth = "DOOR1_1",
                TextureSouth = "DOOR1_1",
                TextureEast = "SLOT1_1",
                TextureWest = "SLOT1_1",
                OffsetHorizontal = true,
            });

        public static TileTheme DoorFacingEastWest = new TileTheme(
            id: 2,
            definition: new Tile
            {
                TextureNorth = "DOOR1_2",
                TextureSouth = "DOOR1_2",
                TextureEast = "SLOT1_2",
                TextureWest = "SLOT1_2",
                OffsetVertical = true,
            });


        public static IEnumerable<TileTheme> GetAll()
        {
            // The Empty tile is a special case and should not be returned.
            yield return GrayStone;
            yield return DoorFacingNorthSouth;
            yield return DoorFacingEastWest;
        }
    }
}
