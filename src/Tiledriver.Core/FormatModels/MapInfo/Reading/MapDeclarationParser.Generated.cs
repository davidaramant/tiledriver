#nullable enable
// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.MapInfo.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.MapInfo.Reading;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public static partial class MapDeclarationParser
{
	private static partial DefaultMap ParseDefaultMap(ILookup<Identifier, VariableAssignment> assignmentLookup)
		=> new DefaultMap(
			EnsureInventories: ReadListAssignment(assignmentLookup, "ensureInventory"),
			SpecialActions: ReadSpecialActionAssignments(assignmentLookup),
			BorderTexture: ReadStringAssignment(assignmentLookup, "borderTexture"),
			Cluster: ReadIntAssignment(assignmentLookup, "cluster"),
			CompletionString: ReadStringAssignment(assignmentLookup, "completionString"),
			DeathCam: ReadBoolAssignment(assignmentLookup, "deathCam"),
			DefaultCeiling: ReadStringAssignment(assignmentLookup, "defaultCeiling"),
			DefaultFloor: ReadStringAssignment(assignmentLookup, "defaultFloor"),
			ExitFade: ReadExitFadeInfoAssignment(assignmentLookup, "exitFade"),
			FloorNumber: ReadIntAssignment(assignmentLookup, "floorNumber"),
			HighScoresGraphic: ReadStringAssignment(assignmentLookup, "highScoresGraphic"),
			LevelBonus: ReadIntAssignment(assignmentLookup, "levelBonus"),
			LevelNum: ReadIntAssignment(assignmentLookup, "levelNum"),
			Music: ReadStringAssignment(assignmentLookup, "music"),
			SpawnWithWeaponRaised: ReadFlag(assignmentLookup, "spawnWithWeaponRaised"),
			SecretDeathSounds: ReadBoolAssignment(assignmentLookup, "secretDeathSounds"),
			Next: ReadNextMapInfoAssignment(assignmentLookup, "next"),
			SecretNext: ReadNextMapInfoAssignment(assignmentLookup, "secretNext"),
			VictoryNext: ReadNextMapInfoAssignment(assignmentLookup, "victoryNext"),
			NoIntermission: ReadFlag(assignmentLookup, "noIntermission"),
			Par: ReadIntAssignment(assignmentLookup, "par"),
			Translator: ReadStringAssignment(assignmentLookup, "translator")
		);

	private static partial AddDefaultMap ParseAddDefaultMap(ILookup<Identifier, VariableAssignment> assignmentLookup)
		=> new AddDefaultMap(
			EnsureInventories: ReadListAssignment(assignmentLookup, "ensureInventory"),
			SpecialActions: ReadSpecialActionAssignments(assignmentLookup),
			BorderTexture: ReadStringAssignment(assignmentLookup, "borderTexture"),
			Cluster: ReadIntAssignment(assignmentLookup, "cluster"),
			CompletionString: ReadStringAssignment(assignmentLookup, "completionString"),
			DeathCam: ReadBoolAssignment(assignmentLookup, "deathCam"),
			DefaultCeiling: ReadStringAssignment(assignmentLookup, "defaultCeiling"),
			DefaultFloor: ReadStringAssignment(assignmentLookup, "defaultFloor"),
			ExitFade: ReadExitFadeInfoAssignment(assignmentLookup, "exitFade"),
			FloorNumber: ReadIntAssignment(assignmentLookup, "floorNumber"),
			HighScoresGraphic: ReadStringAssignment(assignmentLookup, "highScoresGraphic"),
			LevelBonus: ReadIntAssignment(assignmentLookup, "levelBonus"),
			LevelNum: ReadIntAssignment(assignmentLookup, "levelNum"),
			Music: ReadStringAssignment(assignmentLookup, "music"),
			SpawnWithWeaponRaised: ReadFlag(assignmentLookup, "spawnWithWeaponRaised"),
			SecretDeathSounds: ReadBoolAssignment(assignmentLookup, "secretDeathSounds"),
			Next: ReadNextMapInfoAssignment(assignmentLookup, "next"),
			SecretNext: ReadNextMapInfoAssignment(assignmentLookup, "secretNext"),
			VictoryNext: ReadNextMapInfoAssignment(assignmentLookup, "victoryNext"),
			NoIntermission: ReadFlag(assignmentLookup, "noIntermission"),
			Par: ReadIntAssignment(assignmentLookup, "par"),
			Translator: ReadStringAssignment(assignmentLookup, "translator")
		);

	private static partial Map ParseMap(
		ILookup<Identifier, VariableAssignment> assignmentLookup,
		string mapLump,
		string? mapName,
		bool isMapNameLookup,
		DefaultMap defaultMap) =>
		new Map(
			MapLump: mapLump,
			EnsureInventories: ReadListAssignment(assignmentLookup, "ensureInventory").AddRange(defaultMap.EnsureInventories),
			SpecialActions: ReadSpecialActionAssignments(assignmentLookup).AddRange(defaultMap.SpecialActions),
			MapName: mapName,
			IsMapNameLookup: isMapNameLookup,
			BorderTexture: ReadStringAssignment(assignmentLookup, "borderTexture") ?? defaultMap.BorderTexture,
			Cluster: ReadIntAssignment(assignmentLookup, "cluster") ?? defaultMap.Cluster,
			CompletionString: ReadStringAssignment(assignmentLookup, "completionString") ?? defaultMap.CompletionString,
			DeathCam: ReadBoolAssignment(assignmentLookup, "deathCam") ?? defaultMap.DeathCam,
			DefaultCeiling: ReadStringAssignment(assignmentLookup, "defaultCeiling") ?? defaultMap.DefaultCeiling,
			DefaultFloor: ReadStringAssignment(assignmentLookup, "defaultFloor") ?? defaultMap.DefaultFloor,
			ExitFade: ReadExitFadeInfoAssignment(assignmentLookup, "exitFade") ?? defaultMap.ExitFade,
			FloorNumber: ReadIntAssignment(assignmentLookup, "floorNumber") ?? defaultMap.FloorNumber,
			HighScoresGraphic: ReadStringAssignment(assignmentLookup, "highScoresGraphic") ?? defaultMap.HighScoresGraphic,
			LevelBonus: ReadIntAssignment(assignmentLookup, "levelBonus") ?? defaultMap.LevelBonus,
			LevelNum: ReadIntAssignment(assignmentLookup, "levelNum") ?? defaultMap.LevelNum,
			Music: ReadStringAssignment(assignmentLookup, "music") ?? defaultMap.Music,
			SpawnWithWeaponRaised: ReadFlag(assignmentLookup, "spawnWithWeaponRaised") ?? defaultMap.SpawnWithWeaponRaised,
			SecretDeathSounds: ReadBoolAssignment(assignmentLookup, "secretDeathSounds") ?? defaultMap.SecretDeathSounds,
			Next: ReadNextMapInfoAssignment(assignmentLookup, "next") ?? defaultMap.Next,
			SecretNext: ReadNextMapInfoAssignment(assignmentLookup, "secretNext") ?? defaultMap.SecretNext,
			VictoryNext: ReadNextMapInfoAssignment(assignmentLookup, "victoryNext") ?? defaultMap.VictoryNext,
			NoIntermission: ReadFlag(assignmentLookup, "noIntermission") ?? defaultMap.NoIntermission,
			Par: ReadIntAssignment(assignmentLookup, "par") ?? defaultMap.Par,
			Translator: ReadStringAssignment(assignmentLookup, "translator") ?? defaultMap.Translator
		);

	private static partial DefaultMap UpdateDefaultMap(DefaultMap defaultMap, AddDefaultMap addDefaultMap) =>
		new DefaultMap(
			EnsureInventories: defaultMap.EnsureInventories.AddRange(addDefaultMap.EnsureInventories),
			SpecialActions: defaultMap.SpecialActions.AddRange(addDefaultMap.SpecialActions),
			BorderTexture: addDefaultMap.BorderTexture ?? defaultMap.BorderTexture,
			Cluster: addDefaultMap.Cluster ?? defaultMap.Cluster,
			CompletionString: addDefaultMap.CompletionString ?? defaultMap.CompletionString,
			DeathCam: addDefaultMap.DeathCam ?? defaultMap.DeathCam,
			DefaultCeiling: addDefaultMap.DefaultCeiling ?? defaultMap.DefaultCeiling,
			DefaultFloor: addDefaultMap.DefaultFloor ?? defaultMap.DefaultFloor,
			ExitFade: addDefaultMap.ExitFade ?? defaultMap.ExitFade,
			FloorNumber: addDefaultMap.FloorNumber ?? defaultMap.FloorNumber,
			HighScoresGraphic: addDefaultMap.HighScoresGraphic ?? defaultMap.HighScoresGraphic,
			LevelBonus: addDefaultMap.LevelBonus ?? defaultMap.LevelBonus,
			LevelNum: addDefaultMap.LevelNum ?? defaultMap.LevelNum,
			Music: addDefaultMap.Music ?? defaultMap.Music,
			SpawnWithWeaponRaised: addDefaultMap.SpawnWithWeaponRaised ?? defaultMap.SpawnWithWeaponRaised,
			SecretDeathSounds: addDefaultMap.SecretDeathSounds ?? defaultMap.SecretDeathSounds,
			Next: addDefaultMap.Next ?? defaultMap.Next,
			SecretNext: addDefaultMap.SecretNext ?? defaultMap.SecretNext,
			VictoryNext: addDefaultMap.VictoryNext ?? defaultMap.VictoryNext,
			NoIntermission: addDefaultMap.NoIntermission ?? defaultMap.NoIntermission,
			Par: addDefaultMap.Par ?? defaultMap.Par,
			Translator: addDefaultMap.Translator ?? defaultMap.Translator
		);
}
