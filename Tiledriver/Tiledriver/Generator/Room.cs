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
        private readonly List<RegionThing> _things = new List<RegionThing>();

        // Row-major [row,col]
        private readonly MapTile[,] _tiles;
        private readonly Dictionary<Point, Door> _doors = new Dictionary<Point, Door>();

        private bool hasEndgame = false;

        private struct Door
        {
            public readonly bool FacingNorthSouth;
            public readonly int Tag;
            public readonly bool IsLocked;

            public Door(bool facingNorthSouth, int tag, bool isLocked = false)
            {
                FacingNorthSouth = facingNorthSouth;
                Tag = tag;
                IsLocked = isLocked;
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

        public void AddThing(RegionThing thing)
        {
            _things.Add(thing);
        }

        public IEnumerable<Thing> GetThings()
        {
            return _things.Select(t => new Thing
            {
                Type = t.Actor.Id,
                X = BoundingBox.Left + t.LocationOffset.X + 0.5,
                Y = BoundingBox.Top + t.LocationOffset.Y + 0.5,
                Angle = (int)t.Facing,
                Skill1 = true,
                Skill2 = true,
                Skill3 = true,
                Skill4 = true,
            });
        }

        public IEnumerable<Trigger> GetTriggers()
        {
            List<Trigger> triggers = new List<Trigger>();
            triggers.AddRange(_doors.Select(locatedDoor => new Trigger
            {
                X = BoundingBox.Left + locatedDoor.Key.X,
                Y = BoundingBox.Top + locatedDoor.Key.Y,
                Z = 0,
                Action = 1, // Door action
                Arg0 = locatedDoor.Value.Tag, // Tag
                Arg1 = 16, // Speed
                Arg2 = 300, // Delay
                Arg3 = locatedDoor.Value.IsLocked ? 1 : 0, // Lock
                Arg4 = locatedDoor.Value.FacingNorthSouth ? 1 : 0,
                PlayerUse = true,
                Repeatable = true,
                MonsterUse = true,
            }));
            if (hasEndgame)
            {
                triggers.Add(new Trigger()
                {
                    X = (BoundingBox.Left + BoundingBox.Right) / 2,
                    Y = (BoundingBox.Top + BoundingBox.Bottom) / 2,
                    Z = 0,
                    Action = 6, // Exit Victory Spin
                    PlayerUse = true,
                    PlayerCross = true
                });
            }
            return triggers;
        }

        public void AddDoor(int roomRow, int roomCol, bool facingNorthSouth, bool isLocked = false)
        {
            _doors.Add(new Point(x: roomCol, y: roomRow),
                new Door(facingNorthSouth: facingNorthSouth, tag: _tagSequence.GetNext(), isLocked: isLocked));
        }

        public MapTile GetTileAtPosition(int mapRow, int mapCol)
        {
            var roomPosition = new Point(x: mapCol - BoundingBox.Left, y: mapRow - BoundingBox.Top);
            if (_doors.ContainsKey(roomPosition))
            {
                var door = _doors[roomPosition];

                if (door.IsLocked)
                {
                    return MapTile.Textured(
                        door.FacingNorthSouth ? TileTheme.LockedDoorFacingNorthSouth : TileTheme.LockedDoorFacingEastWest,
                        tag: door.Tag);
                }
                else
                {
                    return MapTile.Textured(
                        door.FacingNorthSouth ? TileTheme.DoorFacingNorthSouth : TileTheme.DoorFacingEastWest,
                        tag: door.Tag);
                }
            }

            return _tiles[roomPosition.Y, roomPosition.X];
        }

        public void AddEndgameTrigger()
        {
            hasEndgame = true;
        }
    }
}
