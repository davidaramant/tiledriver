﻿// <autogenerated>
// This code was generated by a tool. Any changes made manually will be lost
// the next time this code is regenerated.
// </autogenerated>

namespace Tiledriver.Core.Wolf3D
{
    /// <summary>
    /// WolfActor
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute( "RichEnumGenerator", "1.0.0.0" )]
    public sealed partial class WolfActor
    {
        private readonly string _instanceName;
        public readonly System.Int32 Id;
        public readonly System.Int32 Skill1Health;
        public readonly System.Int32 Skill2Health;
        public readonly System.Int32 Skill3Health;
        public readonly System.Int32 Skill4Health;
        public readonly System.Int32 Points;
        public readonly System.Int32 PickupAmmo;
        public readonly System.Int32 PickupHealth;
        public readonly System.Boolean Solid;

        /// <summary>
        /// Player1Start
        /// </summary>
        public static readonly WolfActor Player1Start = new WolfActor(
            instanceName: "$Player1Start",
            id: 1,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// PatrolPoint
        /// </summary>
        public static readonly WolfActor PatrolPoint = new WolfActor(
            instanceName: "PatrolPoint",
            id: 10,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Guard
        /// </summary>
        public static readonly WolfActor Guard = new WolfActor(
            instanceName: "Guard",
            id: 11,   
            skill1Health: 25,   
            skill2Health: 25,   
            skill3Health: 25,   
            skill4Health: 25,   
            points: 100,   
            pickupAmmo: 4,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Officer
        /// </summary>
        public static readonly WolfActor Officer = new WolfActor(
            instanceName: "Officer",
            id: 12,   
            skill1Health: 50,   
            skill2Health: 50,   
            skill3Health: 50,   
            skill4Health: 50,   
            points: 400,   
            pickupAmmo: 4,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// SSGuard
        /// </summary>
        public static readonly WolfActor SSGuard = new WolfActor(
            instanceName: "SSGuard",
            id: 13,   
            skill1Health: 100,   
            skill2Health: 100,   
            skill3Health: 100,   
            skill4Health: 100,   
            points: 500,   
            pickupAmmo: 4,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Dog
        /// </summary>
        public static readonly WolfActor Dog = new WolfActor(
            instanceName: "Dog",
            id: 14,   
            skill1Health: 1,   
            skill2Health: 1,   
            skill3Health: 1,   
            skill4Health: 1,   
            points: 200,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Mutant
        /// </summary>
        public static readonly WolfActor Mutant = new WolfActor(
            instanceName: "Mutant",
            id: 15,   
            skill1Health: 45,   
            skill2Health: 55,   
            skill3Health: 55,   
            skill4Health: 65,   
            points: 700,   
            pickupAmmo: 4,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Hans
        /// </summary>
        public static readonly WolfActor Hans = new WolfActor(
            instanceName: "Hans",
            id: 16,   
            skill1Health: 850,   
            skill2Health: 950,   
            skill3Health: 1050,   
            skill4Health: 1200,   
            points: 5000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Schabbs
        /// </summary>
        public static readonly WolfActor Schabbs = new WolfActor(
            instanceName: "Schabbs",
            id: 17,   
            skill1Health: 850,   
            skill2Health: 950,   
            skill3Health: 1550,   
            skill4Health: 2400,   
            points: 5000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// FakeHitler
        /// </summary>
        public static readonly WolfActor FakeHitler = new WolfActor(
            instanceName: "FakeHitler",
            id: 18,   
            skill1Health: 200,   
            skill2Health: 300,   
            skill3Health: 400,   
            skill4Health: 500,   
            points: 2000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Hitler
        /// </summary>
        public static readonly WolfActor Hitler = new WolfActor(
            instanceName: "Hitler",
            id: 19,   
            skill1Health: 850,   
            skill2Health: 950,   
            skill3Health: 1050,   
            skill4Health: 1200,   
            points: 5000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Gretel
        /// </summary>
        public static readonly WolfActor Gretel = new WolfActor(
            instanceName: "Gretel",
            id: 20,   
            skill1Health: 1350,   
            skill2Health: 1650,   
            skill3Health: 1850,   
            skill4Health: 2100,   
            points: 10000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Gift
        /// </summary>
        public static readonly WolfActor Gift = new WolfActor(
            instanceName: "Gift",
            id: 21,   
            skill1Health: 850,   
            skill2Health: 950,   
            skill3Health: 1050,   
            skill4Health: 1200,   
            points: 5000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Fatface
        /// </summary>
        public static readonly WolfActor Fatface = new WolfActor(
            instanceName: "Fatface",
            id: 22,   
            skill1Health: 850,   
            skill2Health: 950,   
            skill3Health: 1050,   
            skill4Health: 1200,   
            points: 5000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// PacmanGhostBlinky
        /// </summary>
        public static readonly WolfActor PacmanGhostBlinky = new WolfActor(
            instanceName: "PacmanGhostBlinky",
            id: 29,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// PacmanGhostClyde
        /// </summary>
        public static readonly WolfActor PacmanGhostClyde = new WolfActor(
            instanceName: "PacmanGhostClyde",
            id: 30,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// PacmanGhostPinky
        /// </summary>
        public static readonly WolfActor PacmanGhostPinky = new WolfActor(
            instanceName: "PacmanGhostPinky",
            id: 31,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// PacmanGhostInky
        /// </summary>
        public static readonly WolfActor PacmanGhostInky = new WolfActor(
            instanceName: "PacmanGhostInky",
            id: 32,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Puddle
        /// </summary>
        public static readonly WolfActor Puddle = new WolfActor(
            instanceName: "Puddle",
            id: 33,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// GreenBarrel
        /// </summary>
        public static readonly WolfActor GreenBarrel = new WolfActor(
            instanceName: "GreenBarrel",
            id: 34,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// TableWithChairs
        /// </summary>
        public static readonly WolfActor TableWithChairs = new WolfActor(
            instanceName: "TableWithChairs",
            id: 35,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// FloorLamp
        /// </summary>
        public static readonly WolfActor FloorLamp = new WolfActor(
            instanceName: "FloorLamp",
            id: 36,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Chandelier
        /// </summary>
        public static readonly WolfActor Chandelier = new WolfActor(
            instanceName: "Chandelier",
            id: 37,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// HangedMan
        /// </summary>
        public static readonly WolfActor HangedMan = new WolfActor(
            instanceName: "HangedMan",
            id: 38,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// DogFood
        /// </summary>
        public static readonly WolfActor DogFood = new WolfActor(
            instanceName: "DogFood",
            id: 39,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 4,   
            solid: false  
        );
        /// <summary>
        /// WhitePillar
        /// </summary>
        public static readonly WolfActor WhitePillar = new WolfActor(
            instanceName: "WhitePillar",
            id: 40,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// GreenPlant
        /// </summary>
        public static readonly WolfActor GreenPlant = new WolfActor(
            instanceName: "GreenPlant",
            id: 41,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// SkeletonFlat
        /// </summary>
        public static readonly WolfActor SkeletonFlat = new WolfActor(
            instanceName: "SkeletonFlat",
            id: 42,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Sink
        /// </summary>
        public static readonly WolfActor Sink = new WolfActor(
            instanceName: "Sink",
            id: 43,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// BrownPlant
        /// </summary>
        public static readonly WolfActor BrownPlant = new WolfActor(
            instanceName: "BrownPlant",
            id: 44,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Vase
        /// </summary>
        public static readonly WolfActor Vase = new WolfActor(
            instanceName: "Vase",
            id: 45,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Table
        /// </summary>
        public static readonly WolfActor Table = new WolfActor(
            instanceName: "Table",
            id: 46,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// CeilingLight
        /// </summary>
        public static readonly WolfActor CeilingLight = new WolfActor(
            instanceName: "CeilingLight",
            id: 47,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// KitchenStuff
        /// </summary>
        public static readonly WolfActor KitchenStuff = new WolfActor(
            instanceName: "KitchenStuff",
            id: 48,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// SuitOfArmor
        /// </summary>
        public static readonly WolfActor SuitOfArmor = new WolfActor(
            instanceName: "SuitOfArmor",
            id: 49,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// EmptyCage
        /// </summary>
        public static readonly WolfActor EmptyCage = new WolfActor(
            instanceName: "EmptyCage",
            id: 50,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// CageWithSkeleton
        /// </summary>
        public static readonly WolfActor CageWithSkeleton = new WolfActor(
            instanceName: "CageWithSkeleton",
            id: 51,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Bones1
        /// </summary>
        public static readonly WolfActor Bones1 = new WolfActor(
            instanceName: "Bones1",
            id: 52,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// GoldKey
        /// </summary>
        public static readonly WolfActor GoldKey = new WolfActor(
            instanceName: "GoldKey",
            id: 53,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// SilverKey
        /// </summary>
        public static readonly WolfActor SilverKey = new WolfActor(
            instanceName: "SilverKey",
            id: 54,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Bed
        /// </summary>
        public static readonly WolfActor Bed = new WolfActor(
            instanceName: "Bed",
            id: 55,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Basket
        /// </summary>
        public static readonly WolfActor Basket = new WolfActor(
            instanceName: "Basket",
            id: 56,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Food
        /// </summary>
        public static readonly WolfActor Food = new WolfActor(
            instanceName: "Food",
            id: 57,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 10,   
            solid: false  
        );
        /// <summary>
        /// FirstAidKit
        /// </summary>
        public static readonly WolfActor FirstAidKit = new WolfActor(
            instanceName: "FirstAidKit",
            id: 58,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 58,   
            solid: false  
        );
        /// <summary>
        /// Clip
        /// </summary>
        public static readonly WolfActor Clip = new WolfActor(
            instanceName: "Clip",
            id: 59,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 8,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// MachineGun
        /// </summary>
        public static readonly WolfActor MachineGun = new WolfActor(
            instanceName: "MachineGun",
            id: 60,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 6,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// GatlingGun
        /// </summary>
        public static readonly WolfActor GatlingGun = new WolfActor(
            instanceName: "GatlingGun",
            id: 61,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 6,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Cross
        /// </summary>
        public static readonly WolfActor Cross = new WolfActor(
            instanceName: "Cross",
            id: 62,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 100,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Chalice
        /// </summary>
        public static readonly WolfActor Chalice = new WolfActor(
            instanceName: "Chalice",
            id: 63,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 500,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// ChestOfJewels
        /// </summary>
        public static readonly WolfActor ChestOfJewels = new WolfActor(
            instanceName: "ChestOfJewels",
            id: 64,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 1000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Crown
        /// </summary>
        public static readonly WolfActor Crown = new WolfActor(
            instanceName: "Crown",
            id: 65,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 5000,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// ExtraLife
        /// </summary>
        public static readonly WolfActor ExtraLife = new WolfActor(
            instanceName: "ExtraLife",
            id: 66,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 25,   
            pickupHealth: 99,   
            solid: false  
        );
        /// <summary>
        /// Blood
        /// </summary>
        public static readonly WolfActor Blood = new WolfActor(
            instanceName: "Blood",
            id: 67,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 1,   
            solid: false  
        );
        /// <summary>
        /// Barrel
        /// </summary>
        public static readonly WolfActor Barrel = new WolfActor(
            instanceName: "Barrel",
            id: 68,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Well
        /// </summary>
        public static readonly WolfActor Well = new WolfActor(
            instanceName: "Well",
            id: 69,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// EmptyWell
        /// </summary>
        public static readonly WolfActor EmptyWell = new WolfActor(
            instanceName: "EmptyWell",
            id: 70,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Gibs
        /// </summary>
        public static readonly WolfActor Gibs = new WolfActor(
            instanceName: "Gibs",
            id: 71,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 1,   
            solid: false  
        );
        /// <summary>
        /// Flag
        /// </summary>
        public static readonly WolfActor Flag = new WolfActor(
            instanceName: "Flag",
            id: 72,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// CallApogee
        /// </summary>
        public static readonly WolfActor CallApogee = new WolfActor(
            instanceName: "CallApogee",
            id: 73,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Bones2
        /// </summary>
        public static readonly WolfActor Bones2 = new WolfActor(
            instanceName: "Bones2",
            id: 74,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Bones3
        /// </summary>
        public static readonly WolfActor Bones3 = new WolfActor(
            instanceName: "Bones3",
            id: 75,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Bones4
        /// </summary>
        public static readonly WolfActor Bones4 = new WolfActor(
            instanceName: "Bones4",
            id: 76,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Pots
        /// </summary>
        public static readonly WolfActor Pots = new WolfActor(
            instanceName: "Pots",
            id: 77,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// Stove
        /// </summary>
        public static readonly WolfActor Stove = new WolfActor(
            instanceName: "Stove",
            id: 78,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Spears
        /// </summary>
        public static readonly WolfActor Spears = new WolfActor(
            instanceName: "Spears",
            id: 79,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: true  
        );
        /// <summary>
        /// Vines
        /// </summary>
        public static readonly WolfActor Vines = new WolfActor(
            instanceName: "Vines",
            id: 80,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );
        /// <summary>
        /// DeadGuard
        /// </summary>
        public static readonly WolfActor DeadGuard = new WolfActor(
            instanceName: "DeadGuard",
            id: 81,   
            skill1Health: 0,   
            skill2Health: 0,   
            skill3Health: 0,   
            skill4Health: 0,   
            points: 0,   
            pickupAmmo: 0,   
            pickupHealth: 0,   
            solid: false  
        );

        private WolfActor(
            string instanceName,
            System.Int32 id,   
            System.Int32 skill1Health,   
            System.Int32 skill2Health,   
            System.Int32 skill3Health,   
            System.Int32 skill4Health,   
            System.Int32 points,   
            System.Int32 pickupAmmo,   
            System.Int32 pickupHealth,   
            System.Boolean solid  
        )
        {
            _instanceName = instanceName;
            Id = id; 
            Skill1Health = skill1Health; 
            Skill2Health = skill2Health; 
            Skill3Health = skill3Health; 
            Skill4Health = skill4Health; 
            Points = points; 
            PickupAmmo = pickupAmmo; 
            PickupHealth = pickupHealth; 
            Solid = solid; 
        }

        /// <summary>
        /// Returns the name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _instanceName;
        }

        /// <summary>
        /// Returns all the enumeration values.
        /// </summary>
        public static System.Collections.Generic.IEnumerable<WolfActor> GetAll()
        {
            yield return Player1Start;
            yield return PatrolPoint;
            yield return Guard;
            yield return Officer;
            yield return SSGuard;
            yield return Dog;
            yield return Mutant;
            yield return Hans;
            yield return Schabbs;
            yield return FakeHitler;
            yield return Hitler;
            yield return Gretel;
            yield return Gift;
            yield return Fatface;
            yield return PacmanGhostBlinky;
            yield return PacmanGhostClyde;
            yield return PacmanGhostPinky;
            yield return PacmanGhostInky;
            yield return Puddle;
            yield return GreenBarrel;
            yield return TableWithChairs;
            yield return FloorLamp;
            yield return Chandelier;
            yield return HangedMan;
            yield return DogFood;
            yield return WhitePillar;
            yield return GreenPlant;
            yield return SkeletonFlat;
            yield return Sink;
            yield return BrownPlant;
            yield return Vase;
            yield return Table;
            yield return CeilingLight;
            yield return KitchenStuff;
            yield return SuitOfArmor;
            yield return EmptyCage;
            yield return CageWithSkeleton;
            yield return Bones1;
            yield return GoldKey;
            yield return SilverKey;
            yield return Bed;
            yield return Basket;
            yield return Food;
            yield return FirstAidKit;
            yield return Clip;
            yield return MachineGun;
            yield return GatlingGun;
            yield return Cross;
            yield return Chalice;
            yield return ChestOfJewels;
            yield return Crown;
            yield return ExtraLife;
            yield return Blood;
            yield return Barrel;
            yield return Well;
            yield return EmptyWell;
            yield return Gibs;
            yield return Flag;
            yield return CallApogee;
            yield return Bones2;
            yield return Bones3;
            yield return Bones4;
            yield return Pots;
            yield return Stove;
            yield return Spears;
            yield return Vines;
            yield return DeadGuard;
        }
    }
}

