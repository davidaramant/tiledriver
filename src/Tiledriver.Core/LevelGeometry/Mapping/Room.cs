// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.GameInfo.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Room : IRoom
    {
        private readonly ObservableCollection<MapLocation> _locations;
        private int goldKeys;
        private int silverKeys;

        public Room(int roomNumber)
        {
            Name = $"#{roomNumber}";
            AdjacentRooms = new Dictionary<IList<Passage>, IRoom>();
            _locations = new ObservableCollection<MapLocation>();
            Bosses = new List<Thing>();
            Enemies = new List<Thing>();
            Weapons = new List<Thing>();
            Treasure = new List<Thing>();
            Health = new List<Thing>();
            _locations.CollectionChanged += Locations_CollectionChanged;
        }

        public string Name { get; }

        public IDictionary<IList<Passage>, IRoom> AdjacentRooms { get; }

        public IList<MapLocation> Locations => _locations;


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

        public bool IsEndingRoom => Locations.Any(location => location.CanExit());


        public int UnopenableDoors { get; set; }
        public IList<Thing> Enemies { get; }
        public IList<Thing> Bosses { get; }
        public IList<Thing> Weapons { get; }
        public int Ammo { get; private set; }
        public IList<Thing> Treasure { get; }
        public IList<Thing> Health { get; }
        public int Lives { get; private set; }
        public int BoringTiles { get; private set; }
        public bool HasGoldKey => goldKeys > 0;
        public bool HasSilverKey => silverKeys > 0;

        private void Locations_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AdjustCounters((list, thing) => list.Add(thing), 1, e.NewItems!.Cast<MapLocation>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    AdjustCounters((list, thing) => list.Remove(thing), -1, e.NewItems!.Cast<MapLocation>());
                    break;
            }
        }

        private void AdjustCounters(Action<IList<Thing>, Thing> change, int increment, IEnumerable<MapLocation> locations)
        {
            foreach (var mapLocation in locations)
            {
                foreach (var mapLocationThing in mapLocation.Things)
                {
                    if (bossTypes.Contains(mapLocationThing.Type))
                        change(Bosses, mapLocationThing);
                    if (enemyTypes.Contains(mapLocationThing.Type))
                        change(Enemies, mapLocationThing);
                    if (WeaponTypes.Contains(mapLocationThing.Type))
                        change(Weapons, mapLocationThing);
                    if (mapLocationThing.Type == Actor.Clip.ClassName)
                        Ammo += increment;
                    if (TreasureTypes.Contains(mapLocationThing.Type))
                        change(Treasure, mapLocationThing);
                    if (HealthTypes.Contains(mapLocationThing.Type))
                        change(Health, mapLocationThing);
                    if (mapLocationThing.Type == Actor.OneUp.ClassName)
                        Lives += increment;
                    if (mapLocationThing.Type == Actor.GoldKey.ClassName || mapLocationThing.Type == Actor.Hans.ClassName || mapLocationThing.Type == Actor.Gretel.ClassName)
                        goldKeys += increment;
                    if (mapLocationThing.Type == Actor.SilverKey.ClassName)
                        silverKeys += increment;
                }

                if (mapLocation.Tile == null && !mapLocation.Things.Any())
                    BoringTiles += increment;
            }
        }

        private string DebuggerDisplay => $"Room: Name={Name}; Locations={Locations.Count})";

        private readonly string[] bossTypes = new[]
        {
            Actor.MechaHitler.ClassName,
            Actor.Gift.ClassName,
            Actor.Hans.ClassName,
            Actor.Gretel.ClassName,
            Actor.FatFace.ClassName,
            Actor.Schabbs.ClassName
        };

        private readonly string[] enemyTypes = new[]
        {
            Actor.Dog.ClassName,
            Actor.FakeHitler.ClassName,
            Actor.Mutant.ClassName,
            Actor.Guard.ClassName,
            Actor.Officer.ClassName,
            Actor.WolfensteinSS.ClassName
        };

        private readonly string[] WeaponTypes = new[]
        {
            Actor.GatlingGunUpgrade.ClassName,
            Actor.MachineGun.ClassName
        };

        private readonly string[] TreasureTypes = new[]
        {
            Actor.Chalice.ClassName,
            Actor.ChestofJewels.ClassName,
            Actor.Cross.ClassName,
            Actor.Crown.ClassName
        };

        private readonly string[] HealthTypes = new[]
        {
            Actor.Blood.ClassName,
            Actor.Dog.ClassName,
            Actor.Food.ClassName,
            Actor.Gibs.ClassName,
            Actor.Medikit.ClassName
        };
    }
}