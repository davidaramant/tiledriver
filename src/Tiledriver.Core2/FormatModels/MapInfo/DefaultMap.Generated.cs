#nullable enable
// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.MapInfo
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record DefaultMap(
        ImmutableList<string> EnsureInventories,
        ImmutableList<SpecialAction> SpecialActions,
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
    ) : BaseMap(
        EnsureInventories,
        SpecialActions,
        BorderTexture,
        Cluster,
        CompletionString,
        DeathCam,
        DefaultCeiling,
        DefaultFloor,
        ExitFade,
        FloorNumber,
        HighScoresGraphic,
        LevelBonus,
        LevelNum,
        Music,
        SpawnWithWeaponRaised,
        SecretDeathSounds,
        Next,
        SecretNext,
        VictoryNext,
        NoIntermission,
        Par,
        Translator
    );
}
