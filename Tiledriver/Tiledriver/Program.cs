using System.IO;
using Tiledriver.Uwmf;

namespace Tiledriver
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var fs = File.Open("TEXTMAP.uwmf",FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var emptyTile = new TileId(0);
                var solidTile = new TileId(1);
                var sectorId = new SectorId(0);
                var zoneId = new ZoneId(0);

                var map = new Map
                {
                    Name = "Test Output",
                    Width = 4,
                    Height = 4,
                    TileSize = 64,
                    Tiles =
                    {
                        new Tile
                        {
                            Id = emptyTile,
                            TextureNorth = "-",
                            TextureSouth = "-",
                            TextureEast = "-",
                            TextureWest = "-",
                            BlockingNorth = false,
                            BlockingSouth = false,
                            BlockingWest = false,
                            BlockingEast = false,
                        },
                        new Tile
                        {
                            Id = solidTile,
                            TextureNorth = "GSTONEA1",
                            TextureSouth = "GSTONEA1",
                            TextureEast = "GSTONEA1",
                            TextureWest = "GSTONEA1",
                        },
                    },
                    Sectors =
                    {
                        new Sector
                        {
                            Id = new SectorId(1),
                            TextureCeiling = "#000000",
                            TextureFloor = "#FFFFFF",
                        }
                    },
                    Zones =
                    {
                        new Zone { Id = zoneId },
                    },
                    Planes = { new Plane { Depth = 64} },
                    Planemaps = { new Planemap
                    {
                        Entries =
                        {
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },

                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = emptyTile, Sector = sectorId, Zone = zoneId },
                            new PlanemapEntry { Tile = emptyTile, Sector = sectorId, Zone = zoneId },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },

                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = emptyTile, Sector = sectorId, Zone = zoneId },
                            new PlanemapEntry { Tile = emptyTile, Sector = sectorId, Zone = zoneId },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },

                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                        },
                    } },
                    Things =
                    {
                        new Thing
                        {
                            Type = 1,
                            X = 96,
                            Y = 96,
                            Skill1 = true,
                            Skill2 = true,
                            Skill3 = true,
                            Skill4 = true,
                        },
                    }
                };
                map.Write(sw);
            }
        }
    }
}
