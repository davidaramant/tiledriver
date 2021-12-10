// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

// TODO: This used the old T4 generator. Port this over to DataModelGenerator

using System.Collections.Generic;

namespace Tiledriver.Core.GameInfo.Wolf3D;

public sealed record Actor(
    string InstanceName,
    int ID,
    ActorCategory ActorCategory,
    bool IsSolid)
{
    public string ClassName => ((this == Player1Start) ? "$" : "") + InstanceName;

    /// <summary>
    /// Player1Start
    /// </summary>
    public static readonly Actor Player1Start = new Actor(
        InstanceName: "Player1Start",
        ID: 1,
        ActorCategory: ActorCategory.Special,
        IsSolid: false
    );
    /// <summary>
    /// PatrolPoint
    /// </summary>
    public static readonly Actor PatrolPoint = new Actor(
        InstanceName: "PatrolPoint",
        ID: 10,
        ActorCategory: ActorCategory.Special,
        IsSolid: false
    );
    /// <summary>
    /// Puddle
    /// </summary>
    public static readonly Actor Puddle = new Actor(
        InstanceName: "Puddle",
        ID: 33,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// GreenBarrel
    /// </summary>
    public static readonly Actor GreenBarrel = new Actor(
        InstanceName: "GreenBarrel",
        ID: 34,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// TableWithChairs
    /// </summary>
    public static readonly Actor TableWithChairs = new Actor(
        InstanceName: "TableWithChairs",
        ID: 35,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// FloorLamp
    /// </summary>
    public static readonly Actor FloorLamp = new Actor(
        InstanceName: "FloorLamp",
        ID: 36,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// Chandelier
    /// </summary>
    public static readonly Actor Chandelier = new Actor(
        InstanceName: "Chandelier",
        ID: 37,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// HangedMan
    /// </summary>
    public static readonly Actor HangedMan = new Actor(
        InstanceName: "HangedMan",
        ID: 38,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// WhitePillar
    /// </summary>
    public static readonly Actor WhitePillar = new Actor(
        InstanceName: "WhitePillar",
        ID: 40,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// GreenPlant
    /// </summary>
    public static readonly Actor GreenPlant = new Actor(
        InstanceName: "GreenPlant",
        ID: 41,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// SkeletonFlat
    /// </summary>
    public static readonly Actor SkeletonFlat = new Actor(
        InstanceName: "SkeletonFlat",
        ID: 42,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// Sink
    /// </summary>
    public static readonly Actor Sink = new Actor(
        InstanceName: "Sink",
        ID: 43,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// BrownPlant
    /// </summary>
    public static readonly Actor BrownPlant = new Actor(
        InstanceName: "BrownPlant",
        ID: 44,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// Vase
    /// </summary>
    public static readonly Actor Vase = new Actor(
        InstanceName: "Vase",
        ID: 45,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// BareTable
    /// </summary>
    public static readonly Actor BareTable = new Actor(
        InstanceName: "BareTable",
        ID: 46,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// CeilingLight
    /// </summary>
    public static readonly Actor CeilingLight = new Actor(
        InstanceName: "CeilingLight",
        ID: 47,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// KitchenStuff
    /// </summary>
    public static readonly Actor KitchenStuff = new Actor(
        InstanceName: "KitchenStuff",
        ID: 48,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// SuitOfArmor
    /// </summary>
    public static readonly Actor SuitOfArmor = new Actor(
        InstanceName: "SuitOfArmor",
        ID: 49,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// HangingCage
    /// </summary>
    public static readonly Actor HangingCage = new Actor(
        InstanceName: "HangingCage",
        ID: 50,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// SkeletonCage
    /// </summary>
    public static readonly Actor SkeletonCage = new Actor(
        InstanceName: "SkeletonCage",
        ID: 51,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// Bones1
    /// </summary>
    public static readonly Actor Bones1 = new Actor(
        InstanceName: "Bones1",
        ID: 52,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// BunkBed
    /// </summary>
    public static readonly Actor BunkBed = new Actor(
        InstanceName: "BunkBed",
        ID: 55,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// Basket
    /// </summary>
    public static readonly Actor Basket = new Actor(
        InstanceName: "Basket",
        ID: 56,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// Barrel
    /// </summary>
    public static readonly Actor Barrel = new Actor(
        InstanceName: "Barrel",
        ID: 68,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// Well
    /// </summary>
    public static readonly Actor Well = new Actor(
        InstanceName: "Well",
        ID: 69,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// EmptyWell
    /// </summary>
    public static readonly Actor EmptyWell = new Actor(
        InstanceName: "EmptyWell",
        ID: 70,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// Flag
    /// </summary>
    public static readonly Actor Flag = new Actor(
        InstanceName: "Flag",
        ID: 72,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// CallApogee
    /// </summary>
    public static readonly Actor CallApogee = new Actor(
        InstanceName: "CallApogee",
        ID: 73,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// Bones2
    /// </summary>
    public static readonly Actor Bones2 = new Actor(
        InstanceName: "Bones2",
        ID: 74,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// Bones3
    /// </summary>
    public static readonly Actor Bones3 = new Actor(
        InstanceName: "Bones3",
        ID: 75,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// Bones4
    /// </summary>
    public static readonly Actor Bones4 = new Actor(
        InstanceName: "Bones4",
        ID: 76,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// Pots
    /// </summary>
    public static readonly Actor Pots = new Actor(
        InstanceName: "Pots",
        ID: 77,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// Stove
    /// </summary>
    public static readonly Actor Stove = new Actor(
        InstanceName: "Stove",
        ID: 78,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: true
    );
    /// <summary>
    /// Spears
    /// </summary>
    public static readonly Actor Spears = new Actor(
        InstanceName: "Spears",
        ID: 79,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// Vines
    /// </summary>
    public static readonly Actor Vines = new Actor(
        InstanceName: "Vines",
        ID: 80,
        ActorCategory: ActorCategory.Decorations,
        IsSolid: false
    );
    /// <summary>
    /// FakeHitler
    /// </summary>
    public static readonly Actor FakeHitler = new Actor(
        InstanceName: "FakeHitler",
        ID: 18,
        ActorCategory: ActorCategory.Bosses,
        IsSolid: false
    );
    /// <summary>
    /// MechaHitler
    /// </summary>
    public static readonly Actor MechaHitler = new Actor(
        InstanceName: "MechaHitler",
        ID: 19,
        ActorCategory: ActorCategory.Bosses,
        IsSolid: false
    );
    /// <summary>
    /// Clip
    /// </summary>
    public static readonly Actor Clip = new Actor(
        InstanceName: "Clip",
        ID: 59,
        ActorCategory: ActorCategory.Ammo,
        IsSolid: false
    );
    /// <summary>
    /// Cross
    /// </summary>
    public static readonly Actor Cross = new Actor(
        InstanceName: "Cross",
        ID: 62,
        ActorCategory: ActorCategory.Treasure,
        IsSolid: false
    );
    /// <summary>
    /// Chalice
    /// </summary>
    public static readonly Actor Chalice = new Actor(
        InstanceName: "Chalice",
        ID: 63,
        ActorCategory: ActorCategory.Treasure,
        IsSolid: false
    );
    /// <summary>
    /// ChestofJewels
    /// </summary>
    public static readonly Actor ChestofJewels = new Actor(
        InstanceName: "ChestofJewels",
        ID: 64,
        ActorCategory: ActorCategory.Treasure,
        IsSolid: false
    );
    /// <summary>
    /// Crown
    /// </summary>
    public static readonly Actor Crown = new Actor(
        InstanceName: "Crown",
        ID: 65,
        ActorCategory: ActorCategory.Treasure,
        IsSolid: false
    );
    /// <summary>
    /// Hans
    /// </summary>
    public static readonly Actor Hans = new Actor(
        InstanceName: "Hans",
        ID: 16,
        ActorCategory: ActorCategory.Bosses,
        IsSolid: false
    );
    /// <summary>
    /// Gretel
    /// </summary>
    public static readonly Actor Gretel = new Actor(
        InstanceName: "Gretel",
        ID: 20,
        ActorCategory: ActorCategory.Bosses,
        IsSolid: false
    );
    /// <summary>
    /// Schabbs
    /// </summary>
    public static readonly Actor Schabbs = new Actor(
        InstanceName: "Schabbs",
        ID: 17,
        ActorCategory: ActorCategory.Bosses,
        IsSolid: false
    );
    /// <summary>
    /// Gift
    /// </summary>
    public static readonly Actor Gift = new Actor(
        InstanceName: "Gift",
        ID: 21,
        ActorCategory: ActorCategory.Bosses,
        IsSolid: false
    );
    /// <summary>
    /// FatFace
    /// </summary>
    public static readonly Actor FatFace = new Actor(
        InstanceName: "FatFace",
        ID: 22,
        ActorCategory: ActorCategory.Bosses,
        IsSolid: false
    );
    /// <summary>
    /// Blinky
    /// </summary>
    public static readonly Actor Blinky = new Actor(
        InstanceName: "Blinky",
        ID: 29,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// Inky
    /// </summary>
    public static readonly Actor Inky = new Actor(
        InstanceName: "Inky",
        ID: 32,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// Pinky
    /// </summary>
    public static readonly Actor Pinky = new Actor(
        InstanceName: "Pinky",
        ID: 31,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// Clyde
    /// </summary>
    public static readonly Actor Clyde = new Actor(
        InstanceName: "Clyde",
        ID: 30,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// Guard
    /// </summary>
    public static readonly Actor Guard = new Actor(
        InstanceName: "Guard",
        ID: 11,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// DeadGuard
    /// </summary>
    public static readonly Actor DeadGuard = new Actor(
        InstanceName: "DeadGuard",
        ID: 81,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// Dog
    /// </summary>
    public static readonly Actor Dog = new Actor(
        InstanceName: "Dog",
        ID: 14,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// Officer
    /// </summary>
    public static readonly Actor Officer = new Actor(
        InstanceName: "Officer",
        ID: 12,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// Mutant
    /// </summary>
    public static readonly Actor Mutant = new Actor(
        InstanceName: "Mutant",
        ID: 15,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// WolfensteinSS
    /// </summary>
    public static readonly Actor WolfensteinSS = new Actor(
        InstanceName: "WolfensteinSS",
        ID: 13,
        ActorCategory: ActorCategory.Enemy,
        IsSolid: false
    );
    /// <summary>
    /// DogFood
    /// </summary>
    public static readonly Actor DogFood = new Actor(
        InstanceName: "DogFood",
        ID: 39,
        ActorCategory: ActorCategory.Health,
        IsSolid: false
    );
    /// <summary>
    /// Food
    /// </summary>
    public static readonly Actor Food = new Actor(
        InstanceName: "Food",
        ID: 57,
        ActorCategory: ActorCategory.Health,
        IsSolid: false
    );
    /// <summary>
    /// Medikit
    /// </summary>
    public static readonly Actor Medikit = new Actor(
        InstanceName: "Medikit",
        ID: 58,
        ActorCategory: ActorCategory.Health,
        IsSolid: false
    );
    /// <summary>
    /// OneUp
    /// </summary>
    public static readonly Actor OneUp = new Actor(
        InstanceName: "OneUp",
        ID: 66,
        ActorCategory: ActorCategory.Health,
        IsSolid: false
    );
    /// <summary>
    /// Blood
    /// </summary>
    public static readonly Actor Blood = new Actor(
        InstanceName: "Blood",
        ID: 67,
        ActorCategory: ActorCategory.Health,
        IsSolid: false
    );
    /// <summary>
    /// Gibs
    /// </summary>
    public static readonly Actor Gibs = new Actor(
        InstanceName: "Gibs",
        ID: 71,
        ActorCategory: ActorCategory.Health,
        IsSolid: false
    );
    /// <summary>
    /// GoldKey
    /// </summary>
    public static readonly Actor GoldKey = new Actor(
        InstanceName: "GoldKey",
        ID: 53,
        ActorCategory: ActorCategory.Key,
        IsSolid: false
    );
    /// <summary>
    /// SilverKey
    /// </summary>
    public static readonly Actor SilverKey = new Actor(
        InstanceName: "SilverKey",
        ID: 54,
        ActorCategory: ActorCategory.Key,
        IsSolid: false
    );
    /// <summary>
    /// MachineGun
    /// </summary>
    public static readonly Actor MachineGun = new Actor(
        InstanceName: "MachineGun",
        ID: 60,
        ActorCategory: ActorCategory.Weapons,
        IsSolid: false
    );
    /// <summary>
    /// GatlingGunUpgrade
    /// </summary>
    public static readonly Actor GatlingGunUpgrade = new Actor(
        InstanceName: "GatlingGunUpgrade",
        ID: 61,
        ActorCategory: ActorCategory.Weapons,
        IsSolid: false
    );

    public override string ToString() => InstanceName;

    /// <summary>
    /// Returns all the enumeration values.
    /// </summary>
    public static IEnumerable<Actor> GetAll()
    {
        yield return Player1Start;
        yield return PatrolPoint;
        yield return Puddle;
        yield return GreenBarrel;
        yield return TableWithChairs;
        yield return FloorLamp;
        yield return Chandelier;
        yield return HangedMan;
        yield return WhitePillar;
        yield return GreenPlant;
        yield return SkeletonFlat;
        yield return Sink;
        yield return BrownPlant;
        yield return Vase;
        yield return BareTable;
        yield return CeilingLight;
        yield return KitchenStuff;
        yield return SuitOfArmor;
        yield return HangingCage;
        yield return SkeletonCage;
        yield return Bones1;
        yield return BunkBed;
        yield return Basket;
        yield return Barrel;
        yield return Well;
        yield return EmptyWell;
        yield return Flag;
        yield return CallApogee;
        yield return Bones2;
        yield return Bones3;
        yield return Bones4;
        yield return Pots;
        yield return Stove;
        yield return Spears;
        yield return Vines;
        yield return FakeHitler;
        yield return MechaHitler;
        yield return Clip;
        yield return Cross;
        yield return Chalice;
        yield return ChestofJewels;
        yield return Crown;
        yield return Hans;
        yield return Gretel;
        yield return Schabbs;
        yield return Gift;
        yield return FatFace;
        yield return Blinky;
        yield return Inky;
        yield return Pinky;
        yield return Clyde;
        yield return Guard;
        yield return DeadGuard;
        yield return Dog;
        yield return Officer;
        yield return Mutant;
        yield return WolfensteinSS;
        yield return DogFood;
        yield return Food;
        yield return Medikit;
        yield return OneUp;
        yield return Blood;
        yield return Gibs;
        yield return GoldKey;
        yield return SilverKey;
        yield return MachineGun;
        yield return GatlingGunUpgrade;
    }
}
