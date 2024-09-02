// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Tiledriver.Core.GameInfo.Doom;

[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public enum ActorCategory
{
	Player,
	Teleport,
	Monster,
	Weapon,
	Ammunition,
	Health,
	Powerup,
	Key,
	Obstacle,
	Light,
	Decoration,
}

[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Actor
{
	/// <summary>Player 1 start</summary>
	public static readonly Actor Player1Start = new(
		Id: 1,
		Description: "Player 1 start",
		Width: 32,
		Height: 56,
		ClassName: "$Player1Start",
		Category: ActorCategory.Player
	);

	/// <summary>Player 2 start</summary>
	public static readonly Actor Player2Start = new(
		Id: 2,
		Description: "Player 2 start",
		Width: 32,
		Height: 56,
		ClassName: "$Player2Start",
		Category: ActorCategory.Player
	);

	/// <summary>Player 3 start</summary>
	public static readonly Actor Player3Start = new(
		Id: 3,
		Description: "Player 3 start",
		Width: 32,
		Height: 56,
		ClassName: "$Player3Start",
		Category: ActorCategory.Player
	);

	/// <summary>Player 4 start</summary>
	public static readonly Actor Player4Start = new(
		Id: 4,
		Description: "Player 4 start",
		Width: 32,
		Height: 56,
		ClassName: "$Player4Start",
		Category: ActorCategory.Player
	);

	/// <summary>Player Deathmatch start</summary>
	public static readonly Actor DeathmatchStart = new(
		Id: 11,
		Description: "Player Deathmatch start",
		Width: 32,
		Height: 56,
		ClassName: "$DeathmatchStart",
		Category: ActorCategory.Player
	);

	/// <summary>Teleport Destination</summary>
	public static readonly Actor TeleportDest = new(
		Id: 14,
		Description: "Teleport Destination",
		Width: 32,
		Height: 56,
		ClassName: "TeleportDest",
		Category: ActorCategory.Teleport
	);

	/// <summary>Former Human</summary>
	public static readonly Actor ZombieMan = new(
		Id: 3004,
		Description: "Former Human",
		Width: 40,
		Height: 56,
		ClassName: "ZombieMan",
		Category: ActorCategory.Monster
	);

	/// <summary>Former Sergeant</summary>
	public static readonly Actor ShotgunGuy = new(
		Id: 9,
		Description: "Former Sergeant",
		Width: 40,
		Height: 56,
		ClassName: "ShotgunGuy",
		Category: ActorCategory.Monster
	);

	/// <summary>Imp</summary>
	public static readonly Actor DoomImp = new(
		Id: 3001,
		Description: "Imp",
		Width: 40,
		Height: 56,
		ClassName: "DoomImp",
		Category: ActorCategory.Monster
	);

	/// <summary>Demon</summary>
	public static readonly Actor Demon = new(
		Id: 3002,
		Description: "Demon",
		Width: 60,
		Height: 56,
		ClassName: "Demon",
		Category: ActorCategory.Monster
	);

	/// <summary>Spectre</summary>
	public static readonly Actor Spectre = new(
		Id: 58,
		Description: "Spectre",
		Width: 60,
		Height: 56,
		ClassName: "Spectre",
		Category: ActorCategory.Monster
	);

	/// <summary>Lost Soul</summary>
	public static readonly Actor LostSoul = new(
		Id: 3006,
		Description: "Lost Soul",
		Width: 32,
		Height: 56,
		ClassName: "LostSoul",
		Category: ActorCategory.Monster
	);

	/// <summary>Cacodemon</summary>
	public static readonly Actor Cacodemon = new(
		Id: 3005,
		Description: "Cacodemon",
		Width: 62,
		Height: 56,
		ClassName: "Cacodemon",
		Category: ActorCategory.Monster
	);

	/// <summary>Baron of Hell</summary>
	public static readonly Actor BaronOfHell = new(
		Id: 3003,
		Description: "Baron of Hell",
		Width: 48,
		Height: 64,
		ClassName: "BaronOfHell",
		Category: ActorCategory.Monster
	);

	/// <summary>Cyberdemon</summary>
	public static readonly Actor Cyberdemon = new(
		Id: 16,
		Description: "Cyberdemon",
		Width: 80,
		Height: 110,
		ClassName: "Cyberdemon",
		Category: ActorCategory.Monster
	);

	/// <summary>Spider Mastermind</summary>
	public static readonly Actor SpiderMastermind = new(
		Id: 7,
		Description: "Spider Mastermind",
		Width: 256,
		Height: 100,
		ClassName: "SpiderMastermind",
		Category: ActorCategory.Monster
	);

	/// <summary>Chainsaw</summary>
	public static readonly Actor Chainsaw = new(
		Id: 2005,
		Description: "Chainsaw",
		Width: 40,
		Height: 25,
		ClassName: "Chainsaw",
		Category: ActorCategory.Weapon
	);

	/// <summary>Shotgun</summary>
	public static readonly Actor Shotgun = new(
		Id: 2001,
		Description: "Shotgun",
		Width: 40,
		Height: 25,
		ClassName: "Shotgun",
		Category: ActorCategory.Weapon
	);

	/// <summary>Chaingun</summary>
	public static readonly Actor Chaingun = new(
		Id: 2002,
		Description: "Chaingun",
		Width: 40,
		Height: 25,
		ClassName: "Chaingun",
		Category: ActorCategory.Weapon
	);

	/// <summary>Rocket launcher</summary>
	public static readonly Actor RocketLauncher = new(
		Id: 2003,
		Description: "Rocket launcher",
		Width: 40,
		Height: 25,
		ClassName: "RocketLauncher",
		Category: ActorCategory.Weapon
	);

	/// <summary>Plasma gun</summary>
	public static readonly Actor PlasmaRifle = new(
		Id: 2004,
		Description: "Plasma gun",
		Width: 40,
		Height: 25,
		ClassName: "PlasmaRifle",
		Category: ActorCategory.Weapon
	);

	/// <summary>BFG9000</summary>
	public static readonly Actor BFG9000 = new(
		Id: 2006,
		Description: "BFG9000",
		Width: 40,
		Height: 30,
		ClassName: "BFG9000",
		Category: ActorCategory.Weapon
	);

	/// <summary>Ammo clip</summary>
	public static readonly Actor Clip = new(
		Id: 2007,
		Description: "Ammo clip",
		Width: 40,
		Height: 16,
		ClassName: "Clip",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Shotgun shells</summary>
	public static readonly Actor Shell = new(
		Id: 2008,
		Description: "Shotgun shells",
		Width: 40,
		Height: 16,
		ClassName: "Shell",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Rocket</summary>
	public static readonly Actor RocketAmmo = new(
		Id: 2010,
		Description: "Rocket",
		Width: 40,
		Height: 25,
		ClassName: "RocketAmmo",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Cell charge</summary>
	public static readonly Actor Cell = new(
		Id: 2047,
		Description: "Cell charge",
		Width: 40,
		Height: 16,
		ClassName: "Cell",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Box of Ammo</summary>
	public static readonly Actor ClipBox = new(
		Id: 2048,
		Description: "Box of Ammo",
		Width: 40,
		Height: 16,
		ClassName: "ClipBox",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Box of Shells</summary>
	public static readonly Actor ShellBox = new(
		Id: 2049,
		Description: "Box of Shells",
		Width: 40,
		Height: 16,
		ClassName: "ShellBox",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Box of Rockets</summary>
	public static readonly Actor RocketBox = new(
		Id: 2046,
		Description: "Box of Rockets",
		Width: 40,
		Height: 25,
		ClassName: "RocketBox",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Cell charge pack</summary>
	public static readonly Actor CellPack = new(
		Id: 17,
		Description: "Cell charge pack",
		Width: 40,
		Height: 25,
		ClassName: "CellPack",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Backpack</summary>
	public static readonly Actor Backpack = new(
		Id: 8,
		Description: "Backpack",
		Width: 40,
		Height: 16,
		ClassName: "Backpack",
		Category: ActorCategory.Ammunition
	);

	/// <summary>Stimpack</summary>
	public static readonly Actor Stimpack = new(
		Id: 2011,
		Description: "Stimpack",
		Width: 40,
		Height: 16,
		ClassName: "Stimpack",
		Category: ActorCategory.Health
	);

	/// <summary>Medikit</summary>
	public static readonly Actor Medikit = new(
		Id: 2012,
		Description: "Medikit",
		Width: 40,
		Height: 25,
		ClassName: "Medikit",
		Category: ActorCategory.Health
	);

	/// <summary>Health bonus</summary>
	public static readonly Actor HealthBonus = new(
		Id: 2014,
		Description: "Health bonus",
		Width: 40,
		Height: 16,
		ClassName: "HealthBonus",
		Category: ActorCategory.Health
	);

	/// <summary>Armor bonus</summary>
	public static readonly Actor ArmorBonus = new(
		Id: 2015,
		Description: "Armor bonus",
		Width: 40,
		Height: 16,
		ClassName: "ArmorBonus",
		Category: ActorCategory.Health
	);

	/// <summary>Green armor</summary>
	public static readonly Actor GreenArmor = new(
		Id: 2018,
		Description: "Green armor",
		Width: 40,
		Height: 16,
		ClassName: "GreenArmor",
		Category: ActorCategory.Health
	);

	/// <summary>Blue armor</summary>
	public static readonly Actor BlueArmor = new(
		Id: 2019,
		Description: "Blue armor",
		Width: 40,
		Height: 16,
		ClassName: "BlueArmor",
		Category: ActorCategory.Health
	);

	/// <summary>Soulsphere</summary>
	public static readonly Actor Soulsphere = new(
		Id: 2013,
		Description: "Soulsphere",
		Width: 40,
		Height: 45,
		ClassName: "Soulsphere",
		Category: ActorCategory.Powerup
	);

	/// <summary>Invulnerability</summary>
	public static readonly Actor InvulnerabilitySphere = new(
		Id: 2022,
		Description: "Invulnerability",
		Width: 40,
		Height: 30,
		ClassName: "InvulnerabilitySphere",
		Category: ActorCategory.Powerup
	);

	/// <summary>Berserk</summary>
	public static readonly Actor Berserk = new(
		Id: 2023,
		Description: "Berserk",
		Width: 40,
		Height: 16,
		ClassName: "Berserk",
		Category: ActorCategory.Powerup
	);

	/// <summary>Invisibility</summary>
	public static readonly Actor BlurSphere = new(
		Id: 2024,
		Description: "Invisibility",
		Width: 40,
		Height: 45,
		ClassName: "BlurSphere",
		Category: ActorCategory.Powerup
	);

	/// <summary>Radiation suit</summary>
	public static readonly Actor RadSuit = new(
		Id: 2025,
		Description: "Radiation suit",
		Width: 40,
		Height: 60,
		ClassName: "RadSuit",
		Category: ActorCategory.Powerup
	);

	/// <summary>Computer map</summary>
	public static readonly Actor Allmap = new(
		Id: 2026,
		Description: "Computer map",
		Width: 40,
		Height: 35,
		ClassName: "Allmap",
		Category: ActorCategory.Powerup
	);

	/// <summary>Lite Amplification goggles</summary>
	public static readonly Actor Infrared = new(
		Id: 2045,
		Description: "Lite Amplification goggles",
		Width: 40,
		Height: 16,
		ClassName: "Infrared",
		Category: ActorCategory.Powerup
	);

	/// <summary>Blue keycard</summary>
	public static readonly Actor BlueCard = new(
		Id: 5,
		Description: "Blue keycard",
		Width: 40,
		Height: 16,
		ClassName: "BlueCard",
		Category: ActorCategory.Key
	);

	/// <summary>Blue skullkey</summary>
	public static readonly Actor BlueSkull = new(
		Id: 40,
		Description: "Blue skullkey",
		Width: 40,
		Height: 16,
		ClassName: "BlueSkull",
		Category: ActorCategory.Key
	);

	/// <summary>Red keycard</summary>
	public static readonly Actor RedCard = new(
		Id: 13,
		Description: "Red keycard",
		Width: 40,
		Height: 16,
		ClassName: "RedCard",
		Category: ActorCategory.Key
	);

	/// <summary>Red skullkey</summary>
	public static readonly Actor RedSkull = new(
		Id: 38,
		Description: "Red skullkey",
		Width: 40,
		Height: 16,
		ClassName: "RedSkull",
		Category: ActorCategory.Key
	);

	/// <summary>Yellow keycard</summary>
	public static readonly Actor YellowCard = new(
		Id: 6,
		Description: "Yellow keycard",
		Width: 40,
		Height: 16,
		ClassName: "YellowCard",
		Category: ActorCategory.Key
	);

	/// <summary>Yellow skullkey</summary>
	public static readonly Actor YellowSkull = new(
		Id: 39,
		Description: "Yellow skullkey",
		Width: 40,
		Height: 16,
		ClassName: "YellowSkull",
		Category: ActorCategory.Key
	);

	/// <summary>Barrel</summary>
	public static readonly Actor ExplosiveBarrel = new(
		Id: 2035,
		Description: "Barrel",
		Width: 20,
		Height: 32,
		ClassName: "ExplosiveBarrel",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Tall techno pillar</summary>
	public static readonly Actor TechPillar = new(
		Id: 48,
		Description: "Tall techno pillar",
		Width: 32,
		Height: 20,
		ClassName: "TechPillar",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Tall green pillar</summary>
	public static readonly Actor TallGreenColumn = new(
		Id: 30,
		Description: "Tall green pillar",
		Width: 32,
		Height: 20,
		ClassName: "TallGreenColumn",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Tall red pillar</summary>
	public static readonly Actor TallRedColumn = new(
		Id: 32,
		Description: "Tall red pillar",
		Width: 32,
		Height: 20,
		ClassName: "TallRedColumn",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Short green pillar</summary>
	public static readonly Actor ShortGreenColumn = new(
		Id: 31,
		Description: "Short green pillar",
		Width: 32,
		Height: 20,
		ClassName: "ShortGreenColumn",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Short green pillar (beating heart)</summary>
	public static readonly Actor HeartColumn = new(
		Id: 36,
		Description: "Short green pillar (beating heart)",
		Width: 32,
		Height: 20,
		ClassName: "HeartColumn",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Short red pillar</summary>
	public static readonly Actor ShortRedColumn = new(
		Id: 33,
		Description: "Short red pillar",
		Width: 32,
		Height: 20,
		ClassName: "ShortRedColumn",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Short red pillar (skull)</summary>
	public static readonly Actor SkullColumn = new(
		Id: 37,
		Description: "Short red pillar (skull)",
		Width: 32,
		Height: 20,
		ClassName: "SkullColumn",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Stalagmite</summary>
	public static readonly Actor Stalagtite = new(
		Id: 47,
		Description: "Stalagmite",
		Width: 32,
		Height: 20,
		ClassName: "Stalagtite",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Gray tree</summary>
	public static readonly Actor TorchTree = new(
		Id: 43,
		Description: "Gray tree",
		Width: 32,
		Height: 20,
		ClassName: "TorchTree",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Large brown tree</summary>
	public static readonly Actor BigTree = new(
		Id: 54,
		Description: "Large brown tree",
		Width: 64,
		Height: 20,
		ClassName: "BigTree",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Evil Eye</summary>
	public static readonly Actor EvilEye = new(
		Id: 41,
		Description: "Evil Eye",
		Width: 32,
		Height: 20,
		ClassName: "EvilEye",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Floating skull rock</summary>
	public static readonly Actor FloatingSkull = new(
		Id: 42,
		Description: "Floating skull rock",
		Width: 32,
		Height: 20,
		ClassName: "FloatingSkull",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Floor lamp</summary>
	public static readonly Actor Column = new(
		Id: 2028,
		Description: "Floor lamp",
		Width: 32,
		Height: 16,
		ClassName: "Column",
		Category: ActorCategory.Light
	);

	/// <summary>Candle</summary>
	public static readonly Actor Candlestick = new(
		Id: 34,
		Description: "Candle",
		Width: 40,
		Height: 16,
		ClassName: "Candlestick",
		Category: ActorCategory.Light
	);

	/// <summary>Candelabra</summary>
	public static readonly Actor Candelabra = new(
		Id: 35,
		Description: "Candelabra",
		Width: 32,
		Height: 16,
		ClassName: "Candelabra",
		Category: ActorCategory.Light
	);

	/// <summary>Tall blue firestick</summary>
	public static readonly Actor BlueTorch = new(
		Id: 44,
		Description: "Tall blue firestick",
		Width: 32,
		Height: 16,
		ClassName: "BlueTorch",
		Category: ActorCategory.Light
	);

	/// <summary>Tall green firestick</summary>
	public static readonly Actor GreenTorch = new(
		Id: 45,
		Description: "Tall green firestick",
		Width: 32,
		Height: 16,
		ClassName: "GreenTorch",
		Category: ActorCategory.Light
	);

	/// <summary>Tall red firestick</summary>
	public static readonly Actor RedTorch = new(
		Id: 46,
		Description: "Tall red firestick",
		Width: 32,
		Height: 16,
		ClassName: "RedTorch",
		Category: ActorCategory.Light
	);

	/// <summary>Short blue firestick</summary>
	public static readonly Actor ShortBlueTorch = new(
		Id: 55,
		Description: "Short blue firestick",
		Width: 32,
		Height: 16,
		ClassName: "ShortBlueTorch",
		Category: ActorCategory.Light
	);

	/// <summary>Short green firestick</summary>
	public static readonly Actor ShortGreenTorch = new(
		Id: 56,
		Description: "Short green firestick",
		Width: 32,
		Height: 16,
		ClassName: "ShortGreenTorch",
		Category: ActorCategory.Light
	);

	/// <summary>Short red firestick</summary>
	public static readonly Actor ShortRedTorch = new(
		Id: 57,
		Description: "Short red firestick",
		Width: 32,
		Height: 16,
		ClassName: "ShortRedTorch",
		Category: ActorCategory.Light
	);

	/// <summary>Hanging victim, twitching (blocking)</summary>
	public static readonly Actor BloodyTwitch = new(
		Id: 49,
		Description: "Hanging victim, twitching (blocking)",
		Width: 32,
		Height: 68,
		ClassName: "BloodyTwitch",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging victim, twitching</summary>
	public static readonly Actor NonsolidTwitch = new(
		Id: 63,
		Description: "Hanging victim, twitching",
		Width: 40,
		Height: 68,
		ClassName: "NonsolidTwitch",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging victim, arms out (blocking)</summary>
	public static readonly Actor Meat2 = new(
		Id: 50,
		Description: "Hanging victim, arms out (blocking)",
		Width: 40,
		Height: 84,
		ClassName: "Meat2",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging victim, arms out</summary>
	public static readonly Actor NonsolidMeat2 = new(
		Id: 59,
		Description: "Hanging victim, arms out",
		Width: 40,
		Height: 84,
		ClassName: "NonsolidMeat2",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging pair of legs (blocking)</summary>
	public static readonly Actor Meat4 = new(
		Id: 52,
		Description: "Hanging pair of legs (blocking)",
		Width: 32,
		Height: 68,
		ClassName: "Meat4",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging pair of legs</summary>
	public static readonly Actor NonsolidMeat4 = new(
		Id: 60,
		Description: "Hanging pair of legs",
		Width: 40,
		Height: 68,
		ClassName: "NonsolidMeat4",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging victim, 1-legged (blocking)</summary>
	public static readonly Actor HangingCorpse = new(
		Id: 51,
		Description: "Hanging victim, 1-legged (blocking)",
		Width: 32,
		Height: 84,
		ClassName: "HangingCorpse",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging victim, 1-legged</summary>
	public static readonly Actor NonsolidMeat3 = new(
		Id: 61,
		Description: "Hanging victim, 1-legged",
		Width: 40,
		Height: 52,
		ClassName: "NonsolidMeat3",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging leg (blocking)</summary>
	public static readonly Actor Meat5 = new(
		Id: 53,
		Description: "Hanging leg (blocking)",
		Width: 32,
		Height: 52,
		ClassName: "Meat5",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging leg</summary>
	public static readonly Actor NonsolidMeat5 = new(
		Id: 62,
		Description: "Hanging leg",
		Width: 40,
		Height: 52,
		ClassName: "NonsolidMeat5",
		Category: ActorCategory.Decoration
	);

	/// <summary>Impaled human</summary>
	public static readonly Actor DeadStick = new(
		Id: 25,
		Description: "Impaled human",
		Width: 32,
		Height: 16,
		ClassName: "DeadStick",
		Category: ActorCategory.Decoration
	);

	/// <summary>Twitching impaled human</summary>
	public static readonly Actor LiveStick = new(
		Id: 26,
		Description: "Twitching impaled human",
		Width: 32,
		Height: 16,
		ClassName: "LiveStick",
		Category: ActorCategory.Decoration
	);

	/// <summary>Skull on a pole</summary>
	public static readonly Actor HeadOnAStick = new(
		Id: 27,
		Description: "Skull on a pole",
		Width: 32,
		Height: 16,
		ClassName: "HeadOnAStick",
		Category: ActorCategory.Decoration
	);

	/// <summary>5 skulls shish kebob</summary>
	public static readonly Actor HeadsOnAStick = new(
		Id: 28,
		Description: "5 skulls shish kebob",
		Width: 32,
		Height: 16,
		ClassName: "HeadsOnAStick",
		Category: ActorCategory.Decoration
	);

	/// <summary>Pile of skulls and candles</summary>
	public static readonly Actor HeadCandles = new(
		Id: 29,
		Description: "Pile of skulls and candles",
		Width: 32,
		Height: 16,
		ClassName: "HeadCandles",
		Category: ActorCategory.Decoration
	);

	/// <summary>Bloody mess 1</summary>
	public static readonly Actor GibbedMarine = new(
		Id: 10,
		Description: "Bloody mess 1",
		Width: 40,
		Height: 16,
		ClassName: "GibbedMarine",
		Category: ActorCategory.Decoration
	);

	/// <summary>Bloody mess 2</summary>
	public static readonly Actor GibbedMarineExtra = new(
		Id: 12,
		Description: "Bloody mess 2",
		Width: 40,
		Height: 16,
		ClassName: "GibbedMarineExtra",
		Category: ActorCategory.Decoration
	);

	/// <summary>Pool of blood and bones</summary>
	public static readonly Actor Gibs = new(
		Id: 24,
		Description: "Pool of blood and bones",
		Width: 40,
		Height: 16,
		ClassName: "Gibs",
		Category: ActorCategory.Decoration
	);

	/// <summary>Dead player</summary>
	public static readonly Actor DeadMarine = new(
		Id: 15,
		Description: "Dead player",
		Width: 40,
		Height: 16,
		ClassName: "DeadMarine",
		Category: ActorCategory.Decoration
	);

	/// <summary>Dead former human</summary>
	public static readonly Actor DeadZombieMan = new(
		Id: 18,
		Description: "Dead former human",
		Width: 40,
		Height: 16,
		ClassName: "DeadZombieMan",
		Category: ActorCategory.Decoration
	);

	/// <summary>Dead former sergeant</summary>
	public static readonly Actor DeadShotgunGuy = new(
		Id: 19,
		Description: "Dead former sergeant",
		Width: 40,
		Height: 16,
		ClassName: "DeadShotgunGuy",
		Category: ActorCategory.Decoration
	);

	/// <summary>Dead imp</summary>
	public static readonly Actor DeadDoomImp = new(
		Id: 20,
		Description: "Dead imp",
		Width: 40,
		Height: 16,
		ClassName: "DeadDoomImp",
		Category: ActorCategory.Decoration
	);

	/// <summary>Dead demon</summary>
	public static readonly Actor DeadDemon = new(
		Id: 21,
		Description: "Dead demon",
		Width: 60,
		Height: 16,
		ClassName: "DeadDemon",
		Category: ActorCategory.Decoration
	);

	/// <summary>Dead cacodemon</summary>
	public static readonly Actor DeadCacodemon = new(
		Id: 22,
		Description: "Dead cacodemon",
		Width: 62,
		Height: 16,
		ClassName: "DeadCacodemon",
		Category: ActorCategory.Decoration
	);

	/// <summary>Dead lost soul</summary>
	public static readonly Actor DeadLostSoul = new(
		Id: 23,
		Description: "Dead lost soul",
		Width: 40,
		Height: 16,
		ClassName: "DeadLostSoul",
		Category: ActorCategory.Decoration
	);

	/// <summary>Chaingunner</summary>
	public static readonly Actor ChaingunGuy = new(
		Id: 65,
		Description: "Chaingunner",
		Width: 40,
		Height: 56,
		ClassName: "ChaingunGuy",
		Category: ActorCategory.Monster
	);

	/// <summary>Hell Knight</summary>
	public static readonly Actor HellKnight = new(
		Id: 69,
		Description: "Hell Knight",
		Width: 48,
		Height: 64,
		ClassName: "HellKnight",
		Category: ActorCategory.Monster
	);

	/// <summary>Arachnotron</summary>
	public static readonly Actor Arachnotron = new(
		Id: 68,
		Description: "Arachnotron",
		Width: 128,
		Height: 64,
		ClassName: "Arachnotron",
		Category: ActorCategory.Monster
	);

	/// <summary>Pain Elemental</summary>
	public static readonly Actor PainElemental = new(
		Id: 71,
		Description: "Pain Elemental",
		Width: 62,
		Height: 56,
		ClassName: "PainElemental",
		Category: ActorCategory.Monster
	);

	/// <summary>Revenant</summary>
	public static readonly Actor Revenant = new(
		Id: 66,
		Description: "Revenant",
		Width: 40,
		Height: 56,
		ClassName: "Revenant",
		Category: ActorCategory.Monster
	);

	/// <summary>Mancubus</summary>
	public static readonly Actor Fatso = new(
		Id: 67,
		Description: "Mancubus",
		Width: 96,
		Height: 64,
		ClassName: "Fatso",
		Category: ActorCategory.Monster
	);

	/// <summary>Archvile</summary>
	public static readonly Actor Archvile = new(
		Id: 64,
		Description: "Archvile",
		Width: 40,
		Height: 56,
		ClassName: "Archvile",
		Category: ActorCategory.Monster
	);

	/// <summary>Wolfenstein SS</summary>
	public static readonly Actor WolfensteinSS = new(
		Id: 84,
		Description: "Wolfenstein SS",
		Width: 40,
		Height: 56,
		ClassName: "WolfensteinSS",
		Category: ActorCategory.Monster
	);

	/// <summary>Commander Keen</summary>
	public static readonly Actor CommanderKeen = new(
		Id: 72,
		Description: "Commander Keen",
		Width: 32,
		Height: 72,
		ClassName: "CommanderKeen",
		Category: ActorCategory.Monster
	);

	/// <summary>Icon of Sin</summary>
	public static readonly Actor BossBrain = new(
		Id: 88,
		Description: "Icon of Sin",
		Width: 32,
		Height: 16,
		ClassName: "BossBrain",
		Category: ActorCategory.Monster
	);

	/// <summary>Monsters Spawner</summary>
	public static readonly Actor BossEye = new(
		Id: 89,
		Description: "Monsters Spawner",
		Width: 40,
		Height: 32,
		ClassName: "BossEye",
		Category: ActorCategory.Monster
	);

	/// <summary>Monsters Target</summary>
	public static readonly Actor BossTarget = new(
		Id: 87,
		Description: "Monsters Target",
		Width: 40,
		Height: 32,
		ClassName: "BossTarget",
		Category: ActorCategory.Monster
	);

	/// <summary>Super Shotgun</summary>
	public static readonly Actor SuperShotgun = new(
		Id: 82,
		Description: "Super Shotgun",
		Width: 40,
		Height: 25,
		ClassName: "SuperShotgun",
		Category: ActorCategory.Weapon
	);

	/// <summary>Megasphere</summary>
	public static readonly Actor Megasphere = new(
		Id: 83,
		Description: "Megasphere",
		Width: 40,
		Height: 40,
		ClassName: "Megasphere",
		Category: ActorCategory.Powerup
	);

	/// <summary>Burning barrel</summary>
	public static readonly Actor BurningBarrel = new(
		Id: 70,
		Description: "Burning barrel",
		Width: 32,
		Height: 32,
		ClassName: "BurningBarrel",
		Category: ActorCategory.Obstacle
	);

	/// <summary>Tall techno floor lamp</summary>
	public static readonly Actor TechLamp = new(
		Id: 85,
		Description: "Tall techno floor lamp",
		Width: 32,
		Height: 16,
		ClassName: "TechLamp",
		Category: ActorCategory.Light
	);

	/// <summary>Short techno floor lamp</summary>
	public static readonly Actor TechLamp2 = new(
		Id: 86,
		Description: "Short techno floor lamp",
		Width: 32,
		Height: 16,
		ClassName: "TechLamp2",
		Category: ActorCategory.Light
	);

	/// <summary>Pool of blood and guts</summary>
	public static readonly Actor ColonGibs = new(
		Id: 79,
		Description: "Pool of blood and guts",
		Width: 32,
		Height: 16,
		ClassName: "ColonGibs",
		Category: ActorCategory.Decoration
	);

	/// <summary>Pool of blood</summary>
	public static readonly Actor SmallBloodPool = new(
		Id: 80,
		Description: "Pool of blood",
		Width: 32,
		Height: 16,
		ClassName: "SmallBloodPool",
		Category: ActorCategory.Decoration
	);

	/// <summary>Pool of brains</summary>
	public static readonly Actor BrainStem = new(
		Id: 81,
		Description: "Pool of brains",
		Width: 32,
		Height: 16,
		ClassName: "BrainStem",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging victim, guts removed</summary>
	public static readonly Actor HangNoGuts = new(
		Id: 73,
		Description: "Hanging victim, guts removed",
		Width: 32,
		Height: 88,
		ClassName: "HangNoGuts",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging victim, guts and brain removed</summary>
	public static readonly Actor HangBNoBrain = new(
		Id: 74,
		Description: "Hanging victim, guts and brain removed",
		Width: 32,
		Height: 88,
		ClassName: "HangBNoBrain",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging torso, looking down</summary>
	public static readonly Actor HangTLookingDown = new(
		Id: 75,
		Description: "Hanging torso, looking down",
		Width: 32,
		Height: 64,
		ClassName: "HangTLookingDown",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging torso, open skull</summary>
	public static readonly Actor HangTSkull = new(
		Id: 76,
		Description: "Hanging torso, open skull",
		Width: 32,
		Height: 64,
		ClassName: "HangTSkull",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging torso, looking up</summary>
	public static readonly Actor HangTLookingUp = new(
		Id: 77,
		Description: "Hanging torso, looking up",
		Width: 32,
		Height: 64,
		ClassName: "HangTLookingUp",
		Category: ActorCategory.Decoration
	);

	/// <summary>Hanging torso, brain removed</summary>
	public static readonly Actor HangTNoBrain = new(
		Id: 78,
		Description: "Hanging torso, brain removed",
		Width: 32,
		Height: 64,
		ClassName: "HangTNoBrain",
		Category: ActorCategory.Decoration
	);

	public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
	{
		{1, Player1Start},
		{2, Player2Start},
		{3, Player3Start},
		{4, Player4Start},
		{11, DeathmatchStart},
		{14, TeleportDest},
		{3004, ZombieMan},
		{9, ShotgunGuy},
		{3001, DoomImp},
		{3002, Demon},
		{58, Spectre},
		{3006, LostSoul},
		{3005, Cacodemon},
		{3003, BaronOfHell},
		{16, Cyberdemon},
		{7, SpiderMastermind},
		{2005, Chainsaw},
		{2001, Shotgun},
		{2002, Chaingun},
		{2003, RocketLauncher},
		{2004, PlasmaRifle},
		{2006, BFG9000},
		{2007, Clip},
		{2008, Shell},
		{2010, RocketAmmo},
		{2047, Cell},
		{2048, ClipBox},
		{2049, ShellBox},
		{2046, RocketBox},
		{17, CellPack},
		{8, Backpack},
		{2011, Stimpack},
		{2012, Medikit},
		{2014, HealthBonus},
		{2015, ArmorBonus},
		{2018, GreenArmor},
		{2019, BlueArmor},
		{2013, Soulsphere},
		{2022, InvulnerabilitySphere},
		{2023, Berserk},
		{2024, BlurSphere},
		{2025, RadSuit},
		{2026, Allmap},
		{2045, Infrared},
		{5, BlueCard},
		{40, BlueSkull},
		{13, RedCard},
		{38, RedSkull},
		{6, YellowCard},
		{39, YellowSkull},
		{2035, ExplosiveBarrel},
		{48, TechPillar},
		{30, TallGreenColumn},
		{32, TallRedColumn},
		{31, ShortGreenColumn},
		{36, HeartColumn},
		{33, ShortRedColumn},
		{37, SkullColumn},
		{47, Stalagtite},
		{43, TorchTree},
		{54, BigTree},
		{41, EvilEye},
		{42, FloatingSkull},
		{2028, Column},
		{34, Candlestick},
		{35, Candelabra},
		{44, BlueTorch},
		{45, GreenTorch},
		{46, RedTorch},
		{55, ShortBlueTorch},
		{56, ShortGreenTorch},
		{57, ShortRedTorch},
		{49, BloodyTwitch},
		{63, NonsolidTwitch},
		{50, Meat2},
		{59, NonsolidMeat2},
		{52, Meat4},
		{60, NonsolidMeat4},
		{51, HangingCorpse},
		{61, NonsolidMeat3},
		{53, Meat5},
		{62, NonsolidMeat5},
		{25, DeadStick},
		{26, LiveStick},
		{27, HeadOnAStick},
		{28, HeadsOnAStick},
		{29, HeadCandles},
		{10, GibbedMarine},
		{12, GibbedMarineExtra},
		{24, Gibs},
		{15, DeadMarine},
		{18, DeadZombieMan},
		{19, DeadShotgunGuy},
		{20, DeadDoomImp},
		{21, DeadDemon},
		{22, DeadCacodemon},
		{23, DeadLostSoul},
		{65, ChaingunGuy},
		{69, HellKnight},
		{68, Arachnotron},
		{71, PainElemental},
		{66, Revenant},
		{67, Fatso},
		{64, Archvile},
		{84, WolfensteinSS},
		{72, CommanderKeen},
		{88, BossBrain},
		{89, BossEye},
		{87, BossTarget},
		{82, SuperShotgun},
		{83, Megasphere},
		{70, BurningBarrel},
		{85, TechLamp},
		{86, TechLamp2},
		{79, ColonGibs},
		{80, SmallBloodPool},
		{81, BrainStem},
		{73, HangNoGuts},
		{74, HangBNoBrain},
		{75, HangTLookingDown},
		{76, HangTSkull},
		{77, HangTLookingUp},
		{78, HangTNoBrain},
	};

	public static class Player
	{
		public static Actor Player1Start => Actor.Player1Start;
		public static Actor Player2Start => Actor.Player2Start;
		public static Actor Player3Start => Actor.Player3Start;
		public static Actor Player4Start => Actor.Player4Start;
		public static Actor DeathmatchStart => Actor.DeathmatchStart;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{1, Player1Start},
			{2, Player2Start},
			{3, Player3Start},
			{4, Player4Start},
			{11, DeathmatchStart},
		};
	}

	public static class Teleport
	{
		public static Actor TeleportDest => Actor.TeleportDest;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{14, TeleportDest},
		};
	}

	public static class Monster
	{
		public static Actor ZombieMan => Actor.ZombieMan;
		public static Actor ShotgunGuy => Actor.ShotgunGuy;
		public static Actor DoomImp => Actor.DoomImp;
		public static Actor Demon => Actor.Demon;
		public static Actor Spectre => Actor.Spectre;
		public static Actor LostSoul => Actor.LostSoul;
		public static Actor Cacodemon => Actor.Cacodemon;
		public static Actor BaronOfHell => Actor.BaronOfHell;
		public static Actor Cyberdemon => Actor.Cyberdemon;
		public static Actor SpiderMastermind => Actor.SpiderMastermind;
		public static Actor ChaingunGuy => Actor.ChaingunGuy;
		public static Actor HellKnight => Actor.HellKnight;
		public static Actor Arachnotron => Actor.Arachnotron;
		public static Actor PainElemental => Actor.PainElemental;
		public static Actor Revenant => Actor.Revenant;
		public static Actor Fatso => Actor.Fatso;
		public static Actor Archvile => Actor.Archvile;
		public static Actor WolfensteinSS => Actor.WolfensteinSS;
		public static Actor CommanderKeen => Actor.CommanderKeen;
		public static Actor BossBrain => Actor.BossBrain;
		public static Actor BossEye => Actor.BossEye;
		public static Actor BossTarget => Actor.BossTarget;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{3004, ZombieMan},
			{9, ShotgunGuy},
			{3001, DoomImp},
			{3002, Demon},
			{58, Spectre},
			{3006, LostSoul},
			{3005, Cacodemon},
			{3003, BaronOfHell},
			{16, Cyberdemon},
			{7, SpiderMastermind},
			{65, ChaingunGuy},
			{69, HellKnight},
			{68, Arachnotron},
			{71, PainElemental},
			{66, Revenant},
			{67, Fatso},
			{64, Archvile},
			{84, WolfensteinSS},
			{72, CommanderKeen},
			{88, BossBrain},
			{89, BossEye},
			{87, BossTarget},
		};
	}

	public static class Weapon
	{
		public static Actor Chainsaw => Actor.Chainsaw;
		public static Actor Shotgun => Actor.Shotgun;
		public static Actor Chaingun => Actor.Chaingun;
		public static Actor RocketLauncher => Actor.RocketLauncher;
		public static Actor PlasmaRifle => Actor.PlasmaRifle;
		public static Actor BFG9000 => Actor.BFG9000;
		public static Actor SuperShotgun => Actor.SuperShotgun;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{2005, Chainsaw},
			{2001, Shotgun},
			{2002, Chaingun},
			{2003, RocketLauncher},
			{2004, PlasmaRifle},
			{2006, BFG9000},
			{82, SuperShotgun},
		};
	}

	public static class Ammunition
	{
		public static Actor Clip => Actor.Clip;
		public static Actor Shell => Actor.Shell;
		public static Actor RocketAmmo => Actor.RocketAmmo;
		public static Actor Cell => Actor.Cell;
		public static Actor ClipBox => Actor.ClipBox;
		public static Actor ShellBox => Actor.ShellBox;
		public static Actor RocketBox => Actor.RocketBox;
		public static Actor CellPack => Actor.CellPack;
		public static Actor Backpack => Actor.Backpack;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{2007, Clip},
			{2008, Shell},
			{2010, RocketAmmo},
			{2047, Cell},
			{2048, ClipBox},
			{2049, ShellBox},
			{2046, RocketBox},
			{17, CellPack},
			{8, Backpack},
		};
	}

	public static class Health
	{
		public static Actor Stimpack => Actor.Stimpack;
		public static Actor Medikit => Actor.Medikit;
		public static Actor HealthBonus => Actor.HealthBonus;
		public static Actor ArmorBonus => Actor.ArmorBonus;
		public static Actor GreenArmor => Actor.GreenArmor;
		public static Actor BlueArmor => Actor.BlueArmor;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{2011, Stimpack},
			{2012, Medikit},
			{2014, HealthBonus},
			{2015, ArmorBonus},
			{2018, GreenArmor},
			{2019, BlueArmor},
		};
	}

	public static class Powerup
	{
		public static Actor Soulsphere => Actor.Soulsphere;
		public static Actor InvulnerabilitySphere => Actor.InvulnerabilitySphere;
		public static Actor Berserk => Actor.Berserk;
		public static Actor BlurSphere => Actor.BlurSphere;
		public static Actor RadSuit => Actor.RadSuit;
		public static Actor Allmap => Actor.Allmap;
		public static Actor Infrared => Actor.Infrared;
		public static Actor Megasphere => Actor.Megasphere;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{2013, Soulsphere},
			{2022, InvulnerabilitySphere},
			{2023, Berserk},
			{2024, BlurSphere},
			{2025, RadSuit},
			{2026, Allmap},
			{2045, Infrared},
			{83, Megasphere},
		};
	}

	public static class Key
	{
		public static Actor BlueCard => Actor.BlueCard;
		public static Actor BlueSkull => Actor.BlueSkull;
		public static Actor RedCard => Actor.RedCard;
		public static Actor RedSkull => Actor.RedSkull;
		public static Actor YellowCard => Actor.YellowCard;
		public static Actor YellowSkull => Actor.YellowSkull;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{5, BlueCard},
			{40, BlueSkull},
			{13, RedCard},
			{38, RedSkull},
			{6, YellowCard},
			{39, YellowSkull},
		};
	}

	public static class Obstacle
	{
		public static Actor ExplosiveBarrel => Actor.ExplosiveBarrel;
		public static Actor TechPillar => Actor.TechPillar;
		public static Actor TallGreenColumn => Actor.TallGreenColumn;
		public static Actor TallRedColumn => Actor.TallRedColumn;
		public static Actor ShortGreenColumn => Actor.ShortGreenColumn;
		public static Actor HeartColumn => Actor.HeartColumn;
		public static Actor ShortRedColumn => Actor.ShortRedColumn;
		public static Actor SkullColumn => Actor.SkullColumn;
		public static Actor Stalagtite => Actor.Stalagtite;
		public static Actor TorchTree => Actor.TorchTree;
		public static Actor BigTree => Actor.BigTree;
		public static Actor EvilEye => Actor.EvilEye;
		public static Actor FloatingSkull => Actor.FloatingSkull;
		public static Actor BurningBarrel => Actor.BurningBarrel;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{2035, ExplosiveBarrel},
			{48, TechPillar},
			{30, TallGreenColumn},
			{32, TallRedColumn},
			{31, ShortGreenColumn},
			{36, HeartColumn},
			{33, ShortRedColumn},
			{37, SkullColumn},
			{47, Stalagtite},
			{43, TorchTree},
			{54, BigTree},
			{41, EvilEye},
			{42, FloatingSkull},
			{70, BurningBarrel},
		};
	}

	public static class Light
	{
		public static Actor Column => Actor.Column;
		public static Actor Candlestick => Actor.Candlestick;
		public static Actor Candelabra => Actor.Candelabra;
		public static Actor BlueTorch => Actor.BlueTorch;
		public static Actor GreenTorch => Actor.GreenTorch;
		public static Actor RedTorch => Actor.RedTorch;
		public static Actor ShortBlueTorch => Actor.ShortBlueTorch;
		public static Actor ShortGreenTorch => Actor.ShortGreenTorch;
		public static Actor ShortRedTorch => Actor.ShortRedTorch;
		public static Actor TechLamp => Actor.TechLamp;
		public static Actor TechLamp2 => Actor.TechLamp2;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{2028, Column},
			{34, Candlestick},
			{35, Candelabra},
			{44, BlueTorch},
			{45, GreenTorch},
			{46, RedTorch},
			{55, ShortBlueTorch},
			{56, ShortGreenTorch},
			{57, ShortRedTorch},
			{85, TechLamp},
			{86, TechLamp2},
		};
	}

	public static class Decoration
	{
		public static Actor BloodyTwitch => Actor.BloodyTwitch;
		public static Actor NonsolidTwitch => Actor.NonsolidTwitch;
		public static Actor Meat2 => Actor.Meat2;
		public static Actor NonsolidMeat2 => Actor.NonsolidMeat2;
		public static Actor Meat4 => Actor.Meat4;
		public static Actor NonsolidMeat4 => Actor.NonsolidMeat4;
		public static Actor HangingCorpse => Actor.HangingCorpse;
		public static Actor NonsolidMeat3 => Actor.NonsolidMeat3;
		public static Actor Meat5 => Actor.Meat5;
		public static Actor NonsolidMeat5 => Actor.NonsolidMeat5;
		public static Actor DeadStick => Actor.DeadStick;
		public static Actor LiveStick => Actor.LiveStick;
		public static Actor HeadOnAStick => Actor.HeadOnAStick;
		public static Actor HeadsOnAStick => Actor.HeadsOnAStick;
		public static Actor HeadCandles => Actor.HeadCandles;
		public static Actor GibbedMarine => Actor.GibbedMarine;
		public static Actor GibbedMarineExtra => Actor.GibbedMarineExtra;
		public static Actor Gibs => Actor.Gibs;
		public static Actor DeadMarine => Actor.DeadMarine;
		public static Actor DeadZombieMan => Actor.DeadZombieMan;
		public static Actor DeadShotgunGuy => Actor.DeadShotgunGuy;
		public static Actor DeadDoomImp => Actor.DeadDoomImp;
		public static Actor DeadDemon => Actor.DeadDemon;
		public static Actor DeadCacodemon => Actor.DeadCacodemon;
		public static Actor DeadLostSoul => Actor.DeadLostSoul;
		public static Actor ColonGibs => Actor.ColonGibs;
		public static Actor SmallBloodPool => Actor.SmallBloodPool;
		public static Actor BrainStem => Actor.BrainStem;
		public static Actor HangNoGuts => Actor.HangNoGuts;
		public static Actor HangBNoBrain => Actor.HangBNoBrain;
		public static Actor HangTLookingDown => Actor.HangTLookingDown;
		public static Actor HangTSkull => Actor.HangTSkull;
		public static Actor HangTLookingUp => Actor.HangTLookingUp;
		public static Actor HangTNoBrain => Actor.HangTNoBrain;

		public static readonly IReadOnlyDictionary<int, Actor> AllById = new Dictionary<int, Actor>
		{
			{49, BloodyTwitch},
			{63, NonsolidTwitch},
			{50, Meat2},
			{59, NonsolidMeat2},
			{52, Meat4},
			{60, NonsolidMeat4},
			{51, HangingCorpse},
			{61, NonsolidMeat3},
			{53, Meat5},
			{62, NonsolidMeat5},
			{25, DeadStick},
			{26, LiveStick},
			{27, HeadOnAStick},
			{28, HeadsOnAStick},
			{29, HeadCandles},
			{10, GibbedMarine},
			{12, GibbedMarineExtra},
			{24, Gibs},
			{15, DeadMarine},
			{18, DeadZombieMan},
			{19, DeadShotgunGuy},
			{20, DeadDoomImp},
			{21, DeadDemon},
			{22, DeadCacodemon},
			{23, DeadLostSoul},
			{79, ColonGibs},
			{80, SmallBloodPool},
			{81, BrainStem},
			{73, HangNoGuts},
			{74, HangBNoBrain},
			{75, HangTLookingDown},
			{76, HangTSkull},
			{77, HangTLookingUp},
			{78, HangTNoBrain},
		};
	}

}
