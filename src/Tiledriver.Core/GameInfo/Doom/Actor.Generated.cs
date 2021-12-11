// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Tiledriver.Core.GameInfo.Doom;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Actor
{
    /// <summary>Player 1 start</summary>
    public static readonly Actor Player1Start = new(1, "Player 1 start");

    /// <summary>Player 2 start</summary>
    public static readonly Actor Player2Start = new(2, "Player 2 start");

    /// <summary>Player 3 start</summary>
    public static readonly Actor Player3Start = new(3, "Player 3 start");

    /// <summary>Player 4 start</summary>
    public static readonly Actor Player4Start = new(4, "Player 4 start");

    /// <summary>Player Deathmatch start</summary>
    public static readonly Actor DeathmatchStart = new(11, "Player Deathmatch start");

    /// <summary>Teleport Destination</summary>
    public static readonly Actor TeleportDest = new(14, "Teleport Destination");

    /// <summary>Former Human</summary>
    public static readonly Actor ZombieMan = new(3004, "Former Human");

    /// <summary>Former Sergeant</summary>
    public static readonly Actor ShotgunGuy = new(9, "Former Sergeant");

    /// <summary>Imp</summary>
    public static readonly Actor DoomImp = new(3001, "Imp");

    /// <summary>Demon</summary>
    public static readonly Actor Demon = new(3002, "Demon");

    /// <summary>Spectre</summary>
    public static readonly Actor Spectre = new(58, "Spectre");

    /// <summary>Lost Soul</summary>
    public static readonly Actor LostSoul = new(3006, "Lost Soul");

    /// <summary>Cacodemon</summary>
    public static readonly Actor Cacodemon = new(3005, "Cacodemon");

    /// <summary>Baron of Hell</summary>
    public static readonly Actor BaronOfHell = new(3003, "Baron of Hell");

    /// <summary>Cyberdemon</summary>
    public static readonly Actor Cyberdemon = new(16, "Cyberdemon");

    /// <summary>Spider Mastermind</summary>
    public static readonly Actor SpiderMastermind = new(7, "Spider Mastermind");

    /// <summary>Chainsaw</summary>
    public static readonly Actor Chainsaw = new(2005, "Chainsaw");

    /// <summary>Shotgun</summary>
    public static readonly Actor Shotgun = new(2001, "Shotgun");

    /// <summary>Chaingun</summary>
    public static readonly Actor Chaingun = new(2002, "Chaingun");

    /// <summary>Rocket launcher</summary>
    public static readonly Actor RocketLauncher = new(2003, "Rocket launcher");

    /// <summary>Plasma gun</summary>
    public static readonly Actor PlasmaRifle = new(2004, "Plasma gun");

    /// <summary>BFG9000</summary>
    public static readonly Actor BFG9000 = new(2006, "BFG9000");

    /// <summary>Ammo clip</summary>
    public static readonly Actor Clip = new(2007, "Ammo clip");

    /// <summary>Shotgun shells</summary>
    public static readonly Actor Shell = new(2008, "Shotgun shells");

    /// <summary>Rocket</summary>
    public static readonly Actor RocketAmmo = new(2010, "Rocket");

    /// <summary>Cell charge</summary>
    public static readonly Actor Cell = new(2047, "Cell charge");

    /// <summary>Box of Ammo</summary>
    public static readonly Actor ClipBox = new(2048, "Box of Ammo");

    /// <summary>Box of Shells</summary>
    public static readonly Actor ShellBox = new(2049, "Box of Shells");

    /// <summary>Box of Rockets</summary>
    public static readonly Actor RocketBox = new(2046, "Box of Rockets");

    /// <summary>Cell charge pack</summary>
    public static readonly Actor CellPack = new(17, "Cell charge pack");

    /// <summary>Backpack</summary>
    public static readonly Actor Backpack = new(8, "Backpack");

    /// <summary>Stimpack</summary>
    public static readonly Actor Stimpack = new(2011, "Stimpack");

    /// <summary>Medikit</summary>
    public static readonly Actor Medikit = new(2012, "Medikit");

    /// <summary>Health bonus</summary>
    public static readonly Actor HealthBonus = new(2014, "Health bonus");

    /// <summary>Armor bonus</summary>
    public static readonly Actor ArmorBonus = new(2015, "Armor bonus");

    /// <summary>Green armor</summary>
    public static readonly Actor GreenArmor = new(2018, "Green armor");

    /// <summary>Blue armor</summary>
    public static readonly Actor BlueArmor = new(2019, "Blue armor");

    /// <summary>Soulsphere</summary>
    public static readonly Actor Soulsphere = new(2013, "Soulsphere");

    /// <summary>Invulnerability</summary>
    public static readonly Actor InvulnerabilitySphere = new(2022, "Invulnerability");

    /// <summary>Berserk</summary>
    public static readonly Actor Berserk = new(2023, "Berserk");

    /// <summary>Invisibility</summary>
    public static readonly Actor BlurSphere = new(2024, "Invisibility");

    /// <summary>Radiation suit</summary>
    public static readonly Actor RadSuit = new(2025, "Radiation suit");

    /// <summary>Computer map</summary>
    public static readonly Actor Allmap = new(2026, "Computer map");

    /// <summary>Lite Amplification goggles</summary>
    public static readonly Actor Infrared = new(2045, "Lite Amplification goggles");

    /// <summary>Blue keycard</summary>
    public static readonly Actor BlueCard = new(5, "Blue keycard");

    /// <summary>Blue skullkey</summary>
    public static readonly Actor BlueSkull = new(40, "Blue skullkey");

    /// <summary>Red keycard</summary>
    public static readonly Actor RedCard = new(13, "Red keycard");

    /// <summary>Red skullkey</summary>
    public static readonly Actor RedSkull = new(38, "Red skullkey");

    /// <summary>Yellow keycard</summary>
    public static readonly Actor YellowCard = new(6, "Yellow keycard");

    /// <summary>Yellow skullkey</summary>
    public static readonly Actor YellowSkull = new(39, "Yellow skullkey");

    /// <summary>Barrel</summary>
    public static readonly Actor ExplosiveBarrel = new(2035, "Barrel");

    /// <summary>Tall techno pillar</summary>
    public static readonly Actor TechPillar = new(48, "Tall techno pillar");

    /// <summary>Tall green pillar</summary>
    public static readonly Actor TallGreenColumn = new(30, "Tall green pillar");

    /// <summary>Tall red pillar</summary>
    public static readonly Actor TallRedColumn = new(32, "Tall red pillar");

    /// <summary>Short green pillar</summary>
    public static readonly Actor ShortGreenColumn = new(31, "Short green pillar");

    /// <summary>Short green pillar (beating heart)</summary>
    public static readonly Actor HeartColumn = new(36, "Short green pillar (beating heart)");

    /// <summary>Short red pillar</summary>
    public static readonly Actor ShortRedColumn = new(33, "Short red pillar");

    /// <summary>Short red pillar (skull)</summary>
    public static readonly Actor SkullColumn = new(37, "Short red pillar (skull)");

    /// <summary>Stalagmite</summary>
    public static readonly Actor Stalagtite = new(47, "Stalagmite");

    /// <summary>Gray tree</summary>
    public static readonly Actor TorchTree = new(43, "Gray tree");

    /// <summary>Large brown tree</summary>
    public static readonly Actor BigTree = new(54, "Large brown tree");

    /// <summary>Evil Eye</summary>
    public static readonly Actor EvilEye = new(41, "Evil Eye");

    /// <summary>Floating skull rock</summary>
    public static readonly Actor FloatingSkull = new(42, "Floating skull rock");

    /// <summary>Floor lamp</summary>
    public static readonly Actor Column = new(2028, "Floor lamp");

    /// <summary>Candle</summary>
    public static readonly Actor Candlestick = new(34, "Candle");

    /// <summary>Candelabra</summary>
    public static readonly Actor Candelabra = new(35, "Candelabra");

    /// <summary>Tall blue firestick</summary>
    public static readonly Actor BlueTorch = new(44, "Tall blue firestick");

    /// <summary>Tall green firestick</summary>
    public static readonly Actor GreenTorch = new(45, "Tall green firestick");

    /// <summary>Tall red firestick</summary>
    public static readonly Actor RedTorch = new(46, "Tall red firestick");

    /// <summary>Short blue firestick</summary>
    public static readonly Actor ShortBlueTorch = new(55, "Short blue firestick");

    /// <summary>Short green firestick</summary>
    public static readonly Actor ShortGreenTorch = new(56, "Short green firestick");

    /// <summary>Short red firestick</summary>
    public static readonly Actor ShortRedTorch = new(57, "Short red firestick");

    /// <summary>Hanging victim, twitching (blocking)</summary>
    public static readonly Actor BloodyTwitch = new(49, "Hanging victim, twitching (blocking)");

    /// <summary>Hanging victim, twitching</summary>
    public static readonly Actor NonsolidTwitch = new(63, "Hanging victim, twitching");

    /// <summary>Hanging victim, arms out (blocking)</summary>
    public static readonly Actor Meat2 = new(50, "Hanging victim, arms out (blocking)");

    /// <summary>Hanging victim, arms out</summary>
    public static readonly Actor NonsolidMeat2 = new(59, "Hanging victim, arms out");

    /// <summary>Hanging pair of legs (blocking)</summary>
    public static readonly Actor Meat4 = new(52, "Hanging pair of legs (blocking)");

    /// <summary>Hanging pair of legs</summary>
    public static readonly Actor NonsolidMeat4 = new(60, "Hanging pair of legs");

    /// <summary>Hanging victim, 1-legged (blocking)</summary>
    public static readonly Actor HangingCorpse = new(51, "Hanging victim, 1-legged (blocking)");

    /// <summary>Hanging victim, 1-legged</summary>
    public static readonly Actor NonsolidMeat3 = new(61, "Hanging victim, 1-legged");

    /// <summary>Hanging leg (blocking)</summary>
    public static readonly Actor Meat5 = new(53, "Hanging leg (blocking)");

    /// <summary>Hanging leg</summary>
    public static readonly Actor NonsolidMeat5 = new(62, "Hanging leg");

    /// <summary>Impaled human</summary>
    public static readonly Actor DeadStick = new(25, "Impaled human");

    /// <summary>Twitching impaled human</summary>
    public static readonly Actor LiveStick = new(26, "Twitching impaled human");

    /// <summary>Skull on a pole</summary>
    public static readonly Actor HeadOnAStick = new(27, "Skull on a pole");

    /// <summary>5 skulls shish kebob</summary>
    public static readonly Actor HeadsOnAStick = new(28, "5 skulls shish kebob");

    /// <summary>Pile of skulls and candles</summary>
    public static readonly Actor HeadCandles = new(29, "Pile of skulls and candles");

    /// <summary>Bloody mess 1</summary>
    public static readonly Actor GibbedMarine = new(10, "Bloody mess 1");

    /// <summary>Bloody mess 2</summary>
    public static readonly Actor GibbedMarineExtra = new(12, "Bloody mess 2");

    /// <summary>Pool of blood and bones</summary>
    public static readonly Actor Gibs = new(24, "Pool of blood and bones");

    /// <summary>Dead player</summary>
    public static readonly Actor DeadMarine = new(15, "Dead player");

    /// <summary>Dead former human</summary>
    public static readonly Actor DeadZombieMan = new(18, "Dead former human");

    /// <summary>Dead former sergeant</summary>
    public static readonly Actor DeadShotgunGuy = new(19, "Dead former sergeant");

    /// <summary>Dead imp</summary>
    public static readonly Actor DeadDoomImp = new(20, "Dead imp");

    /// <summary>Dead demon</summary>
    public static readonly Actor DeadDemon = new(21, "Dead demon");

    /// <summary>Dead cacodemon</summary>
    public static readonly Actor DeadCacodemon = new(22, "Dead cacodemon");

    /// <summary>Dead lost soul</summary>
    public static readonly Actor DeadLostSoul = new(23, "Dead lost soul");

    /// <summary>Chaingunner</summary>
    public static readonly Actor ChaingunGuy = new(65, "Chaingunner");

    /// <summary>Hell Knight</summary>
    public static readonly Actor HellKnight = new(69, "Hell Knight");

    /// <summary>Arachnotron</summary>
    public static readonly Actor Arachnotron = new(68, "Arachnotron");

    /// <summary>Pain Elemental</summary>
    public static readonly Actor PainElemental = new(71, "Pain Elemental");

    /// <summary>Revenant</summary>
    public static readonly Actor Revenant = new(66, "Revenant");

    /// <summary>Mancubus</summary>
    public static readonly Actor Fatso = new(67, "Mancubus");

    /// <summary>Archvile</summary>
    public static readonly Actor Archvile = new(64, "Archvile");

    /// <summary>Wolfenstein SS</summary>
    public static readonly Actor WolfensteinSS = new(84, "Wolfenstein SS");

    /// <summary>Commander Keen</summary>
    public static readonly Actor CommanderKeen = new(72, "Commander Keen");

    /// <summary>Icon of Sin</summary>
    public static readonly Actor BossBrain = new(88, "Icon of Sin");

    /// <summary>Monsters Spawner</summary>
    public static readonly Actor BossEye = new(89, "Monsters Spawner");

    /// <summary>Monsters Target</summary>
    public static readonly Actor BossTarget = new(87, "Monsters Target");

    /// <summary>Super Shotgun</summary>
    public static readonly Actor SuperShotgun = new(82, "Super Shotgun");

    /// <summary>Megasphere</summary>
    public static readonly Actor Megasphere = new(83, "Megasphere");

    /// <summary>Burning barrel</summary>
    public static readonly Actor BurningBarrel = new(70, "Burning barrel");

    /// <summary>Tall techno floor lamp</summary>
    public static readonly Actor TechLamp = new(85, "Tall techno floor lamp");

    /// <summary>Short techno floor lamp</summary>
    public static readonly Actor TechLamp2 = new(86, "Short techno floor lamp");

    /// <summary>Pool of blood and guts</summary>
    public static readonly Actor ColonGibs = new(79, "Pool of blood and guts");

    /// <summary>Pool of blood</summary>
    public static readonly Actor SmallBloodPool = new(80, "Pool of blood");

    /// <summary>Pool of brains</summary>
    public static readonly Actor BrainStem = new(81, "Pool of brains");

    /// <summary>Hanging victim, guts removed</summary>
    public static readonly Actor HangNoGuts = new(73, "Hanging victim, guts removed");

    /// <summary>Hanging victim, guts and brain removed</summary>
    public static readonly Actor HangBNoBrain = new(74, "Hanging victim, guts and brain removed");

    /// <summary>Hanging torso, looking down</summary>
    public static readonly Actor HangTLookingDown = new(75, "Hanging torso, looking down");

    /// <summary>Hanging torso, open skull</summary>
    public static readonly Actor HangTSkull = new(76, "Hanging torso, open skull");

    /// <summary>Hanging torso, looking up</summary>
    public static readonly Actor HangTLookingUp = new(77, "Hanging torso, looking up");

    /// <summary>Hanging torso, brain removed</summary>
    public static readonly Actor HangTNoBrain = new(78, "Hanging torso, brain removed");

}
