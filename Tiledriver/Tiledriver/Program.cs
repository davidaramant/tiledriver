using System.IO;
using Tiledriver.Uwmf;

namespace Tiledriver
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var fs = File.Open("TEXTMAP.uwmf", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var solidTile = (TileId)0;
                var sectorId = (SectorId)0;
                var zoneId = (ZoneId)0;

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
                            TextureCeiling = "#000000",
                            TextureFloor = "#FFFFFF",
                        }
                    },
                    Zones =
                    {
                        new Zone { },
                    },
                    Planes = { new Plane { Depth = 64 } },
                    Planemaps = { new Planemap
                    {
                        Entries =
                        {
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },

                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = TileId.NotSpecified, Sector = sectorId, Zone = zoneId },
                            new PlanemapEntry { Tile = TileId.NotSpecified, Sector = sectorId, Zone = zoneId },
                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },

                            new PlanemapEntry { Tile = solidTile, Sector = sectorId, Zone = ZoneId.NotSpecified },
                            new PlanemapEntry { Tile = TileId.NotSpecified, Sector = sectorId, Zone = zoneId },
                            new PlanemapEntry { Tile = TileId.NotSpecified, Sector = sectorId, Zone = zoneId },
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
                            X = 1.5,
                            Y = 1.5,
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
