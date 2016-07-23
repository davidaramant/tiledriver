// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core
{
    public static class DemoMap
    {
        public static Map Create()
        {
            var map = new Map
            (
                nameSpace: "Wolf3D",
                tileSize: 64,
                name: "Test Output",
                width: 64,
                height: 64,
                comment: "",
                tiles: new[]
                {
                    new Tile
                    (
                        textureNorth: "GSTONEA1",
                        textureSouth: "GSTONEA1",
                        textureEast: "GSTONEA2",
                        textureWest: "GSTONEA2"
                    ),
                },
                sectors: new[]
                {
                    new Sector
                    (
                        textureCeiling: "#C0C0C0",
                        textureFloor: "#A0A0A0"
                    )
                },
                zones: new[]
                {
                    new Zone(),
                },
                planes: new[] { new Plane(depth: 64) },
                planeMaps: new[] { new PlaneMap(CreateGeometry(width: 64, height: 64)) },
                things: new[]
                {
                    new Thing
                    (
                        type: WolfActor.Player1Start.ToString(),
                        x: 1.5,
                        y: 1.5,
                        z: 0,
                        angle: 0,
                        skill1: true,
                        skill2: true,
                        skill3: true,
                        skill4: true
                    ),
                },
                triggers: new Trigger[] { }
            );

            return map;
        }

        private static IEnumerable<TileSpace> CreateGeometry(int width, int height)
        {
            var entries = new TileSpace[height, width];

            var solidTile = new TileSpace(0, 0, -1);
            var emptyTile = new TileSpace(-1, 0, 0);

            // ### Build a big empty square

            // Top wall
            for (var col = 0; col < width; col++)
            {
                entries[0, col] = solidTile;
            }

            for (var row = 1; row < height - 1; row++)
            {
                entries[row, 0] = solidTile;
                for (var col = 1; col < width - 1; col++)
                {
                    entries[row, col] = emptyTile;
                }
                entries[row, width - 1] = solidTile;
            }

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = solidTile;
            }


            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    yield return entries[row, col];
                }
            }
        }
    }
}
