// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.MapInfo.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.MapInfo.Reading
{
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
    }
}
