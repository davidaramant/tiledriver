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
        private readonly TagSequence _tagSequence;
        private readonly List<Thing> _things = new List<Thing>();


        // Row-major [row,col]
        private readonly MapTile[,] _tiles;
        private readonly Dictionary<Point, Door> _doors = new Dictionary<Point, Door>();

        private struct Door
        {
            public readonly bool FacingNorthSouth;
            public readonly int Tag;

            public Door(bool facingNorthSouth, int tag)
            {
                FacingNorthSouth = facingNorthSouth;
                Tag = tag;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room" /> class.
        /// </summary>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="tiles">The tiles in row-major order [row,col].</param>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The size of the tiles array was wrong.</exception>
        public Room(Rectangle boundingBox, MapTile[,] tiles, TagSequence tagSequence)
        {
            BoundingBox = boundingBox;
            if (tiles.GetLength(0) != boundingBox.Height ||
                tiles.GetLength(1) != boundingBox.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(tiles), "The size of the tiles array was wrong.");
            }
            _tiles = tiles;
            _tagSequence = tagSequence;
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
            foreach (var locatedDoor in _doors)
            {
                yield return new Trigger
                {
                    X = BoundingBox.Left + locatedDoor.Key.X,
                    Y = BoundingBox.Top + locatedDoor.Key.Y,
                    Z = 0,
                    Action = 1, // Door action
                    Arg0 = locatedDoor.Value.Tag, // Tag
                    Arg1 = 16, // Speed
                    Arg2 = 300, // Delay
                    Arg3 = 0, // Lock
                    Arg4 = locatedDoor.Value.FacingNorthSouth ? 1 : 0, 
                    PlayerUse = true,
                    Repeatable = true,
                    MonsterUse = true,
                };
            }
        }

        public void AddDoor(int roomRow, int roomCol, bool facingNorthSouth)
        {
            _doors.Add(new Point(x: roomCol, y: roomRow),
                new Door(facingNorthSouth:facingNorthSouth,tag:_tagSequence.GetNext()));
        }

        public MapTile GetTileAtPosition(int mapRow, int mapCol)
        {
            var roomPosition = new Point(x: mapCol - BoundingBox.Left, y: mapRow - BoundingBox.Top);
            if (_doors.ContainsKey(roomPosition))
            {
                return MapTile.Textured(
                    _doors[roomPosition].FacingNorthSouth ? TileTheme.DoorFacingNorthSouth : TileTheme.DoorFacingEastWest,
                    tag:_doors[roomPosition].Tag );
            }

            return _tiles[roomPosition.Y, roomPosition.X];
        }
    }
}
