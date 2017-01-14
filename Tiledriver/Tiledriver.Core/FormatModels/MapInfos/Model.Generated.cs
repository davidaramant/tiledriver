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
        public Maybe<int> Id { get; } = Maybe<int>.Nothing;
        public Maybe<string> ExitText { get; } = Maybe<string>.Nothing;
        public Maybe<string> ExitTextLookup { get; } = Maybe<string>.Nothing;
        public Maybe<bool> ExitTextIsLump { get; } = Maybe<bool>.Nothing;
        public Maybe<bool> ExitTextIsMessage { get; } = Maybe<bool>.Nothing;
    }

    public partial class Episode
    {
        public Maybe<string> Map { get; } = Maybe<string>.Nothing;
        public Maybe<char> Key { get; } = Maybe<char>.Nothing;
        public Maybe<string> Lookup { get; } = Maybe<string>.Nothing;
        public Maybe<string> Name { get; } = Maybe<string>.Nothing;
        public Maybe<bool> NoSkillMenu { get; } = Maybe<bool>.Nothing;
        public Maybe<bool> Optional { get; } = Maybe<bool>.Nothing;
        public Maybe<string> PicName { get; } = Maybe<string>.Nothing;
        public Maybe<bool> Remove { get; } = Maybe<bool>.Nothing;
    }

    public partial class GameInfo
    {
        public Maybe<string> AdvisoryColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> AdvisoryPic { get; } = Maybe<string>.Nothing;
        public Maybe<PlayScreenBorderColors> PlayScreenBorderColors { get; } = Maybe<PlayScreenBorderColors>.Nothing;
        public Maybe<PlayScreenBorderGraphics> PlayScreenBorderGraphics { get; } = Maybe<PlayScreenBorderGraphics>.Nothing;
        public Maybe<string> BorderFlat { get; } = Maybe<string>.Nothing;
        public Maybe<string> DoorSoundSequence { get; } = Maybe<string>.Nothing;
        public Maybe<bool> DrawReadThis { get; } = Maybe<bool>.Nothing;
        public Maybe<string> FinaleMusic { get; } = Maybe<string>.Nothing;
        public Maybe<string> GamePalette { get; } = Maybe<string>.Nothing;
        public Maybe<double> GibFactor { get; } = Maybe<double>.Nothing;
        public Maybe<string> HighScoresFont { get; } = Maybe<string>.Nothing;
        public Maybe<string> HighScoresFontColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> IntermissionMusic { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFade { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFontColorDisabled { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFontColorHighlight { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFontColorHighlightSelection { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFontColorInvalid { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFontColorInvalidSelection { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFontColorLabel { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFontColorSelection { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuFontColorTitle { get; } = Maybe<string>.Nothing;
        public Maybe<string> MenuMusic { get; } = Maybe<string>.Nothing;
        public Maybe<string> PageIndexFontColor { get; } = Maybe<string>.Nothing;
        public IEnumerable<string> playerClassess { get; } = Enumerable.Empty<string>();
        public Maybe<string> PushwallSoundSequence { get; } = Maybe<string>.Nothing;
        public IEnumerable<string> quitMessagess { get; } = Enumerable.Empty<string>();
        public Maybe<string> ScoresMusic { get; } = Maybe<string>.Nothing;
        public Maybe<string> SignOn { get; } = Maybe<string>.Nothing;
        public Maybe<string> TitleMusic { get; } = Maybe<string>.Nothing;
        public Maybe<int> TitleTime { get; } = Maybe<int>.Nothing;
        public Maybe<string> Translator { get; } = Maybe<string>.Nothing;
    }

    public partial class PlayScreenBorderColors
    {
        public Maybe<string> TopColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> BottomColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> HighlightColor { get; } = Maybe<string>.Nothing;
    }

    public partial class PlayScreenBorderGraphics
    {
        public Maybe<string> TopLeft { get; } = Maybe<string>.Nothing;
        public Maybe<string> Top { get; } = Maybe<string>.Nothing;
        public Maybe<string> TopRight { get; } = Maybe<string>.Nothing;
        public Maybe<string> Left { get; } = Maybe<string>.Nothing;
        public Maybe<string> Right { get; } = Maybe<string>.Nothing;
        public Maybe<string> BottomLeft { get; } = Maybe<string>.Nothing;
        public Maybe<string> Bottom { get; } = Maybe<string>.Nothing;
        public Maybe<string> BottomRight { get; } = Maybe<string>.Nothing;
    }

    public partial class MenuColor
    {
        public Maybe<string> Border1 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Border2 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Border3 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Background { get; } = Maybe<string>.Nothing;
        public Maybe<string> Stripe { get; } = Maybe<string>.Nothing;
        public Maybe<string> StripeBg { get; } = Maybe<string>.Nothing;
    }

    public partial class Intermission
    {
        public Maybe<string> Name { get; } = Maybe<string>.Nothing;
        public IEnumerable<IIntermissionAction> intermissionActions { get; } = Enumerable.Empty<IIntermissionAction>();
    }

    public partial class IntermissionAction
    {
        public Maybe<string> Background { get; } = Maybe<string>.Nothing;
        public Maybe<bool> BackgroundTiled { get; } = Maybe<bool>.Nothing;
        public Maybe<string> BackgroundPalette { get; } = Maybe<string>.Nothing;
        public Maybe<string> Draw { get; } = Maybe<string>.Nothing;
        public Maybe<int> DrawX { get; } = Maybe<int>.Nothing;
        public Maybe<int> DrawY { get; } = Maybe<int>.Nothing;
        public Maybe<string> Music { get; } = Maybe<string>.Nothing;
        public Maybe<int> Time { get; } = Maybe<int>.Nothing;
    }

    public partial class Fader : IntermissionAction, IIntermissionAction
    {
        public Maybe<string> FadeType { get; } = Maybe<string>.Nothing;
    }

    public partial class GoToTitle : IIntermissionAction
    {
    }

    public partial class Image : IntermissionAction, IIntermissionAction
    {
    }

    public partial class TextScreen : IntermissionAction, IIntermissionAction
    {
        public IEnumerable<string> texts { get; } = Enumerable.Empty<string>();
        public Maybe<string> TextAlignment { get; } = Maybe<string>.Nothing;
        public Maybe<string> TextColor { get; } = Maybe<string>.Nothing;
        public Maybe<int> TextSpeed { get; } = Maybe<int>.Nothing;
        public Maybe<int> PositionX { get; } = Maybe<int>.Nothing;
        public Maybe<int> PositionY { get; } = Maybe<int>.Nothing;
    }

    public partial class VictoryStats : IntermissionAction, IIntermissionAction
    {
    }

    public partial class DefaultMap : BaseMap
    {
    }

    public partial class AddDefaultMap : BaseMap
    {
    }

    public partial class BaseMap
    {
        public Maybe<string> BorderTexture { get; } = Maybe<string>.Nothing;
        public Maybe<int> Cluster { get; } = Maybe<int>.Nothing;
        public Maybe<string> CompletionString { get; } = Maybe<string>.Nothing;
        public Maybe<bool> DeathCam { get; } = Maybe<bool>.Nothing;
        public Maybe<string> DefaultCeiling { get; } = Maybe<string>.Nothing;
        public Maybe<string> DefaultFloor { get; } = Maybe<string>.Nothing;
        public IEnumerable<string> ensureInventorys { get; } = Enumerable.Empty<string>();
        public Maybe<int> ExitFade { get; } = Maybe<int>.Nothing;
        public Maybe<int> FloorNumber { get; } = Maybe<int>.Nothing;
        public Maybe<string> HighScoresGraphic { get; } = Maybe<string>.Nothing;
        public Maybe<int> LevelBonus { get; } = Maybe<int>.Nothing;
        public Maybe<int> LevelNum { get; } = Maybe<int>.Nothing;
        public Maybe<string> Music { get; } = Maybe<string>.Nothing;
        public Maybe<bool> SpawnWithWeaponRaised { get; } = Maybe<bool>.Nothing;
        public Maybe<bool> SecretDeathSounds { get; } = Maybe<bool>.Nothing;
        public Maybe<string> Next { get; } = Maybe<string>.Nothing;
        public Maybe<string> SecretNext { get; } = Maybe<string>.Nothing;
        public Maybe<string> VictoryNext { get; } = Maybe<string>.Nothing;
        public Maybe<string> NextEndSequence { get; } = Maybe<string>.Nothing;
        public Maybe<string> SecretNextEndSequence { get; } = Maybe<string>.Nothing;
        public Maybe<string> VictoryNextEndSequence { get; } = Maybe<string>.Nothing;
        public IEnumerable<SpecialAction> specialActions { get; } = Enumerable.Empty<SpecialAction>();
        public Maybe<bool> Nointermission { get; } = Maybe<bool>.Nothing;
        public Maybe<int> Par { get; } = Maybe<int>.Nothing;
        public Maybe<string> Translator { get; } = Maybe<string>.Nothing;
    }

    public partial class Map : BaseMap
    {
        public Maybe<string> MapLump { get; } = Maybe<string>.Nothing;
        public Maybe<string> MapName { get; } = Maybe<string>.Nothing;
        public Maybe<string> MapNameLookup { get; } = Maybe<string>.Nothing;
    }

    public partial class SpecialAction
    {
        public Maybe<string> ActorClass { get; } = Maybe<string>.Nothing;
        public Maybe<string> Special { get; } = Maybe<string>.Nothing;
        public Maybe<string> Arg0 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Arg1 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Arg2 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Arg3 { get; } = Maybe<string>.Nothing;
    }

    public partial class MapInfo
    {
        public IEnumerable<Cluster> clusters { get; } = Enumerable.Empty<Cluster>();
        public IEnumerable<Episode> episodes { get; } = Enumerable.Empty<Episode>();
        public Maybe<GameInfo> GameInfo { get; } = Maybe<GameInfo>.Nothing;
        public IEnumerable<Intermission> intermissions { get; } = Enumerable.Empty<Intermission>();
        public IEnumerable<Map> maps { get; } = Enumerable.Empty<Map>();
    }

}
