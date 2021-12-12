// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Tiledriver.Core.GameInfo.Doom;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Actor
{
    /// <summary>Player 1 start</summary>
    public static readonly Actor Player1Start = new(
        Id: 1,
        Description: "Player 1 start",
        Width: 32,
        Height: 56
    );

    /// <summary>Player 2 start</summary>
    public static readonly Actor Player2Start = new(
        Id: 2,
        Description: "Player 2 start",
        Width: 32,
        Height: 56
    );

    /// <summary>Player 3 start</summary>
    public static readonly Actor Player3Start = new(
        Id: 3,
        Description: "Player 3 start",
        Width: 32,
        Height: 56
    );

    /// <summary>Player 4 start</summary>
    public static readonly Actor Player4Start = new(
        Id: 4,
        Description: "Player 4 start",
        Width: 32,
        Height: 56
    );

    /// <summary>Player Deathmatch start</summary>
    public static readonly Actor DeathmatchStart = new(
        Id: 11,
        Description: "Player Deathmatch start",
        Width: 32,
        Height: 56
    );

    /// <summary>Teleport Destination</summary>
    public static readonly Actor TeleportDest = new(
        Id: 14,
        Description: "Teleport Destination",
        Width: 32,
        Height: 56
    );

    /// <summary>Former Human</summary>
    public static readonly Actor ZombieMan = new(
        Id: 3004,
        Description: "Former Human",
        Width: 40,
        Height: 56
    );

    /// <summary>Former Sergeant</summary>
    public static readonly Actor ShotgunGuy = new(
        Id: 9,
        Description: "Former Sergeant",
        Width: 40,
        Height: 56
    );

    /// <summary>Imp</summary>
    public static readonly Actor DoomImp = new(
        Id: 3001,
        Description: "Imp",
        Width: 40,
        Height: 56
    );

    /// <summary>Demon</summary>
    public static readonly Actor Demon = new(
        Id: 3002,
        Description: "Demon",
        Width: 60,
        Height: 56
    );

    /// <summary>Spectre</summary>
    public static readonly Actor Spectre = new(
        Id: 58,
        Description: "Spectre",
        Width: 60,
        Height: 56
    );

    /// <summary>Lost Soul</summary>
    public static readonly Actor LostSoul = new(
        Id: 3006,
        Description: "Lost Soul",
        Width: 32,
        Height: 56
    );

    /// <summary>Cacodemon</summary>
    public static readonly Actor Cacodemon = new(
        Id: 3005,
        Description: "Cacodemon",
        Width: 62,
        Height: 56
    );

    /// <summary>Baron of Hell</summary>
    public static readonly Actor BaronOfHell = new(
        Id: 3003,
        Description: "Baron of Hell",
        Width: 48,
        Height: 64
    );

    /// <summary>Cyberdemon</summary>
    public static readonly Actor Cyberdemon = new(
        Id: 16,
        Description: "Cyberdemon",
        Width: 80,
        Height: 110
    );

    /// <summary>Spider Mastermind</summary>
    public static readonly Actor SpiderMastermind = new(
        Id: 7,
        Description: "Spider Mastermind",
        Width: 256,
        Height: 100
    );

    /// <summary>Chainsaw</summary>
    public static readonly Actor Chainsaw = new(
        Id: 2005,
        Description: "Chainsaw",
        Width: 40,
        Height: 25
    );

    /// <summary>Shotgun</summary>
    public static readonly Actor Shotgun = new(
        Id: 2001,
        Description: "Shotgun",
        Width: 40,
        Height: 25
    );

    /// <summary>Chaingun</summary>
    public static readonly Actor Chaingun = new(
        Id: 2002,
        Description: "Chaingun",
        Width: 40,
        Height: 25
    );

    /// <summary>Rocket launcher</summary>
    public static readonly Actor RocketLauncher = new(
        Id: 2003,
        Description: "Rocket launcher",
        Width: 40,
        Height: 25
    );

    /// <summary>Plasma gun</summary>
    public static readonly Actor PlasmaRifle = new(
        Id: 2004,
        Description: "Plasma gun",
        Width: 40,
        Height: 25
    );

    /// <summary>BFG9000</summary>
    public static readonly Actor BFG9000 = new(
        Id: 2006,
        Description: "BFG9000",
        Width: 40,
        Height: 30
    );

    /// <summary>Ammo clip</summary>
    public static readonly Actor Clip = new(
        Id: 2007,
        Description: "Ammo clip",
        Width: 40,
        Height: 16
    );

    /// <summary>Shotgun shells</summary>
    public static readonly Actor Shell = new(
        Id: 2008,
        Description: "Shotgun shells",
        Width: 40,
        Height: 16
    );

    /// <summary>Rocket</summary>
    public static readonly Actor RocketAmmo = new(
        Id: 2010,
        Description: "Rocket",
        Width: 40,
        Height: 25
    );

    /// <summary>Cell charge</summary>
    public static readonly Actor Cell = new(
        Id: 2047,
        Description: "Cell charge",
        Width: 40,
        Height: 16
    );

    /// <summary>Box of Ammo</summary>
    public static readonly Actor ClipBox = new(
        Id: 2048,
        Description: "Box of Ammo",
        Width: 40,
        Height: 16
    );

    /// <summary>Box of Shells</summary>
    public static readonly Actor ShellBox = new(
        Id: 2049,
        Description: "Box of Shells",
        Width: 40,
        Height: 16
    );

    /// <summary>Box of Rockets</summary>
    public static readonly Actor RocketBox = new(
        Id: 2046,
        Description: "Box of Rockets",
        Width: 40,
        Height: 25
    );

    /// <summary>Cell charge pack</summary>
    public static readonly Actor CellPack = new(
        Id: 17,
        Description: "Cell charge pack",
        Width: 40,
        Height: 25
    );

    /// <summary>Backpack</summary>
    public static readonly Actor Backpack = new(
        Id: 8,
        Description: "Backpack",
        Width: 40,
        Height: 16
    );

    /// <summary>Stimpack</summary>
    public static readonly Actor Stimpack = new(
        Id: 2011,
        Description: "Stimpack",
        Width: 40,
        Height: 16
    );

    /// <summary>Medikit</summary>
    public static readonly Actor Medikit = new(
        Id: 2012,
        Description: "Medikit",
        Width: 40,
        Height: 25
    );

    /// <summary>Health bonus</summary>
    public static readonly Actor HealthBonus = new(
        Id: 2014,
        Description: "Health bonus",
        Width: 40,
        Height: 16
    );

    /// <summary>Armor bonus</summary>
    public static readonly Actor ArmorBonus = new(
        Id: 2015,
        Description: "Armor bonus",
        Width: 40,
        Height: 16
    );

    /// <summary>Green armor</summary>
    public static readonly Actor GreenArmor = new(
        Id: 2018,
        Description: "Green armor",
        Width: 40,
        Height: 16
    );

    /// <summary>Blue armor</summary>
    public static readonly Actor BlueArmor = new(
        Id: 2019,
        Description: "Blue armor",
        Width: 40,
        Height: 16
    );

    /// <summary>Soulsphere</summary>
    public static readonly Actor Soulsphere = new(
        Id: 2013,
        Description: "Soulsphere",
        Width: 40,
        Height: 45
    );

    /// <summary>Invulnerability</summary>
    public static readonly Actor InvulnerabilitySphere = new(
        Id: 2022,
        Description: "Invulnerability",
        Width: 40,
        Height: 30
    );

    /// <summary>Berserk</summary>
    public static readonly Actor Berserk = new(
        Id: 2023,
        Description: "Berserk",
        Width: 40,
        Height: 16
    );

    /// <summary>Invisibility</summary>
    public static readonly Actor BlurSphere = new(
        Id: 2024,
        Description: "Invisibility",
        Width: 40,
        Height: 45
    );

    /// <summary>Radiation suit</summary>
    public static readonly Actor RadSuit = new(
        Id: 2025,
        Description: "Radiation suit",
        Width: 40,
        Height: 60
    );

    /// <summary>Computer map</summary>
    public static readonly Actor Allmap = new(
        Id: 2026,
        Description: "Computer map",
        Width: 40,
        Height: 35
    );

    /// <summary>Lite Amplification goggles</summary>
    public static readonly Actor Infrared = new(
        Id: 2045,
        Description: "Lite Amplification goggles",
        Width: 40,
        Height: 16
    );

    /// <summary>Blue keycard</summary>
    public static readonly Actor BlueCard = new(
        Id: 5,
        Description: "Blue keycard",
        Width: 40,
        Height: 16
    );

    /// <summary>Blue skullkey</summary>
    public static readonly Actor BlueSkull = new(
        Id: 40,
        Description: "Blue skullkey",
        Width: 40,
        Height: 16
    );

    /// <summary>Red keycard</summary>
    public static readonly Actor RedCard = new(
        Id: 13,
        Description: "Red keycard",
        Width: 40,
        Height: 16
    );

    /// <summary>Red skullkey</summary>
    public static readonly Actor RedSkull = new(
        Id: 38,
        Description: "Red skullkey",
        Width: 40,
        Height: 16
    );

    /// <summary>Yellow keycard</summary>
    public static readonly Actor YellowCard = new(
        Id: 6,
        Description: "Yellow keycard",
        Width: 40,
        Height: 16
    );

    /// <summary>Yellow skullkey</summary>
    public static readonly Actor YellowSkull = new(
        Id: 39,
        Description: "Yellow skullkey",
        Width: 40,
        Height: 16
    );

    /// <summary>Barrel</summary>
    public static readonly Actor ExplosiveBarrel = new(
        Id: 2035,
        Description: "Barrel",
        Width: 20,
        Height: 32
    );

    /// <summary>Tall techno pillar</summary>
    public static readonly Actor TechPillar = new(
        Id: 48,
        Description: "Tall techno pillar",
        Width: 32,
        Height: 20
    );

    /// <summary>Tall green pillar</summary>
    public static readonly Actor TallGreenColumn = new(
        Id: 30,
        Description: "Tall green pillar",
        Width: 32,
        Height: 20
    );

    /// <summary>Tall red pillar</summary>
    public static readonly Actor TallRedColumn = new(
        Id: 32,
        Description: "Tall red pillar",
        Width: 32,
        Height: 20
    );

    /// <summary>Short green pillar</summary>
    public static readonly Actor ShortGreenColumn = new(
        Id: 31,
        Description: "Short green pillar",
        Width: 32,
        Height: 20
    );

    /// <summary>Short green pillar (beating heart)</summary>
    public static readonly Actor HeartColumn = new(
        Id: 36,
        Description: "Short green pillar (beating heart)",
        Width: 32,
        Height: 20
    );

    /// <summary>Short red pillar</summary>
    public static readonly Actor ShortRedColumn = new(
        Id: 33,
        Description: "Short red pillar",
        Width: 32,
        Height: 20
    );

    /// <summary>Short red pillar (skull)</summary>
    public static readonly Actor SkullColumn = new(
        Id: 37,
        Description: "Short red pillar (skull)",
        Width: 32,
        Height: 20
    );

    /// <summary>Stalagmite</summary>
    public static readonly Actor Stalagtite = new(
        Id: 47,
        Description: "Stalagmite",
        Width: 32,
        Height: 20
    );

    /// <summary>Gray tree</summary>
    public static readonly Actor TorchTree = new(
        Id: 43,
        Description: "Gray tree",
        Width: 32,
        Height: 20
    );

    /// <summary>Large brown tree</summary>
    public static readonly Actor BigTree = new(
        Id: 54,
        Description: "Large brown tree",
        Width: 64,
        Height: 20
    );

    /// <summary>Evil Eye</summary>
    public static readonly Actor EvilEye = new(
        Id: 41,
        Description: "Evil Eye",
        Width: 32,
        Height: 20
    );

    /// <summary>Floating skull rock</summary>
    public static readonly Actor FloatingSkull = new(
        Id: 42,
        Description: "Floating skull rock",
        Width: 32,
        Height: 20
    );

    /// <summary>Floor lamp</summary>
    public static readonly Actor Column = new(
        Id: 2028,
        Description: "Floor lamp",
        Width: 32,
        Height: 16
    );

    /// <summary>Candle</summary>
    public static readonly Actor Candlestick = new(
        Id: 34,
        Description: "Candle",
        Width: 40,
        Height: 16
    );

    /// <summary>Candelabra</summary>
    public static readonly Actor Candelabra = new(
        Id: 35,
        Description: "Candelabra",
        Width: 32,
        Height: 16
    );

    /// <summary>Tall blue firestick</summary>
    public static readonly Actor BlueTorch = new(
        Id: 44,
        Description: "Tall blue firestick",
        Width: 32,
        Height: 16
    );

    /// <summary>Tall green firestick</summary>
    public static readonly Actor GreenTorch = new(
        Id: 45,
        Description: "Tall green firestick",
        Width: 32,
        Height: 16
    );

    /// <summary>Tall red firestick</summary>
    public static readonly Actor RedTorch = new(
        Id: 46,
        Description: "Tall red firestick",
        Width: 32,
        Height: 16
    );

    /// <summary>Short blue firestick</summary>
    public static readonly Actor ShortBlueTorch = new(
        Id: 55,
        Description: "Short blue firestick",
        Width: 32,
        Height: 16
    );

    /// <summary>Short green firestick</summary>
    public static readonly Actor ShortGreenTorch = new(
        Id: 56,
        Description: "Short green firestick",
        Width: 32,
        Height: 16
    );

    /// <summary>Short red firestick</summary>
    public static readonly Actor ShortRedTorch = new(
        Id: 57,
        Description: "Short red firestick",
        Width: 32,
        Height: 16
    );

    /// <summary>Hanging victim, twitching (blocking)</summary>
    public static readonly Actor BloodyTwitch = new(
        Id: 49,
        Description: "Hanging victim, twitching (blocking)",
        Width: 32,
        Height: 68
    );

    /// <summary>Hanging victim, twitching</summary>
    public static readonly Actor NonsolidTwitch = new(
        Id: 63,
        Description: "Hanging victim, twitching",
        Width: 40,
        Height: 68
    );

    /// <summary>Hanging victim, arms out (blocking)</summary>
    public static readonly Actor Meat2 = new(
        Id: 50,
        Description: "Hanging victim, arms out (blocking)",
        Width: 40,
        Height: 84
    );

    /// <summary>Hanging victim, arms out</summary>
    public static readonly Actor NonsolidMeat2 = new(
        Id: 59,
        Description: "Hanging victim, arms out",
        Width: 40,
        Height: 84
    );

    /// <summary>Hanging pair of legs (blocking)</summary>
    public static readonly Actor Meat4 = new(
        Id: 52,
        Description: "Hanging pair of legs (blocking)",
        Width: 32,
        Height: 68
    );

    /// <summary>Hanging pair of legs</summary>
    public static readonly Actor NonsolidMeat4 = new(
        Id: 60,
        Description: "Hanging pair of legs",
        Width: 40,
        Height: 68
    );

    /// <summary>Hanging victim, 1-legged (blocking)</summary>
    public static readonly Actor HangingCorpse = new(
        Id: 51,
        Description: "Hanging victim, 1-legged (blocking)",
        Width: 32,
        Height: 84
    );

    /// <summary>Hanging victim, 1-legged</summary>
    public static readonly Actor NonsolidMeat3 = new(
        Id: 61,
        Description: "Hanging victim, 1-legged",
        Width: 40,
        Height: 52
    );

    /// <summary>Hanging leg (blocking)</summary>
    public static readonly Actor Meat5 = new(
        Id: 53,
        Description: "Hanging leg (blocking)",
        Width: 32,
        Height: 52
    );

    /// <summary>Hanging leg</summary>
    public static readonly Actor NonsolidMeat5 = new(
        Id: 62,
        Description: "Hanging leg",
        Width: 40,
        Height: 52
    );

    /// <summary>Impaled human</summary>
    public static readonly Actor DeadStick = new(
        Id: 25,
        Description: "Impaled human",
        Width: 32,
        Height: 16
    );

    /// <summary>Twitching impaled human</summary>
    public static readonly Actor LiveStick = new(
        Id: 26,
        Description: "Twitching impaled human",
        Width: 32,
        Height: 16
    );

    /// <summary>Skull on a pole</summary>
    public static readonly Actor HeadOnAStick = new(
        Id: 27,
        Description: "Skull on a pole",
        Width: 32,
        Height: 16
    );

    /// <summary>5 skulls shish kebob</summary>
    public static readonly Actor HeadsOnAStick = new(
        Id: 28,
        Description: "5 skulls shish kebob",
        Width: 32,
        Height: 16
    );

    /// <summary>Pile of skulls and candles</summary>
    public static readonly Actor HeadCandles = new(
        Id: 29,
        Description: "Pile of skulls and candles",
        Width: 32,
        Height: 16
    );

    /// <summary>Bloody mess 1</summary>
    public static readonly Actor GibbedMarine = new(
        Id: 10,
        Description: "Bloody mess 1",
        Width: 40,
        Height: 16
    );

    /// <summary>Bloody mess 2</summary>
    public static readonly Actor GibbedMarineExtra = new(
        Id: 12,
        Description: "Bloody mess 2",
        Width: 40,
        Height: 16
    );

    /// <summary>Pool of blood and bones</summary>
    public static readonly Actor Gibs = new(
        Id: 24,
        Description: "Pool of blood and bones",
        Width: 40,
        Height: 16
    );

    /// <summary>Dead player</summary>
    public static readonly Actor DeadMarine = new(
        Id: 15,
        Description: "Dead player",
        Width: 40,
        Height: 16
    );

    /// <summary>Dead former human</summary>
    public static readonly Actor DeadZombieMan = new(
        Id: 18,
        Description: "Dead former human",
        Width: 40,
        Height: 16
    );

    /// <summary>Dead former sergeant</summary>
    public static readonly Actor DeadShotgunGuy = new(
        Id: 19,
        Description: "Dead former sergeant",
        Width: 40,
        Height: 16
    );

    /// <summary>Dead imp</summary>
    public static readonly Actor DeadDoomImp = new(
        Id: 20,
        Description: "Dead imp",
        Width: 40,
        Height: 16
    );

    /// <summary>Dead demon</summary>
    public static readonly Actor DeadDemon = new(
        Id: 21,
        Description: "Dead demon",
        Width: 60,
        Height: 16
    );

    /// <summary>Dead cacodemon</summary>
    public static readonly Actor DeadCacodemon = new(
        Id: 22,
        Description: "Dead cacodemon",
        Width: 62,
        Height: 16
    );

    /// <summary>Dead lost soul</summary>
    public static readonly Actor DeadLostSoul = new(
        Id: 23,
        Description: "Dead lost soul",
        Width: 40,
        Height: 16
    );

    /// <summary>Chaingunner</summary>
    public static readonly Actor ChaingunGuy = new(
        Id: 65,
        Description: "Chaingunner",
        Width: 40,
        Height: 56
    );

    /// <summary>Hell Knight</summary>
    public static readonly Actor HellKnight = new(
        Id: 69,
        Description: "Hell Knight",
        Width: 48,
        Height: 64
    );

    /// <summary>Arachnotron</summary>
    public static readonly Actor Arachnotron = new(
        Id: 68,
        Description: "Arachnotron",
        Width: 128,
        Height: 64
    );

    /// <summary>Pain Elemental</summary>
    public static readonly Actor PainElemental = new(
        Id: 71,
        Description: "Pain Elemental",
        Width: 62,
        Height: 56
    );

    /// <summary>Revenant</summary>
    public static readonly Actor Revenant = new(
        Id: 66,
        Description: "Revenant",
        Width: 40,
        Height: 56
    );

    /// <summary>Mancubus</summary>
    public static readonly Actor Fatso = new(
        Id: 67,
        Description: "Mancubus",
        Width: 96,
        Height: 64
    );

    /// <summary>Archvile</summary>
    public static readonly Actor Archvile = new(
        Id: 64,
        Description: "Archvile",
        Width: 40,
        Height: 56
    );

    /// <summary>Wolfenstein SS</summary>
    public static readonly Actor WolfensteinSS = new(
        Id: 84,
        Description: "Wolfenstein SS",
        Width: 40,
        Height: 56
    );

    /// <summary>Commander Keen</summary>
    public static readonly Actor CommanderKeen = new(
        Id: 72,
        Description: "Commander Keen",
        Width: 32,
        Height: 72
    );

    /// <summary>Icon of Sin</summary>
    public static readonly Actor BossBrain = new(
        Id: 88,
        Description: "Icon of Sin",
        Width: 32,
        Height: 16
    );

    /// <summary>Monsters Spawner</summary>
    public static readonly Actor BossEye = new(
        Id: 89,
        Description: "Monsters Spawner",
        Width: 40,
        Height: 32
    );

    /// <summary>Monsters Target</summary>
    public static readonly Actor BossTarget = new(
        Id: 87,
        Description: "Monsters Target",
        Width: 40,
        Height: 32
    );

    /// <summary>Super Shotgun</summary>
    public static readonly Actor SuperShotgun = new(
        Id: 82,
        Description: "Super Shotgun",
        Width: 40,
        Height: 25
    );

    /// <summary>Megasphere</summary>
    public static readonly Actor Megasphere = new(
        Id: 83,
        Description: "Megasphere",
        Width: 40,
        Height: 40
    );

    /// <summary>Burning barrel</summary>
    public static readonly Actor BurningBarrel = new(
        Id: 70,
        Description: "Burning barrel",
        Width: 32,
        Height: 32
    );

    /// <summary>Tall techno floor lamp</summary>
    public static readonly Actor TechLamp = new(
        Id: 85,
        Description: "Tall techno floor lamp",
        Width: 32,
        Height: 16
    );

    /// <summary>Short techno floor lamp</summary>
    public static readonly Actor TechLamp2 = new(
        Id: 86,
        Description: "Short techno floor lamp",
        Width: 32,
        Height: 16
    );

    /// <summary>Pool of blood and guts</summary>
    public static readonly Actor ColonGibs = new(
        Id: 79,
        Description: "Pool of blood and guts",
        Width: 32,
        Height: 16
    );

    /// <summary>Pool of blood</summary>
    public static readonly Actor SmallBloodPool = new(
        Id: 80,
        Description: "Pool of blood",
        Width: 32,
        Height: 16
    );

    /// <summary>Pool of brains</summary>
    public static readonly Actor BrainStem = new(
        Id: 81,
        Description: "Pool of brains",
        Width: 32,
        Height: 16
    );

    /// <summary>Hanging victim, guts removed</summary>
    public static readonly Actor HangNoGuts = new(
        Id: 73,
        Description: "Hanging victim, guts removed",
        Width: 32,
        Height: 88
    );

    /// <summary>Hanging victim, guts and brain removed</summary>
    public static readonly Actor HangBNoBrain = new(
        Id: 74,
        Description: "Hanging victim, guts and brain removed",
        Width: 32,
        Height: 88
    );

    /// <summary>Hanging torso, looking down</summary>
    public static readonly Actor HangTLookingDown = new(
        Id: 75,
        Description: "Hanging torso, looking down",
        Width: 32,
        Height: 64
    );

    /// <summary>Hanging torso, open skull</summary>
    public static readonly Actor HangTSkull = new(
        Id: 76,
        Description: "Hanging torso, open skull",
        Width: 32,
        Height: 64
    );

    /// <summary>Hanging torso, looking up</summary>
    public static readonly Actor HangTLookingUp = new(
        Id: 77,
        Description: "Hanging torso, looking up",
        Width: 32,
        Height: 64
    );

    /// <summary>Hanging torso, brain removed</summary>
    public static readonly Actor HangTNoBrain = new(
        Id: 78,
        Description: "Hanging torso, brain removed",
        Width: 32,
        Height: 64
    );

}
