// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiledriver.Core.Extensions;
using Functional.Maybe;

namespace Tiledriver.Core.FormatModels.MapInfos
{
    public partial class Cluster
    {
        public Maybe<int> Id { get; private set; }
        public Maybe<string> ExitText { get; private set; }
        public Maybe<string> ExitTextLookup { get; private set; }
        public Maybe<bool> ExitTextIsLump { get; private set; }
        public Maybe<bool> ExitTextIsMessage { get; private set; }
    }

    public partial class Episode
    {
        public Maybe<string> Map { get; private set; }
        public Maybe<char> Key { get; private set; }
        public Maybe<string> Lookup { get; private set; }
        public Maybe<string> Name { get; private set; }
        public Maybe<bool> NoSkillMenu { get; private set; }
        public Maybe<bool> Optional { get; private set; }
        public Maybe<string> PicName { get; private set; }
        public Maybe<bool> Remove { get; private set; }
    }

    public partial class GameInfo
    {
        public Maybe<string> AdvisoryColor { get; private set; }
        public Maybe<string> AdvisoryPic { get; private set; }
        public Maybe<PlayScreenBorderColors> PlayScreenBorderColors { get; private set; }
        public Maybe<PlayScreenBorderGraphics> PlayScreenBorderGraphics { get; private set; }
        public Maybe<string> BorderFlat { get; private set; }
        public Maybe<string> DoorSoundSequence { get; private set; }
        public Maybe<bool> DrawReadThis { get; private set; }
        public Maybe<string> FinaleMusic { get; private set; }
        public Maybe<string> GamePalette { get; private set; }
        public Maybe<double> GibFactor { get; private set; }
        public Maybe<string> HighScoresFont { get; private set; }
        public Maybe<string> HighScoresFontColor { get; private set; }
        public Maybe<string> IntermissionMusic { get; private set; }
        public Maybe<string> MenuColor { get; private set; }
        public Maybe<string> MenuFade { get; private set; }
        public Maybe<string> MenuFontColorDisabled { get; private set; }
        public Maybe<string> MenuFontColorHighlight { get; private set; }
        public Maybe<string> MenuFontColorHighlightSelection { get; private set; }
        public Maybe<string> MenuFontColorInvalid { get; private set; }
        public Maybe<string> MenuFontColorInvalidSelection { get; private set; }
        public Maybe<string> MenuFontColorLabel { get; private set; }
        public Maybe<string> MenuFontColorSelection { get; private set; }
        public Maybe<string> MenuFontColorTitle { get; private set; }
        public Maybe<string> MenuMusic { get; private set; }
        public Maybe<string> PageIndexFontColor { get; private set; }
        public List<string> PlayerClasses { get; } = new List<string>();
        public Maybe<string> PushwallSoundSequence { get; private set; }
        public List<string> QuitMessages { get; } = new List<string>();
        public Maybe<string> ScoresMusic { get; private set; }
        public Maybe<string> SignOn { get; private set; }
        public Maybe<string> TitleMusic { get; private set; }
        public Maybe<int> TitleTime { get; private set; }
        public Maybe<string> Translator { get; private set; }
    }

    public partial class PlayScreenBorderColors
    {
        public Maybe<string> TopColor { get; private set; }
        public Maybe<string> BottomColor { get; private set; }
        public Maybe<string> HighlightColor { get; private set; }
    }

    public partial class PlayScreenBorderGraphics
    {
        public Maybe<string> TopLeft { get; private set; }
        public Maybe<string> Top { get; private set; }
        public Maybe<string> TopRight { get; private set; }
        public Maybe<string> Left { get; private set; }
        public Maybe<string> Right { get; private set; }
        public Maybe<string> BottomLeft { get; private set; }
        public Maybe<string> Bottom { get; private set; }
        public Maybe<string> BottomRight { get; private set; }
    }

    public partial class MenuColor
    {
        public Maybe<string> Border1 { get; private set; }
        public Maybe<string> Border2 { get; private set; }
        public Maybe<string> Border3 { get; private set; }
        public Maybe<string> Background { get; private set; }
        public Maybe<string> Stripe { get; private set; }
        public Maybe<string> StripeBg { get; private set; }
    }

    public partial class Intermission
    {
        public Maybe<string> Name { get; private set; }
        public List<IIntermissionAction> IntermissionActions { get; } = new List<IIntermissionAction>();
    }

    public partial class IntermissionAction
    {
        public Maybe<string> Background { get; private set; }
        public Maybe<bool> BackgroundTiled { get; private set; }
        public Maybe<string> BackgroundPalette { get; private set; }
        public Maybe<string> Draw { get; private set; }
        public Maybe<int> DrawX { get; private set; }
        public Maybe<int> DrawY { get; private set; }
        public Maybe<string> Music { get; private set; }
        public Maybe<int> Time { get; private set; }
    }

    public partial class Fader : IntermissionAction, IIntermissionAction
    {
        public Maybe<string> FadeType { get; private set; }
    }

    public partial class GoToTitle : IIntermissionAction
    {
    }

    public partial class Image : IntermissionAction, IIntermissionAction
    {
    }

    public partial class TextScreen : IntermissionAction, IIntermissionAction
    {
        public List<string> Text { get; } = new List<string>();
        public Maybe<string> TextAlignment { get; private set; }
        public Maybe<string> TextColor { get; private set; }
        public Maybe<int> TextSpeed { get; private set; }
        public Maybe<int> PositionX { get; private set; }
        public Maybe<int> PositionY { get; private set; }
    }

    public partial class VictoryStats : IntermissionAction, IIntermissionAction
    {
    }

    public partial class DefaultMap
    {
        public Maybe<string> BorderTexture { get; private set; }
        public Maybe<int> Cluster { get; private set; }
        public Maybe<string> CompletionString { get; private set; }
        public Maybe<bool> DeathCam { get; private set; }
        public Maybe<string> DefaultCeiling { get; private set; }
        public Maybe<string> DefaultFloor { get; private set; }
        public List<string> EnsureInventory { get; } = new List<string>();
        public Maybe<int> ExitFade { get; private set; }
        public Maybe<int> FloorNumber { get; private set; }
        public Maybe<string> HighScoresGraphic { get; private set; }
        public Maybe<int> LevelBonus { get; private set; }
        public Maybe<int> LevelNum { get; private set; }
        public Maybe<string> Music { get; private set; }
        public Maybe<bool> SpawnWithWeaponRaised { get; private set; }
        public Maybe<bool> SecretDeathSounds { get; private set; }
        public Maybe<string> Next { get; private set; }
        public Maybe<string> SecretNext { get; private set; }
        public Maybe<string> VictoryNext { get; private set; }
        public Maybe<string> NextEndSequence { get; private set; }
        public Maybe<string> SecretNextEndSequence { get; private set; }
        public Maybe<string> VictoryNextEndSequence { get; private set; }
        public List<SpecialAction> SpecialActions { get; } = new List<SpecialAction>();
        public Maybe<bool> Nointermission { get; private set; }
        public Maybe<int> Par { get; private set; }
        public Maybe<string> Translator { get; private set; }
    }

    public partial class AddDefaultMap
    {
        public Maybe<string> BorderTexture { get; private set; }
        public Maybe<int> Cluster { get; private set; }
        public Maybe<string> CompletionString { get; private set; }
        public Maybe<bool> DeathCam { get; private set; }
        public Maybe<string> DefaultCeiling { get; private set; }
        public Maybe<string> DefaultFloor { get; private set; }
        public List<string> EnsureInventory { get; } = new List<string>();
        public Maybe<int> ExitFade { get; private set; }
        public Maybe<int> FloorNumber { get; private set; }
        public Maybe<string> HighScoresGraphic { get; private set; }
        public Maybe<int> LevelBonus { get; private set; }
        public Maybe<int> LevelNum { get; private set; }
        public Maybe<string> Music { get; private set; }
        public Maybe<bool> SpawnWithWeaponRaised { get; private set; }
        public Maybe<bool> SecretDeathSounds { get; private set; }
        public Maybe<string> Next { get; private set; }
        public Maybe<string> SecretNext { get; private set; }
        public Maybe<string> VictoryNext { get; private set; }
        public Maybe<string> NextEndSequence { get; private set; }
        public Maybe<string> SecretNextEndSequence { get; private set; }
        public Maybe<string> VictoryNextEndSequence { get; private set; }
        public List<SpecialAction> SpecialActions { get; } = new List<SpecialAction>();
        public Maybe<bool> Nointermission { get; private set; }
        public Maybe<int> Par { get; private set; }
        public Maybe<string> Translator { get; private set; }
    }

    public partial class Map
    {
        public Maybe<string> MapLump { get; private set; }
        public Maybe<string> MapName { get; private set; }
        public Maybe<string> MapNameLookup { get; private set; }
        public Maybe<string> BorderTexture { get; private set; }
        public Maybe<int> Cluster { get; private set; }
        public Maybe<string> CompletionString { get; private set; }
        public Maybe<bool> DeathCam { get; private set; }
        public Maybe<string> DefaultCeiling { get; private set; }
        public Maybe<string> DefaultFloor { get; private set; }
        public List<string> EnsureInventory { get; } = new List<string>();
        public Maybe<int> ExitFade { get; private set; }
        public Maybe<int> FloorNumber { get; private set; }
        public Maybe<string> HighScoresGraphic { get; private set; }
        public Maybe<int> LevelBonus { get; private set; }
        public Maybe<int> LevelNum { get; private set; }
        public Maybe<string> Music { get; private set; }
        public Maybe<bool> SpawnWithWeaponRaised { get; private set; }
        public Maybe<bool> SecretDeathSounds { get; private set; }
        public Maybe<string> Next { get; private set; }
        public Maybe<string> SecretNext { get; private set; }
        public Maybe<string> VictoryNext { get; private set; }
        public Maybe<string> NextEndSequence { get; private set; }
        public Maybe<string> SecretNextEndSequence { get; private set; }
        public Maybe<string> VictoryNextEndSequence { get; private set; }
        public List<SpecialAction> SpecialActions { get; } = new List<SpecialAction>();
        public Maybe<bool> Nointermission { get; private set; }
        public Maybe<int> Par { get; private set; }
        public Maybe<string> Translator { get; private set; }
    }

    public partial class SpecialAction
    {
        public Maybe<string> ActorClass { get; private set; }
        public Maybe<string> Special { get; private set; }
        public Maybe<string> Arg0 { get; private set; }
        public Maybe<string> Arg1 { get; private set; }
        public Maybe<string> Arg2 { get; private set; }
        public Maybe<string> Arg3 { get; private set; }
    }

    public partial class MapInfo
    {
        public List<Cluster> Clusters { get; } = new List<Cluster>();
        public List<Episode> Episodes { get; } = new List<Episode>();
        public Maybe<GameInfo> GameInfo { get; private set; }
        public List<Intermission> Intermissions { get; } = new List<Intermission>();
        public List<Map> Maps { get; } = new List<Map>();
    }

}
