#nullable enable
// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public abstract partial record BaseMap(
	ImmutableArray<string> EnsureInventories,
	ImmutableArray<SpecialAction> SpecialActions,
	string? BorderTexture = null,
	int? Cluster = null,
	string? CompletionString = null,
	bool? DeathCam = null,
	string? DefaultCeiling = null,
	string? DefaultFloor = null,
	ExitFadeInfo? ExitFade = null,
	int? FloorNumber = null,
	string? HighScoresGraphic = null,
	int? LevelBonus = null,
	int? LevelNum = null,
	string? Music = null,
	bool? SpawnWithWeaponRaised = null,
	bool? SecretDeathSounds = null,
	NextMapInfo? Next = null,
	NextMapInfo? SecretNext = null,
	NextMapInfo? VictoryNext = null,
	bool? NoIntermission = null,
	int? Par = null,
	string? Translator = null
);
