// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.MapInfo
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record GameInfo(
        string AdvisoryColor,
        string AdvisoryPic,
        IGameBorder Border,
        string BorderFlat,
        string DeathTransition,
        string DialogColor,
        string DoorSoundSequence,
        bool DrawReadThis,
        string FinaleFlat,
        string FinaleMusic,
        string GameColorMap,
        string GameOverPic,
        string GamePalette,
        double GibFactor,
        string HighScoresFont,
        string HighScoresFontColor,
        string IntermissionMusic,
        MenuColors MenuColors,
        string MenuFade,
        string MenuFontColorDisabled,
        string MenuFontColorHighlight,
        string MenuFontColorHighlightSelection,
        string MenuFontColorInvalid,
        string MenuFontColorInvalidSelection,
        string MenuFontColorLabel,
        string MenuFontColorSelection,
        string MenuFontColorTitle,
        string MenuMusic,
        MenuWindowColors MenuWindowColors,
        MessageColors MessageColors,
        string MessageFontColor,
        string PageIndexFontColor,
        ImmutableArray<string> PlayerClasses,
        Psyched Psyched,
        string PushwallSoundSequence,
        ImmutableArray<string> QuitMessages,
        string ScoresMusic,
        string SignOn,
        string TitleMusic,
        string TitlePage,
        string TitlePalette,
        int TitleTime,
        bool TrackHighScores,
        string Translator,
        string VictoryMusic,
        string VictoryPic
    );
}
