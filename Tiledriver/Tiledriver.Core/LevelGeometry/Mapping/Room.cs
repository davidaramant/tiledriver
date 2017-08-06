// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Room : IRoom
    {
        public string Name { get; }
        public int Lives { get; }
        public IDictionary<IList<Passage>, IRoom> AdjacentRooms { get; }
        public IList<MapLocation> Locations { get; }

        public Room(int roomNumber)
        {
            Name = $"#{roomNumber}";
            AdjacentRooms = new Dictionary<IList<Passage>, IRoom>();
            Locations = new List<MapLocation>();
        }

        public bool IsStartingRoom
        {
            get
            {
                foreach (var location in Locations)
                {
                    if (location.Things.Any(thing => thing.Type == Actor.Player1Start.ClassName))
                        return true;
                }

                return false;
            }
        }

        public bool IsEndingRoom
        {
            get
            {
                return Locations.Any(location => location.CanExit());
            }
        }

        public int UnopenableDoors { get; }
        public IEnumerable<Thing> Enemies { get; }
        public IEnumerable<Thing> Bosses { get; }
        public IEnumerable<Thing> Weapons { get; }
        public int Ammo { get; }
        public IEnumerable<Thing> Treasure { get; }
        public int Health { get; }

        private string DebuggerDisplay => $"Room: Name={Name}; Locations={Locations.Count})";
    }
}