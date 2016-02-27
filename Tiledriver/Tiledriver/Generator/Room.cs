using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;
using Tiledriver.Wolf3D;

namespace Tiledriver.Generator
{
    public sealed class Room : IRegion
    {
        private readonly List<Thing> _things = new List<Thing>();

        // Row-major [row,col]
        private readonly PrefabTile[,] _tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="tiles">The tiles in row-major order [row,col].</param>
        public Room(Rectangle boundingBox, PrefabTile[,] tiles)
        {
            BoundingBox = boundingBox;
            if (tiles.GetLength(0) != boundingBox.Height ||
                tiles.GetLength(1) != boundingBox.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(tiles), "The size of the tiles array was wrong.");
            }
            _tiles = tiles;
        }

        public Rectangle BoundingBox { get; }

        public void AddThing(Thing thing)
        {
            _things.Add(thing);
        }

        public IEnumerable<Thing> GetThings()
        {
            return _things;
        }

        public IEnumerable<Trigger> GetTriggers()
        {
            yield break;
        }

        public PrefabTile GetTileAtPosition(int mapRow, int mapCol)
        {
            return _tiles[mapRow - BoundingBox.Top, mapCol - BoundingBox.Left];
        }
    }
}
