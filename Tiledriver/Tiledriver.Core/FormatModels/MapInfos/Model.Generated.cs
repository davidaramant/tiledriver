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
        public Maybe<bool> ExitTextIsLump { get; } = ((bool)false).ToMaybe();
        public Maybe<bool> ExitTextIsMessage { get; } = ((bool)false).ToMaybe();
        public Cluster() { }
        public Cluster(
            Maybe<int> id,
            Maybe<string> exitText,
            Maybe<string> exitTextLookup,
            Maybe<bool> exitTextIsLump,
            Maybe<bool> exitTextIsMessage)
        {
            Id = id;
            ExitText = exitText;
            ExitTextLookup = exitTextLookup;
            ExitTextIsLump = exitTextIsLump;
            ExitTextIsMessage = exitTextIsMessage;
        }
    }

    public partial class Episode
    {
        public Maybe<string> Map { get; } = Maybe<string>.Nothing;
        public Maybe<char> Key { get; } = Maybe<char>.Nothing;
        public Maybe<string> Lookup { get; } = Maybe<string>.Nothing;
        public Maybe<string> Name { get; } = Maybe<string>.Nothing;
        public Maybe<bool> NoSkillMenu { get; } = ((bool)false).ToMaybe();
        public Maybe<bool> Optional { get; } = ((bool)false).ToMaybe();
        public Maybe<string> PicName { get; } = Maybe<string>.Nothing;
        public Maybe<bool> Remove { get; } = ((bool)false).ToMaybe();
        public Episode() { }
        public Episode(
            Maybe<string> map,
            Maybe<char> key,
            Maybe<string> lookup,
            Maybe<string> name,
            Maybe<bool> noSkillMenu,
            Maybe<bool> optional,
            Maybe<string> picName,
            Maybe<bool> remove)
        {
            Map = map;
            Key = key;
            Lookup = lookup;
            Name = name;
            NoSkillMenu = noSkillMenu;
            Optional = optional;
            PicName = picName;
            Remove = remove;
        }
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
        public IEnumerable<string> PlayerClassess { get; } = Enumerable.Empty<string>();
        public Maybe<string> PushwallSoundSequence { get; } = Maybe<string>.Nothing;
        public IEnumerable<string> QuitMessagess { get; } = Enumerable.Empty<string>();
        public Maybe<string> ScoresMusic { get; } = Maybe<string>.Nothing;
        public Maybe<string> SignOn { get; } = Maybe<string>.Nothing;
        public Maybe<string> TitleMusic { get; } = Maybe<string>.Nothing;
        public Maybe<int> TitleTime { get; } = Maybe<int>.Nothing;
        public Maybe<string> Translator { get; } = Maybe<string>.Nothing;
        public GameInfo() { }
        public GameInfo(
            Maybe<string> advisoryColor,
            Maybe<string> advisoryPic,
            Maybe<PlayScreenBorderColors> playScreenBorderColors,
            Maybe<PlayScreenBorderGraphics> playScreenBorderGraphics,
            Maybe<string> borderFlat,
            Maybe<string> doorSoundSequence,
            Maybe<bool> drawReadThis,
            Maybe<string> finaleMusic,
            Maybe<string> gamePalette,
            Maybe<double> gibFactor,
            Maybe<string> highScoresFont,
            Maybe<string> highScoresFontColor,
            Maybe<string> intermissionMusic,
            Maybe<string> menuColor,
            Maybe<string> menuFade,
            Maybe<string> menuFontColorDisabled,
            Maybe<string> menuFontColorHighlight,
            Maybe<string> menuFontColorHighlightSelection,
            Maybe<string> menuFontColorInvalid,
            Maybe<string> menuFontColorInvalidSelection,
            Maybe<string> menuFontColorLabel,
            Maybe<string> menuFontColorSelection,
            Maybe<string> menuFontColorTitle,
            Maybe<string> menuMusic,
            Maybe<string> pageIndexFontColor,
            IEnumerable<string> playerClassess,
            Maybe<string> pushwallSoundSequence,
            IEnumerable<string> quitMessagess,
            Maybe<string> scoresMusic,
            Maybe<string> signOn,
            Maybe<string> titleMusic,
            Maybe<int> titleTime,
            Maybe<string> translator)
        {
            AdvisoryColor = advisoryColor;
            AdvisoryPic = advisoryPic;
            PlayScreenBorderColors = playScreenBorderColors;
            PlayScreenBorderGraphics = playScreenBorderGraphics;
            BorderFlat = borderFlat;
            DoorSoundSequence = doorSoundSequence;
            DrawReadThis = drawReadThis;
            FinaleMusic = finaleMusic;
            GamePalette = gamePalette;
            GibFactor = gibFactor;
            HighScoresFont = highScoresFont;
            HighScoresFontColor = highScoresFontColor;
            IntermissionMusic = intermissionMusic;
            MenuColor = menuColor;
            MenuFade = menuFade;
            MenuFontColorDisabled = menuFontColorDisabled;
            MenuFontColorHighlight = menuFontColorHighlight;
            MenuFontColorHighlightSelection = menuFontColorHighlightSelection;
            MenuFontColorInvalid = menuFontColorInvalid;
            MenuFontColorInvalidSelection = menuFontColorInvalidSelection;
            MenuFontColorLabel = menuFontColorLabel;
            MenuFontColorSelection = menuFontColorSelection;
            MenuFontColorTitle = menuFontColorTitle;
            MenuMusic = menuMusic;
            PageIndexFontColor = pageIndexFontColor;
            PlayerClassess = playerClassess;
            PushwallSoundSequence = pushwallSoundSequence;
            QuitMessagess = quitMessagess;
            ScoresMusic = scoresMusic;
            SignOn = signOn;
            TitleMusic = titleMusic;
            TitleTime = titleTime;
            Translator = translator;
        }
    }

    public partial class PlayScreenBorderColors
    {
        public Maybe<string> TopColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> BottomColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> HighlightColor { get; } = Maybe<string>.Nothing;
        public PlayScreenBorderColors() { }
        public PlayScreenBorderColors(
            Maybe<string> topColor,
            Maybe<string> bottomColor,
            Maybe<string> highlightColor)
        {
            TopColor = topColor;
            BottomColor = bottomColor;
            HighlightColor = highlightColor;
        }
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
        public PlayScreenBorderGraphics() { }
        public PlayScreenBorderGraphics(
            Maybe<string> topLeft,
            Maybe<string> top,
            Maybe<string> topRight,
            Maybe<string> left,
            Maybe<string> right,
            Maybe<string> bottomLeft,
            Maybe<string> bottom,
            Maybe<string> bottomRight)
        {
            TopLeft = topLeft;
            Top = top;
            TopRight = topRight;
            Left = left;
            Right = right;
            BottomLeft = bottomLeft;
            Bottom = bottom;
            BottomRight = bottomRight;
        }
    }

    public partial class MenuColor
    {
        public Maybe<string> Border1 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Border2 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Border3 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Background { get; } = Maybe<string>.Nothing;
        public Maybe<string> Stripe { get; } = Maybe<string>.Nothing;
        public Maybe<string> StripeBg { get; } = Maybe<string>.Nothing;
        public MenuColor() { }
        public MenuColor(
            Maybe<string> border1,
            Maybe<string> border2,
            Maybe<string> border3,
            Maybe<string> background,
            Maybe<string> stripe,
            Maybe<string> stripeBg)
        {
            Border1 = border1;
            Border2 = border2;
            Border3 = border3;
            Background = background;
            Stripe = stripe;
            StripeBg = stripeBg;
        }
    }

    public partial class Intermission
    {
        public Maybe<string> Name { get; } = Maybe<string>.Nothing;
        public IEnumerable<IIntermissionAction> IntermissionActions { get; } = Enumerable.Empty<IIntermissionAction>();
        public Intermission() { }
        public Intermission(
            Maybe<string> name,
            IEnumerable<IIntermissionAction> intermissionActions)
        {
            Name = name;
            IntermissionActions = intermissionActions;
        }
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
        public IntermissionAction() { }
        public IntermissionAction(
            Maybe<string> background,
            Maybe<bool> backgroundTiled,
            Maybe<string> backgroundPalette,
            Maybe<string> draw,
            Maybe<int> drawX,
            Maybe<int> drawY,
            Maybe<string> music,
            Maybe<int> time)
        {
            Background = background;
            BackgroundTiled = backgroundTiled;
            BackgroundPalette = backgroundPalette;
            Draw = draw;
            DrawX = drawX;
            DrawY = drawY;
            Music = music;
            Time = time;
        }
    }

    public partial class Fader : IntermissionAction, IIntermissionAction
    {
        public Maybe<string> FadeType { get; } = Maybe<string>.Nothing;
        public Fader() { }
        public Fader(
            Maybe<string> background,
            Maybe<bool> backgroundTiled,
            Maybe<string> backgroundPalette,
            Maybe<string> draw,
            Maybe<int> drawX,
            Maybe<int> drawY,
            Maybe<string> music,
            Maybe<int> time,
            Maybe<string> fadeType)
            : base(
                background,
                backgroundTiled,
                backgroundPalette,
                draw,
                drawX,
                drawY,
                music,
                time)
        {
            FadeType = fadeType;
        }
    }

    public partial class GoToTitle : IIntermissionAction
    {
        public GoToTitle() { }
    }

    public partial class Image : IntermissionAction, IIntermissionAction
    {
        public Image() { }
        public Image(
            Maybe<string> background,
            Maybe<bool> backgroundTiled,
            Maybe<string> backgroundPalette,
            Maybe<string> draw,
            Maybe<int> drawX,
            Maybe<int> drawY,
            Maybe<string> music,
            Maybe<int> time)
            : base(
                background,
                backgroundTiled,
                backgroundPalette,
                draw,
                drawX,
                drawY,
                music,
                time)
        {
        }
    }

    public partial class TextScreen : IntermissionAction, IIntermissionAction
    {
        public IEnumerable<string> Texts { get; } = Enumerable.Empty<string>();
        public Maybe<string> TextAlignment { get; } = Maybe<string>.Nothing;
        public Maybe<string> TextColor { get; } = Maybe<string>.Nothing;
        public Maybe<int> TextSpeed { get; } = Maybe<int>.Nothing;
        public Maybe<int> PositionX { get; } = Maybe<int>.Nothing;
        public Maybe<int> PositionY { get; } = Maybe<int>.Nothing;
        public TextScreen() { }
        public TextScreen(
            Maybe<string> background,
            Maybe<bool> backgroundTiled,
            Maybe<string> backgroundPalette,
            Maybe<string> draw,
            Maybe<int> drawX,
            Maybe<int> drawY,
            Maybe<string> music,
            Maybe<int> time,
            IEnumerable<string> texts,
            Maybe<string> textAlignment,
            Maybe<string> textColor,
            Maybe<int> textSpeed,
            Maybe<int> positionX,
            Maybe<int> positionY)
            : base(
                background,
                backgroundTiled,
                backgroundPalette,
                draw,
                drawX,
                drawY,
                music,
                time)
        {
            Texts = texts;
            TextAlignment = textAlignment;
            TextColor = textColor;
            TextSpeed = textSpeed;
            PositionX = positionX;
            PositionY = positionY;
        }
    }

    public partial class VictoryStats : IntermissionAction, IIntermissionAction
    {
        public VictoryStats() { }
        public VictoryStats(
            Maybe<string> background,
            Maybe<bool> backgroundTiled,
            Maybe<string> backgroundPalette,
            Maybe<string> draw,
            Maybe<int> drawX,
            Maybe<int> drawY,
            Maybe<string> music,
            Maybe<int> time)
            : base(
                background,
                backgroundTiled,
                backgroundPalette,
                draw,
                drawX,
                drawY,
                music,
                time)
        {
        }
    }

    public partial class DefaultMap : BaseMap
    {
        public DefaultMap() { }
        public DefaultMap(
            Maybe<string> borderTexture,
            Maybe<int> cluster,
            Maybe<string> completionString,
            Maybe<bool> deathCam,
            Maybe<string> defaultCeiling,
            Maybe<string> defaultFloor,
            IEnumerable<string> ensureInventorys,
            Maybe<int> exitFade,
            Maybe<int> floorNumber,
            Maybe<string> highScoresGraphic,
            Maybe<int> levelBonus,
            Maybe<int> levelNum,
            Maybe<string> music,
            Maybe<bool> spawnWithWeaponRaised,
            Maybe<bool> secretDeathSounds,
            Maybe<string> next,
            Maybe<string> secretNext,
            Maybe<string> victoryNext,
            Maybe<string> nextEndSequence,
            Maybe<string> secretNextEndSequence,
            Maybe<string> victoryNextEndSequence,
            IEnumerable<SpecialAction> specialActions,
            Maybe<bool> nointermission,
            Maybe<int> par,
            Maybe<string> translator)
            : base(
                borderTexture,
                cluster,
                completionString,
                deathCam,
                defaultCeiling,
                defaultFloor,
                ensureInventorys,
                exitFade,
                floorNumber,
                highScoresGraphic,
                levelBonus,
                levelNum,
                music,
                spawnWithWeaponRaised,
                secretDeathSounds,
                next,
                secretNext,
                victoryNext,
                nextEndSequence,
                secretNextEndSequence,
                victoryNextEndSequence,
                specialActions,
                nointermission,
                par,
                translator)
        {
        }
    }

    public partial class AddDefaultMap : BaseMap
    {
        public AddDefaultMap() { }
        public AddDefaultMap(
            Maybe<string> borderTexture,
            Maybe<int> cluster,
            Maybe<string> completionString,
            Maybe<bool> deathCam,
            Maybe<string> defaultCeiling,
            Maybe<string> defaultFloor,
            IEnumerable<string> ensureInventorys,
            Maybe<int> exitFade,
            Maybe<int> floorNumber,
            Maybe<string> highScoresGraphic,
            Maybe<int> levelBonus,
            Maybe<int> levelNum,
            Maybe<string> music,
            Maybe<bool> spawnWithWeaponRaised,
            Maybe<bool> secretDeathSounds,
            Maybe<string> next,
            Maybe<string> secretNext,
            Maybe<string> victoryNext,
            Maybe<string> nextEndSequence,
            Maybe<string> secretNextEndSequence,
            Maybe<string> victoryNextEndSequence,
            IEnumerable<SpecialAction> specialActions,
            Maybe<bool> nointermission,
            Maybe<int> par,
            Maybe<string> translator)
            : base(
                borderTexture,
                cluster,
                completionString,
                deathCam,
                defaultCeiling,
                defaultFloor,
                ensureInventorys,
                exitFade,
                floorNumber,
                highScoresGraphic,
                levelBonus,
                levelNum,
                music,
                spawnWithWeaponRaised,
                secretDeathSounds,
                next,
                secretNext,
                victoryNext,
                nextEndSequence,
                secretNextEndSequence,
                victoryNextEndSequence,
                specialActions,
                nointermission,
                par,
                translator)
        {
        }
    }

    public partial class BaseMap
    {
        public Maybe<string> BorderTexture { get; } = Maybe<string>.Nothing;
        public Maybe<int> Cluster { get; } = Maybe<int>.Nothing;
        public Maybe<string> CompletionString { get; } = Maybe<string>.Nothing;
        public Maybe<bool> DeathCam { get; } = ((bool)false).ToMaybe();
        public Maybe<string> DefaultCeiling { get; } = Maybe<string>.Nothing;
        public Maybe<string> DefaultFloor { get; } = Maybe<string>.Nothing;
        public IEnumerable<string> EnsureInventorys { get; } = Enumerable.Empty<string>();
        public Maybe<int> ExitFade { get; } = Maybe<int>.Nothing;
        public Maybe<int> FloorNumber { get; } = Maybe<int>.Nothing;
        public Maybe<string> HighScoresGraphic { get; } = Maybe<string>.Nothing;
        public Maybe<int> LevelBonus { get; } = Maybe<int>.Nothing;
        public Maybe<int> LevelNum { get; } = Maybe<int>.Nothing;
        public Maybe<string> Music { get; } = Maybe<string>.Nothing;
        public Maybe<bool> SpawnWithWeaponRaised { get; } = ((bool)false).ToMaybe();
        public Maybe<bool> SecretDeathSounds { get; } = ((bool)false).ToMaybe();
        public Maybe<string> Next { get; } = Maybe<string>.Nothing;
        public Maybe<string> SecretNext { get; } = Maybe<string>.Nothing;
        public Maybe<string> VictoryNext { get; } = Maybe<string>.Nothing;
        public Maybe<string> NextEndSequence { get; } = Maybe<string>.Nothing;
        public Maybe<string> SecretNextEndSequence { get; } = Maybe<string>.Nothing;
        public Maybe<string> VictoryNextEndSequence { get; } = Maybe<string>.Nothing;
        public IEnumerable<SpecialAction> SpecialActions { get; } = Enumerable.Empty<SpecialAction>();
        public Maybe<bool> Nointermission { get; } = ((bool)false).ToMaybe();
        public Maybe<int> Par { get; } = Maybe<int>.Nothing;
        public Maybe<string> Translator { get; } = Maybe<string>.Nothing;
        public BaseMap() { }
        public BaseMap(
            Maybe<string> borderTexture,
            Maybe<int> cluster,
            Maybe<string> completionString,
            Maybe<bool> deathCam,
            Maybe<string> defaultCeiling,
            Maybe<string> defaultFloor,
            IEnumerable<string> ensureInventorys,
            Maybe<int> exitFade,
            Maybe<int> floorNumber,
            Maybe<string> highScoresGraphic,
            Maybe<int> levelBonus,
            Maybe<int> levelNum,
            Maybe<string> music,
            Maybe<bool> spawnWithWeaponRaised,
            Maybe<bool> secretDeathSounds,
            Maybe<string> next,
            Maybe<string> secretNext,
            Maybe<string> victoryNext,
            Maybe<string> nextEndSequence,
            Maybe<string> secretNextEndSequence,
            Maybe<string> victoryNextEndSequence,
            IEnumerable<SpecialAction> specialActions,
            Maybe<bool> nointermission,
            Maybe<int> par,
            Maybe<string> translator)
        {
            BorderTexture = borderTexture;
            Cluster = cluster;
            CompletionString = completionString;
            DeathCam = deathCam;
            DefaultCeiling = defaultCeiling;
            DefaultFloor = defaultFloor;
            EnsureInventorys = ensureInventorys;
            ExitFade = exitFade;
            FloorNumber = floorNumber;
            HighScoresGraphic = highScoresGraphic;
            LevelBonus = levelBonus;
            LevelNum = levelNum;
            Music = music;
            SpawnWithWeaponRaised = spawnWithWeaponRaised;
            SecretDeathSounds = secretDeathSounds;
            Next = next;
            SecretNext = secretNext;
            VictoryNext = victoryNext;
            NextEndSequence = nextEndSequence;
            SecretNextEndSequence = secretNextEndSequence;
            VictoryNextEndSequence = victoryNextEndSequence;
            SpecialActions = specialActions;
            Nointermission = nointermission;
            Par = par;
            Translator = translator;
        }
    }

    public partial class Map : BaseMap
    {
        public Maybe<string> MapLump { get; } = Maybe<string>.Nothing;
        public Maybe<string> MapName { get; } = Maybe<string>.Nothing;
        public Maybe<string> MapNameLookup { get; } = Maybe<string>.Nothing;
        public Map() { }
        public Map(
            Maybe<string> borderTexture,
            Maybe<int> cluster,
            Maybe<string> completionString,
            Maybe<bool> deathCam,
            Maybe<string> defaultCeiling,
            Maybe<string> defaultFloor,
            IEnumerable<string> ensureInventorys,
            Maybe<int> exitFade,
            Maybe<int> floorNumber,
            Maybe<string> highScoresGraphic,
            Maybe<int> levelBonus,
            Maybe<int> levelNum,
            Maybe<string> music,
            Maybe<bool> spawnWithWeaponRaised,
            Maybe<bool> secretDeathSounds,
            Maybe<string> next,
            Maybe<string> secretNext,
            Maybe<string> victoryNext,
            Maybe<string> nextEndSequence,
            Maybe<string> secretNextEndSequence,
            Maybe<string> victoryNextEndSequence,
            IEnumerable<SpecialAction> specialActions,
            Maybe<bool> nointermission,
            Maybe<int> par,
            Maybe<string> translator,
            Maybe<string> mapLump,
            Maybe<string> mapName,
            Maybe<string> mapNameLookup)
            : base(
                borderTexture,
                cluster,
                completionString,
                deathCam,
                defaultCeiling,
                defaultFloor,
                ensureInventorys,
                exitFade,
                floorNumber,
                highScoresGraphic,
                levelBonus,
                levelNum,
                music,
                spawnWithWeaponRaised,
                secretDeathSounds,
                next,
                secretNext,
                victoryNext,
                nextEndSequence,
                secretNextEndSequence,
                victoryNextEndSequence,
                specialActions,
                nointermission,
                par,
                translator)
        {
            MapLump = mapLump;
            MapName = mapName;
            MapNameLookup = mapNameLookup;
        }
    }

    public partial class SpecialAction
    {
        public Maybe<string> ActorClass { get; } = Maybe<string>.Nothing;
        public Maybe<string> Special { get; } = Maybe<string>.Nothing;
        public Maybe<string> Arg0 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Arg1 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Arg2 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Arg3 { get; } = Maybe<string>.Nothing;
        public SpecialAction() { }
        public SpecialAction(
            Maybe<string> actorClass,
            Maybe<string> special,
            Maybe<string> arg0,
            Maybe<string> arg1,
            Maybe<string> arg2,
            Maybe<string> arg3)
        {
            ActorClass = actorClass;
            Special = special;
            Arg0 = arg0;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
        }
    }

    public partial class MapInfo
    {
        public IEnumerable<Cluster> Clusters { get; } = Enumerable.Empty<Cluster>();
        public IEnumerable<Episode> Episodes { get; } = Enumerable.Empty<Episode>();
        public Maybe<GameInfo> GameInfo { get; } = Maybe<GameInfo>.Nothing;
        public IEnumerable<Intermission> Intermissions { get; } = Enumerable.Empty<Intermission>();
        public IEnumerable<Map> Maps { get; } = Enumerable.Empty<Map>();
        public MapInfo() { }
        public MapInfo(
            IEnumerable<Cluster> clusters,
            IEnumerable<Episode> episodes,
            Maybe<GameInfo> gameInfo,
            IEnumerable<Intermission> intermissions,
            IEnumerable<Map> maps)
        {
            Clusters = clusters;
            Episodes = episodes;
            GameInfo = gameInfo;
            Intermissions = intermissions;
            Maps = maps;
        }
    }

}
