﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Functional.Maybe;

namespace Tiledriver.Core.FormatModels.MapInfos
{
    public sealed partial class Cluster
    {
        public Maybe<int> Id { get; } = Maybe<int>.Nothing;
        public Maybe<ClusterExitText> ExitText { get; } = Maybe<ClusterExitText>.Nothing;
        public Maybe<bool> ExitTextIsLump { get; } = ((bool)false).ToMaybe();
        public Maybe<bool> ExitTextIsMessage { get; } = ((bool)false).ToMaybe();
        public static Cluster Default = new Cluster();
        private Cluster() { }
        public Cluster(
            Maybe<int> id,
            Maybe<ClusterExitText> exitText,
            Maybe<bool> exitTextIsLump,
            Maybe<bool> exitTextIsMessage)
        {
            Id = id;
            ExitText = exitText;
            ExitTextIsLump = exitTextIsLump;
            ExitTextIsMessage = exitTextIsMessage;
        }
        public Cluster WithId( int id )
        {
            return new Cluster(
                id: id.ToMaybe(),
                exitText: ExitText,
                exitTextIsLump: ExitTextIsLump,
                exitTextIsMessage: ExitTextIsMessage);
        }
        public Cluster WithExitText( ClusterExitText exitText )
        {
            return new Cluster(
                id: Id,
                exitText: exitText.ToMaybe(),
                exitTextIsLump: ExitTextIsLump,
                exitTextIsMessage: ExitTextIsMessage);
        }
        public Cluster WithExitTextIsLump( bool exitTextIsLump )
        {
            return new Cluster(
                id: Id,
                exitText: ExitText,
                exitTextIsLump: exitTextIsLump.ToMaybe(),
                exitTextIsMessage: ExitTextIsMessage);
        }
        public Cluster WithExitTextIsMessage( bool exitTextIsMessage )
        {
            return new Cluster(
                id: Id,
                exitText: ExitText,
                exitTextIsLump: ExitTextIsLump,
                exitTextIsMessage: exitTextIsMessage.ToMaybe());
        }
    }

    public sealed partial class ClusterExitText
    {
        public Maybe<string> Text { get; } = Maybe<string>.Nothing;
        public Maybe<bool> Lookup { get; } = Maybe<bool>.Nothing;
        public static ClusterExitText Default = new ClusterExitText();
        private ClusterExitText() { }
        public ClusterExitText(
            Maybe<string> text,
            Maybe<bool> lookup)
        {
            Text = text;
            Lookup = lookup;
        }
        public ClusterExitText WithText( string text )
        {
            return new ClusterExitText(
                text: text.ToMaybe(),
                lookup: Lookup);
        }
        public ClusterExitText WithLookup( bool lookup )
        {
            return new ClusterExitText(
                text: Text,
                lookup: lookup.ToMaybe());
        }
    }

    public sealed partial class Episode
    {
        public Maybe<string> Map { get; } = Maybe<string>.Nothing;
        public Maybe<char> Key { get; } = Maybe<char>.Nothing;
        public Maybe<string> Lookup { get; } = Maybe<string>.Nothing;
        public Maybe<string> Name { get; } = Maybe<string>.Nothing;
        public Maybe<bool> NoSkillMenu { get; } = ((bool)false).ToMaybe();
        public Maybe<bool> Optional { get; } = ((bool)false).ToMaybe();
        public Maybe<string> PicName { get; } = Maybe<string>.Nothing;
        public Maybe<bool> Remove { get; } = ((bool)false).ToMaybe();
        public static Episode Default = new Episode();
        private Episode() { }
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
        public Episode WithMap( string map )
        {
            return new Episode(
                map: map.ToMaybe(),
                key: Key,
                lookup: Lookup,
                name: Name,
                noSkillMenu: NoSkillMenu,
                optional: Optional,
                picName: PicName,
                remove: Remove);
        }
        public Episode WithKey( char key )
        {
            return new Episode(
                map: Map,
                key: key.ToMaybe(),
                lookup: Lookup,
                name: Name,
                noSkillMenu: NoSkillMenu,
                optional: Optional,
                picName: PicName,
                remove: Remove);
        }
        public Episode WithLookup( string lookup )
        {
            return new Episode(
                map: Map,
                key: Key,
                lookup: lookup.ToMaybe(),
                name: Name,
                noSkillMenu: NoSkillMenu,
                optional: Optional,
                picName: PicName,
                remove: Remove);
        }
        public Episode WithName( string name )
        {
            return new Episode(
                map: Map,
                key: Key,
                lookup: Lookup,
                name: name.ToMaybe(),
                noSkillMenu: NoSkillMenu,
                optional: Optional,
                picName: PicName,
                remove: Remove);
        }
        public Episode WithNoSkillMenu( bool noSkillMenu )
        {
            return new Episode(
                map: Map,
                key: Key,
                lookup: Lookup,
                name: Name,
                noSkillMenu: noSkillMenu.ToMaybe(),
                optional: Optional,
                picName: PicName,
                remove: Remove);
        }
        public Episode WithOptional( bool optional )
        {
            return new Episode(
                map: Map,
                key: Key,
                lookup: Lookup,
                name: Name,
                noSkillMenu: NoSkillMenu,
                optional: optional.ToMaybe(),
                picName: PicName,
                remove: Remove);
        }
        public Episode WithPicName( string picName )
        {
            return new Episode(
                map: Map,
                key: Key,
                lookup: Lookup,
                name: Name,
                noSkillMenu: NoSkillMenu,
                optional: Optional,
                picName: picName.ToMaybe(),
                remove: Remove);
        }
        public Episode WithRemove( bool remove )
        {
            return new Episode(
                map: Map,
                key: Key,
                lookup: Lookup,
                name: Name,
                noSkillMenu: NoSkillMenu,
                optional: Optional,
                picName: PicName,
                remove: remove.ToMaybe());
        }
    }

    public sealed partial class GameInfo
    {
        public Maybe<string> AdvisoryColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> AdvisoryPic { get; } = Maybe<string>.Nothing;
        public Maybe<GameBorder> Border { get; } = Maybe<GameBorder>.Nothing;
        public Maybe<string> BorderFlat { get; } = Maybe<string>.Nothing;
        public Maybe<string> DoorSoundSequence { get; } = Maybe<string>.Nothing;
        public Maybe<bool> DrawReadThis { get; } = Maybe<bool>.Nothing;
        public Maybe<string> FinaleMusic { get; } = Maybe<string>.Nothing;
        public Maybe<string> GamePalette { get; } = Maybe<string>.Nothing;
        public Maybe<double> GibFactor { get; } = Maybe<double>.Nothing;
        public Maybe<string> HighScoresFont { get; } = Maybe<string>.Nothing;
        public Maybe<string> HighScoresFontColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> IntermissionMusic { get; } = Maybe<string>.Nothing;
        public Maybe<MenuColor> MenuColor { get; } = Maybe<MenuColor>.Nothing;
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
        public ImmutableList<string> PlayerClasses { get; } = ImmutableList<string>.Empty;
        public Maybe<string> PushwallSoundSequence { get; } = Maybe<string>.Nothing;
        public ImmutableList<string> QuitMessages { get; } = ImmutableList<string>.Empty;
        public Maybe<string> ScoresMusic { get; } = Maybe<string>.Nothing;
        public Maybe<string> SignOn { get; } = Maybe<string>.Nothing;
        public Maybe<string> TitleMusic { get; } = Maybe<string>.Nothing;
        public Maybe<int> TitleTime { get; } = Maybe<int>.Nothing;
        public Maybe<string> Translator { get; } = Maybe<string>.Nothing;
        public static GameInfo Default = new GameInfo();
        private GameInfo() { }
        public GameInfo(
            Maybe<string> advisoryColor,
            Maybe<string> advisoryPic,
            Maybe<GameBorder> border,
            Maybe<string> borderFlat,
            Maybe<string> doorSoundSequence,
            Maybe<bool> drawReadThis,
            Maybe<string> finaleMusic,
            Maybe<string> gamePalette,
            Maybe<double> gibFactor,
            Maybe<string> highScoresFont,
            Maybe<string> highScoresFontColor,
            Maybe<string> intermissionMusic,
            Maybe<MenuColor> menuColor,
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
            IEnumerable<string> playerClasses,
            Maybe<string> pushwallSoundSequence,
            IEnumerable<string> quitMessages,
            Maybe<string> scoresMusic,
            Maybe<string> signOn,
            Maybe<string> titleMusic,
            Maybe<int> titleTime,
            Maybe<string> translator)
        {
            AdvisoryColor = advisoryColor;
            AdvisoryPic = advisoryPic;
            Border = border;
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
            PlayerClasses = playerClasses.ToImmutableList();
            PushwallSoundSequence = pushwallSoundSequence;
            QuitMessages = quitMessages.ToImmutableList();
            ScoresMusic = scoresMusic;
            SignOn = signOn;
            TitleMusic = titleMusic;
            TitleTime = titleTime;
            Translator = translator;
        }
        public GameInfo WithAdvisoryColor( string advisoryColor )
        {
            return new GameInfo(
                advisoryColor: advisoryColor.ToMaybe(),
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithAdvisoryPic( string advisoryPic )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: advisoryPic.ToMaybe(),
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithBorder( GameBorder border )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: border.ToMaybe(),
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithBorderFlat( string borderFlat )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: borderFlat.ToMaybe(),
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithDoorSoundSequence( string doorSoundSequence )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: doorSoundSequence.ToMaybe(),
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithDrawReadThis( bool drawReadThis )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: drawReadThis.ToMaybe(),
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithFinaleMusic( string finaleMusic )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: finaleMusic.ToMaybe(),
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithGamePalette( string gamePalette )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: gamePalette.ToMaybe(),
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithGibFactor( double gibFactor )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: gibFactor.ToMaybe(),
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithHighScoresFont( string highScoresFont )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: highScoresFont.ToMaybe(),
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithHighScoresFontColor( string highScoresFontColor )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: highScoresFontColor.ToMaybe(),
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithIntermissionMusic( string intermissionMusic )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: intermissionMusic.ToMaybe(),
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuColor( MenuColor menuColor )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: menuColor.ToMaybe(),
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFade( string menuFade )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: menuFade.ToMaybe(),
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFontColorDisabled( string menuFontColorDisabled )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: menuFontColorDisabled.ToMaybe(),
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFontColorHighlight( string menuFontColorHighlight )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: menuFontColorHighlight.ToMaybe(),
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFontColorHighlightSelection( string menuFontColorHighlightSelection )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: menuFontColorHighlightSelection.ToMaybe(),
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFontColorInvalid( string menuFontColorInvalid )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: menuFontColorInvalid.ToMaybe(),
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFontColorInvalidSelection( string menuFontColorInvalidSelection )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: menuFontColorInvalidSelection.ToMaybe(),
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFontColorLabel( string menuFontColorLabel )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: menuFontColorLabel.ToMaybe(),
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFontColorSelection( string menuFontColorSelection )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: menuFontColorSelection.ToMaybe(),
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuFontColorTitle( string menuFontColorTitle )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: menuFontColorTitle.ToMaybe(),
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithMenuMusic( string menuMusic )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: menuMusic.ToMaybe(),
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithPageIndexFontColor( string pageIndexFontColor )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: pageIndexFontColor.ToMaybe(),
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithPlayerClasses( IEnumerable<string> playerClasses )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: playerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithAdditionalPlayerClass( string playerClass )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses.Add(playerClass),
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithPushwallSoundSequence( string pushwallSoundSequence )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: pushwallSoundSequence.ToMaybe(),
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithQuitMessages( IEnumerable<string> quitMessages )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: quitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithAdditionalQuitMessage( string quitMessage )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages.Add(quitMessage),
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithScoresMusic( string scoresMusic )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: scoresMusic.ToMaybe(),
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithSignOn( string signOn )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: signOn.ToMaybe(),
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithTitleMusic( string titleMusic )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: titleMusic.ToMaybe(),
                titleTime: TitleTime,
                translator: Translator);
        }
        public GameInfo WithTitleTime( int titleTime )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: titleTime.ToMaybe(),
                translator: Translator);
        }
        public GameInfo WithTranslator( string translator )
        {
            return new GameInfo(
                advisoryColor: AdvisoryColor,
                advisoryPic: AdvisoryPic,
                border: Border,
                borderFlat: BorderFlat,
                doorSoundSequence: DoorSoundSequence,
                drawReadThis: DrawReadThis,
                finaleMusic: FinaleMusic,
                gamePalette: GamePalette,
                gibFactor: GibFactor,
                highScoresFont: HighScoresFont,
                highScoresFontColor: HighScoresFontColor,
                intermissionMusic: IntermissionMusic,
                menuColor: MenuColor,
                menuFade: MenuFade,
                menuFontColorDisabled: MenuFontColorDisabled,
                menuFontColorHighlight: MenuFontColorHighlight,
                menuFontColorHighlightSelection: MenuFontColorHighlightSelection,
                menuFontColorInvalid: MenuFontColorInvalid,
                menuFontColorInvalidSelection: MenuFontColorInvalidSelection,
                menuFontColorLabel: MenuFontColorLabel,
                menuFontColorSelection: MenuFontColorSelection,
                menuFontColorTitle: MenuFontColorTitle,
                menuMusic: MenuMusic,
                pageIndexFontColor: PageIndexFontColor,
                playerClasses: PlayerClasses,
                pushwallSoundSequence: PushwallSoundSequence,
                quitMessages: QuitMessages,
                scoresMusic: ScoresMusic,
                signOn: SignOn,
                titleMusic: TitleMusic,
                titleTime: TitleTime,
                translator: translator.ToMaybe());
        }
        public GameInfo WithGameInfo( GameInfo gameInfo )
        {
            return new GameInfo(
                advisoryColor: gameInfo.AdvisoryColor.Or(AdvisoryColor),
                advisoryPic: gameInfo.AdvisoryPic.Or(AdvisoryPic),
                border: gameInfo.Border.Or(Border),
                borderFlat: gameInfo.BorderFlat.Or(BorderFlat),
                doorSoundSequence: gameInfo.DoorSoundSequence.Or(DoorSoundSequence),
                drawReadThis: gameInfo.DrawReadThis.Or(DrawReadThis),
                finaleMusic: gameInfo.FinaleMusic.Or(FinaleMusic),
                gamePalette: gameInfo.GamePalette.Or(GamePalette),
                gibFactor: gameInfo.GibFactor.Or(GibFactor),
                highScoresFont: gameInfo.HighScoresFont.Or(HighScoresFont),
                highScoresFontColor: gameInfo.HighScoresFontColor.Or(HighScoresFontColor),
                intermissionMusic: gameInfo.IntermissionMusic.Or(IntermissionMusic),
                menuColor: gameInfo.MenuColor.Or(MenuColor),
                menuFade: gameInfo.MenuFade.Or(MenuFade),
                menuFontColorDisabled: gameInfo.MenuFontColorDisabled.Or(MenuFontColorDisabled),
                menuFontColorHighlight: gameInfo.MenuFontColorHighlight.Or(MenuFontColorHighlight),
                menuFontColorHighlightSelection: gameInfo.MenuFontColorHighlightSelection.Or(MenuFontColorHighlightSelection),
                menuFontColorInvalid: gameInfo.MenuFontColorInvalid.Or(MenuFontColorInvalid),
                menuFontColorInvalidSelection: gameInfo.MenuFontColorInvalidSelection.Or(MenuFontColorInvalidSelection),
                menuFontColorLabel: gameInfo.MenuFontColorLabel.Or(MenuFontColorLabel),
                menuFontColorSelection: gameInfo.MenuFontColorSelection.Or(MenuFontColorSelection),
                menuFontColorTitle: gameInfo.MenuFontColorTitle.Or(MenuFontColorTitle),
                menuMusic: gameInfo.MenuMusic.Or(MenuMusic),
                pageIndexFontColor: gameInfo.PageIndexFontColor.Or(PageIndexFontColor),
                playerClasses: PlayerClasses.AddRange(gameInfo.PlayerClasses),
                pushwallSoundSequence: gameInfo.PushwallSoundSequence.Or(PushwallSoundSequence),
                quitMessages: QuitMessages.AddRange(gameInfo.QuitMessages),
                scoresMusic: gameInfo.ScoresMusic.Or(ScoresMusic),
                signOn: gameInfo.SignOn.Or(SignOn),
                titleMusic: gameInfo.TitleMusic.Or(TitleMusic),
                titleTime: gameInfo.TitleTime.Or(TitleTime),
                translator: gameInfo.Translator.Or(Translator));
        }
    }

    public sealed partial class GameBorder
    {
        public Maybe<GameBorderColors> Colors { get; } = Maybe<GameBorderColors>.Nothing;
        public Maybe<GameBorderGraphics> Graphics { get; } = Maybe<GameBorderGraphics>.Nothing;
        public static GameBorder Default = new GameBorder();
        private GameBorder() { }
        public GameBorder(
            Maybe<GameBorderColors> colors,
            Maybe<GameBorderGraphics> graphics)
        {
            Colors = colors;
            Graphics = graphics;
        }
        public GameBorder WithColors( GameBorderColors colors )
        {
            return new GameBorder(
                colors: colors.ToMaybe(),
                graphics: Graphics);
        }
        public GameBorder WithGraphics( GameBorderGraphics graphics )
        {
            return new GameBorder(
                colors: Colors,
                graphics: graphics.ToMaybe());
        }
    }

    public sealed partial class GameBorderColors
    {
        public Maybe<string> TopColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> BottomColor { get; } = Maybe<string>.Nothing;
        public Maybe<string> HighlightColor { get; } = Maybe<string>.Nothing;
        public static GameBorderColors Default = new GameBorderColors();
        private GameBorderColors() { }
        public GameBorderColors(
            Maybe<string> topColor,
            Maybe<string> bottomColor,
            Maybe<string> highlightColor)
        {
            TopColor = topColor;
            BottomColor = bottomColor;
            HighlightColor = highlightColor;
        }
        public GameBorderColors WithTopColor( string topColor )
        {
            return new GameBorderColors(
                topColor: topColor.ToMaybe(),
                bottomColor: BottomColor,
                highlightColor: HighlightColor);
        }
        public GameBorderColors WithBottomColor( string bottomColor )
        {
            return new GameBorderColors(
                topColor: TopColor,
                bottomColor: bottomColor.ToMaybe(),
                highlightColor: HighlightColor);
        }
        public GameBorderColors WithHighlightColor( string highlightColor )
        {
            return new GameBorderColors(
                topColor: TopColor,
                bottomColor: BottomColor,
                highlightColor: highlightColor.ToMaybe());
        }
    }

    public sealed partial class GameBorderGraphics
    {
        public Maybe<int> Offset { get; } = Maybe<int>.Nothing;
        public Maybe<string> TopLeft { get; } = Maybe<string>.Nothing;
        public Maybe<string> Top { get; } = Maybe<string>.Nothing;
        public Maybe<string> TopRight { get; } = Maybe<string>.Nothing;
        public Maybe<string> Left { get; } = Maybe<string>.Nothing;
        public Maybe<string> Right { get; } = Maybe<string>.Nothing;
        public Maybe<string> BottomLeft { get; } = Maybe<string>.Nothing;
        public Maybe<string> Bottom { get; } = Maybe<string>.Nothing;
        public Maybe<string> BottomRight { get; } = Maybe<string>.Nothing;
        public static GameBorderGraphics Default = new GameBorderGraphics();
        private GameBorderGraphics() { }
        public GameBorderGraphics(
            Maybe<int> offset,
            Maybe<string> topLeft,
            Maybe<string> top,
            Maybe<string> topRight,
            Maybe<string> left,
            Maybe<string> right,
            Maybe<string> bottomLeft,
            Maybe<string> bottom,
            Maybe<string> bottomRight)
        {
            Offset = offset;
            TopLeft = topLeft;
            Top = top;
            TopRight = topRight;
            Left = left;
            Right = right;
            BottomLeft = bottomLeft;
            Bottom = bottom;
            BottomRight = bottomRight;
        }
        public GameBorderGraphics WithOffset( int offset )
        {
            return new GameBorderGraphics(
                offset: offset.ToMaybe(),
                topLeft: TopLeft,
                top: Top,
                topRight: TopRight,
                left: Left,
                right: Right,
                bottomLeft: BottomLeft,
                bottom: Bottom,
                bottomRight: BottomRight);
        }
        public GameBorderGraphics WithTopLeft( string topLeft )
        {
            return new GameBorderGraphics(
                offset: Offset,
                topLeft: topLeft.ToMaybe(),
                top: Top,
                topRight: TopRight,
                left: Left,
                right: Right,
                bottomLeft: BottomLeft,
                bottom: Bottom,
                bottomRight: BottomRight);
        }
        public GameBorderGraphics WithTop( string top )
        {
            return new GameBorderGraphics(
                offset: Offset,
                topLeft: TopLeft,
                top: top.ToMaybe(),
                topRight: TopRight,
                left: Left,
                right: Right,
                bottomLeft: BottomLeft,
                bottom: Bottom,
                bottomRight: BottomRight);
        }
        public GameBorderGraphics WithTopRight( string topRight )
        {
            return new GameBorderGraphics(
                offset: Offset,
                topLeft: TopLeft,
                top: Top,
                topRight: topRight.ToMaybe(),
                left: Left,
                right: Right,
                bottomLeft: BottomLeft,
                bottom: Bottom,
                bottomRight: BottomRight);
        }
        public GameBorderGraphics WithLeft( string left )
        {
            return new GameBorderGraphics(
                offset: Offset,
                topLeft: TopLeft,
                top: Top,
                topRight: TopRight,
                left: left.ToMaybe(),
                right: Right,
                bottomLeft: BottomLeft,
                bottom: Bottom,
                bottomRight: BottomRight);
        }
        public GameBorderGraphics WithRight( string right )
        {
            return new GameBorderGraphics(
                offset: Offset,
                topLeft: TopLeft,
                top: Top,
                topRight: TopRight,
                left: Left,
                right: right.ToMaybe(),
                bottomLeft: BottomLeft,
                bottom: Bottom,
                bottomRight: BottomRight);
        }
        public GameBorderGraphics WithBottomLeft( string bottomLeft )
        {
            return new GameBorderGraphics(
                offset: Offset,
                topLeft: TopLeft,
                top: Top,
                topRight: TopRight,
                left: Left,
                right: Right,
                bottomLeft: bottomLeft.ToMaybe(),
                bottom: Bottom,
                bottomRight: BottomRight);
        }
        public GameBorderGraphics WithBottom( string bottom )
        {
            return new GameBorderGraphics(
                offset: Offset,
                topLeft: TopLeft,
                top: Top,
                topRight: TopRight,
                left: Left,
                right: Right,
                bottomLeft: BottomLeft,
                bottom: bottom.ToMaybe(),
                bottomRight: BottomRight);
        }
        public GameBorderGraphics WithBottomRight( string bottomRight )
        {
            return new GameBorderGraphics(
                offset: Offset,
                topLeft: TopLeft,
                top: Top,
                topRight: TopRight,
                left: Left,
                right: Right,
                bottomLeft: BottomLeft,
                bottom: Bottom,
                bottomRight: bottomRight.ToMaybe());
        }
    }

    public sealed partial class MenuColor
    {
        public Maybe<string> Border1 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Border2 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Border3 { get; } = Maybe<string>.Nothing;
        public Maybe<string> Background { get; } = Maybe<string>.Nothing;
        public Maybe<string> Stripe { get; } = Maybe<string>.Nothing;
        public Maybe<string> StripeBg { get; } = Maybe<string>.Nothing;
        public static MenuColor Default = new MenuColor();
        private MenuColor() { }
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
        public MenuColor WithBorder1( string border1 )
        {
            return new MenuColor(
                border1: border1.ToMaybe(),
                border2: Border2,
                border3: Border3,
                background: Background,
                stripe: Stripe,
                stripeBg: StripeBg);
        }
        public MenuColor WithBorder2( string border2 )
        {
            return new MenuColor(
                border1: Border1,
                border2: border2.ToMaybe(),
                border3: Border3,
                background: Background,
                stripe: Stripe,
                stripeBg: StripeBg);
        }
        public MenuColor WithBorder3( string border3 )
        {
            return new MenuColor(
                border1: Border1,
                border2: Border2,
                border3: border3.ToMaybe(),
                background: Background,
                stripe: Stripe,
                stripeBg: StripeBg);
        }
        public MenuColor WithBackground( string background )
        {
            return new MenuColor(
                border1: Border1,
                border2: Border2,
                border3: Border3,
                background: background.ToMaybe(),
                stripe: Stripe,
                stripeBg: StripeBg);
        }
        public MenuColor WithStripe( string stripe )
        {
            return new MenuColor(
                border1: Border1,
                border2: Border2,
                border3: Border3,
                background: Background,
                stripe: stripe.ToMaybe(),
                stripeBg: StripeBg);
        }
        public MenuColor WithStripeBg( string stripeBg )
        {
            return new MenuColor(
                border1: Border1,
                border2: Border2,
                border3: Border3,
                background: Background,
                stripe: Stripe,
                stripeBg: stripeBg.ToMaybe());
        }
    }

    public sealed partial class Intermission
    {
        public Maybe<string> Name { get; } = Maybe<string>.Nothing;
        public ImmutableList<IIntermissionAction> IntermissionActions { get; } = ImmutableList<IIntermissionAction>.Empty;
        public static Intermission Default = new Intermission();
        private Intermission() { }
        public Intermission(
            Maybe<string> name,
            IEnumerable<IIntermissionAction> intermissionActions)
        {
            Name = name;
            IntermissionActions = intermissionActions.ToImmutableList();
        }
        public Intermission WithName( string name )
        {
            return new Intermission(
                name: name.ToMaybe(),
                intermissionActions: IntermissionActions);
        }
        public Intermission WithIntermissionActions( IEnumerable<IIntermissionAction> intermissionActions )
        {
            return new Intermission(
                name: Name,
                intermissionActions: intermissionActions);
        }
        public Intermission WithAdditionalIntermissionAction( IIntermissionAction intermissionAction )
        {
            return new Intermission(
                name: Name,
                intermissionActions: IntermissionActions.Add(intermissionAction));
        }
    }

    public abstract partial class IntermissionAction
    {
        public Maybe<IntermissionBackground> Background { get; } = Maybe<IntermissionBackground>.Nothing;
        public Maybe<IntermissionDraw> Draw { get; } = Maybe<IntermissionDraw>.Nothing;
        public Maybe<string> Music { get; } = Maybe<string>.Nothing;
        public Maybe<int> Time { get; } = Maybe<int>.Nothing;
        protected IntermissionAction() { }
        protected IntermissionAction(
            Maybe<IntermissionBackground> background,
            Maybe<IntermissionDraw> draw,
            Maybe<string> music,
            Maybe<int> time)
        {
            Background = background;
            Draw = draw;
            Music = music;
            Time = time;
        }
    }

    public sealed partial class IntermissionBackground
    {
        public Maybe<string> Texture { get; } = Maybe<string>.Nothing;
        public Maybe<bool> Tiled { get; } = Maybe<bool>.Nothing;
        public Maybe<string> Palette { get; } = Maybe<string>.Nothing;
        public static IntermissionBackground Default = new IntermissionBackground();
        private IntermissionBackground() { }
        public IntermissionBackground(
            Maybe<string> texture,
            Maybe<bool> tiled,
            Maybe<string> palette)
        {
            Texture = texture;
            Tiled = tiled;
            Palette = palette;
        }
        public IntermissionBackground WithTexture( string texture )
        {
            return new IntermissionBackground(
                texture: texture.ToMaybe(),
                tiled: Tiled,
                palette: Palette);
        }
        public IntermissionBackground WithTiled( bool tiled )
        {
            return new IntermissionBackground(
                texture: Texture,
                tiled: tiled.ToMaybe(),
                palette: Palette);
        }
        public IntermissionBackground WithPalette( string palette )
        {
            return new IntermissionBackground(
                texture: Texture,
                tiled: Tiled,
                palette: palette.ToMaybe());
        }
    }

    public sealed partial class IntermissionDraw
    {
        public Maybe<string> Texture { get; } = Maybe<string>.Nothing;
        public Maybe<int> X { get; } = Maybe<int>.Nothing;
        public Maybe<int> Y { get; } = Maybe<int>.Nothing;
        public static IntermissionDraw Default = new IntermissionDraw();
        private IntermissionDraw() { }
        public IntermissionDraw(
            Maybe<string> texture,
            Maybe<int> x,
            Maybe<int> y)
        {
            Texture = texture;
            X = x;
            Y = y;
        }
        public IntermissionDraw WithTexture( string texture )
        {
            return new IntermissionDraw(
                texture: texture.ToMaybe(),
                x: X,
                y: Y);
        }
        public IntermissionDraw WithX( int x )
        {
            return new IntermissionDraw(
                texture: Texture,
                x: x.ToMaybe(),
                y: Y);
        }
        public IntermissionDraw WithY( int y )
        {
            return new IntermissionDraw(
                texture: Texture,
                x: X,
                y: y.ToMaybe());
        }
    }

    public sealed partial class Fader : IntermissionAction, IIntermissionAction
    {
        public Maybe<string> FadeType { get; } = Maybe<string>.Nothing;
        public static Fader Default = new Fader();
        private Fader() { }
        public Fader(
            Maybe<IntermissionBackground> background,
            Maybe<IntermissionDraw> draw,
            Maybe<string> music,
            Maybe<int> time,
            Maybe<string> fadeType)
            : base(
                background,
                draw,
                music,
                time)
        {
            FadeType = fadeType;
        }
        public Fader WithBackground( IntermissionBackground background )
        {
            return new Fader(
                background: background.ToMaybe(),
                draw: Draw,
                music: Music,
                time: Time,
                fadeType: FadeType);
        }
        public Fader WithDraw( IntermissionDraw draw )
        {
            return new Fader(
                background: Background,
                draw: draw.ToMaybe(),
                music: Music,
                time: Time,
                fadeType: FadeType);
        }
        public Fader WithMusic( string music )
        {
            return new Fader(
                background: Background,
                draw: Draw,
                music: music.ToMaybe(),
                time: Time,
                fadeType: FadeType);
        }
        public Fader WithTime( int time )
        {
            return new Fader(
                background: Background,
                draw: Draw,
                music: Music,
                time: time.ToMaybe(),
                fadeType: FadeType);
        }
        public Fader WithFadeType( string fadeType )
        {
            return new Fader(
                background: Background,
                draw: Draw,
                music: Music,
                time: Time,
                fadeType: fadeType.ToMaybe());
        }
    }

    public sealed partial class GoToTitle : IIntermissionAction
    {
        public static GoToTitle Default = new GoToTitle();
        private GoToTitle() { }
    }

    public sealed partial class Image : IntermissionAction, IIntermissionAction
    {
        public static Image Default = new Image();
        private Image() { }
        public Image(
            Maybe<IntermissionBackground> background,
            Maybe<IntermissionDraw> draw,
            Maybe<string> music,
            Maybe<int> time)
            : base(
                background,
                draw,
                music,
                time)
        {
        }
        public Image WithBackground( IntermissionBackground background )
        {
            return new Image(
                background: background.ToMaybe(),
                draw: Draw,
                music: Music,
                time: Time);
        }
        public Image WithDraw( IntermissionDraw draw )
        {
            return new Image(
                background: Background,
                draw: draw.ToMaybe(),
                music: Music,
                time: Time);
        }
        public Image WithMusic( string music )
        {
            return new Image(
                background: Background,
                draw: Draw,
                music: music.ToMaybe(),
                time: Time);
        }
        public Image WithTime( int time )
        {
            return new Image(
                background: Background,
                draw: Draw,
                music: Music,
                time: time.ToMaybe());
        }
    }

    public sealed partial class TextScreen : IntermissionAction, IIntermissionAction
    {
        public ImmutableList<string> Texts { get; } = ImmutableList<string>.Empty;
        public Maybe<string> TextAlignment { get; } = Maybe<string>.Nothing;
        public Maybe<string> TextColor { get; } = Maybe<string>.Nothing;
        public Maybe<int> TextSpeed { get; } = Maybe<int>.Nothing;
        public Maybe<TextScreenPosition> Position { get; } = Maybe<TextScreenPosition>.Nothing;
        public static TextScreen Default = new TextScreen();
        private TextScreen() { }
        public TextScreen(
            Maybe<IntermissionBackground> background,
            Maybe<IntermissionDraw> draw,
            Maybe<string> music,
            Maybe<int> time,
            IEnumerable<string> texts,
            Maybe<string> textAlignment,
            Maybe<string> textColor,
            Maybe<int> textSpeed,
            Maybe<TextScreenPosition> position)
            : base(
                background,
                draw,
                music,
                time)
        {
            Texts = texts.ToImmutableList();
            TextAlignment = textAlignment;
            TextColor = textColor;
            TextSpeed = textSpeed;
            Position = position;
        }
        public TextScreen WithBackground( IntermissionBackground background )
        {
            return new TextScreen(
                background: background.ToMaybe(),
                draw: Draw,
                music: Music,
                time: Time,
                texts: Texts,
                textAlignment: TextAlignment,
                textColor: TextColor,
                textSpeed: TextSpeed,
                position: Position);
        }
        public TextScreen WithDraw( IntermissionDraw draw )
        {
            return new TextScreen(
                background: Background,
                draw: draw.ToMaybe(),
                music: Music,
                time: Time,
                texts: Texts,
                textAlignment: TextAlignment,
                textColor: TextColor,
                textSpeed: TextSpeed,
                position: Position);
        }
        public TextScreen WithMusic( string music )
        {
            return new TextScreen(
                background: Background,
                draw: Draw,
                music: music.ToMaybe(),
                time: Time,
                texts: Texts,
                textAlignment: TextAlignment,
                textColor: TextColor,
                textSpeed: TextSpeed,
                position: Position);
        }
        public TextScreen WithTime( int time )
        {
            return new TextScreen(
                background: Background,
                draw: Draw,
                music: Music,
                time: time.ToMaybe(),
                texts: Texts,
                textAlignment: TextAlignment,
                textColor: TextColor,
                textSpeed: TextSpeed,
                position: Position);
        }
        public TextScreen WithTexts( IEnumerable<string> texts )
        {
            return new TextScreen(
                background: Background,
                draw: Draw,
                music: Music,
                time: Time,
                texts: texts,
                textAlignment: TextAlignment,
                textColor: TextColor,
                textSpeed: TextSpeed,
                position: Position);
        }
        public TextScreen WithAdditionalText( string text )
        {
            return new TextScreen(
                background: Background,
                draw: Draw,
                music: Music,
                time: Time,
                texts: Texts.Add(text),
                textAlignment: TextAlignment,
                textColor: TextColor,
                textSpeed: TextSpeed,
                position: Position);
        }
        public TextScreen WithTextAlignment( string textAlignment )
        {
            return new TextScreen(
                background: Background,
                draw: Draw,
                music: Music,
                time: Time,
                texts: Texts,
                textAlignment: textAlignment.ToMaybe(),
                textColor: TextColor,
                textSpeed: TextSpeed,
                position: Position);
        }
        public TextScreen WithTextColor( string textColor )
        {
            return new TextScreen(
                background: Background,
                draw: Draw,
                music: Music,
                time: Time,
                texts: Texts,
                textAlignment: TextAlignment,
                textColor: textColor.ToMaybe(),
                textSpeed: TextSpeed,
                position: Position);
        }
        public TextScreen WithTextSpeed( int textSpeed )
        {
            return new TextScreen(
                background: Background,
                draw: Draw,
                music: Music,
                time: Time,
                texts: Texts,
                textAlignment: TextAlignment,
                textColor: TextColor,
                textSpeed: textSpeed.ToMaybe(),
                position: Position);
        }
        public TextScreen WithPosition( TextScreenPosition position )
        {
            return new TextScreen(
                background: Background,
                draw: Draw,
                music: Music,
                time: Time,
                texts: Texts,
                textAlignment: TextAlignment,
                textColor: TextColor,
                textSpeed: TextSpeed,
                position: position.ToMaybe());
        }
    }

    public sealed partial class TextScreenPosition
    {
        public Maybe<int> X { get; } = Maybe<int>.Nothing;
        public Maybe<int> Y { get; } = Maybe<int>.Nothing;
        public static TextScreenPosition Default = new TextScreenPosition();
        private TextScreenPosition() { }
        public TextScreenPosition(
            Maybe<int> x,
            Maybe<int> y)
        {
            X = x;
            Y = y;
        }
        public TextScreenPosition WithX( int x )
        {
            return new TextScreenPosition(
                x: x.ToMaybe(),
                y: Y);
        }
        public TextScreenPosition WithY( int y )
        {
            return new TextScreenPosition(
                x: X,
                y: y.ToMaybe());
        }
    }

    public sealed partial class VictoryStats : IntermissionAction, IIntermissionAction
    {
        public static VictoryStats Default = new VictoryStats();
        private VictoryStats() { }
        public VictoryStats(
            Maybe<IntermissionBackground> background,
            Maybe<IntermissionDraw> draw,
            Maybe<string> music,
            Maybe<int> time)
            : base(
                background,
                draw,
                music,
                time)
        {
        }
        public VictoryStats WithBackground( IntermissionBackground background )
        {
            return new VictoryStats(
                background: background.ToMaybe(),
                draw: Draw,
                music: Music,
                time: Time);
        }
        public VictoryStats WithDraw( IntermissionDraw draw )
        {
            return new VictoryStats(
                background: Background,
                draw: draw.ToMaybe(),
                music: Music,
                time: Time);
        }
        public VictoryStats WithMusic( string music )
        {
            return new VictoryStats(
                background: Background,
                draw: Draw,
                music: music.ToMaybe(),
                time: Time);
        }
        public VictoryStats WithTime( int time )
        {
            return new VictoryStats(
                background: Background,
                draw: Draw,
                music: Music,
                time: time.ToMaybe());
        }
    }

    public sealed partial class DefaultMap : BaseMap
    {
        public static DefaultMap Default = new DefaultMap();
        private DefaultMap() { }
        public DefaultMap(
            Maybe<string> borderTexture,
            Maybe<int> cluster,
            Maybe<string> completionString,
            Maybe<bool> deathCam,
            Maybe<string> defaultCeiling,
            Maybe<string> defaultFloor,
            IEnumerable<string> ensureInventories,
            Maybe<int> exitFade,
            Maybe<int> floorNumber,
            Maybe<string> highScoresGraphic,
            Maybe<int> levelBonus,
            Maybe<int> levelNum,
            Maybe<string> music,
            Maybe<bool> spawnWithWeaponRaised,
            Maybe<bool> secretDeathSounds,
            Maybe<NextMapInfo> next,
            Maybe<NextMapInfo> secretNext,
            Maybe<NextMapInfo> victoryNext,
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
                ensureInventories,
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
                specialActions,
                nointermission,
                par,
                translator)
        {
        }
        public DefaultMap WithBorderTexture( string borderTexture )
        {
            return new DefaultMap(
                borderTexture: borderTexture.ToMaybe(),
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithCluster( int cluster )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: cluster.ToMaybe(),
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithCompletionString( string completionString )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: completionString.ToMaybe(),
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithDeathCam( bool deathCam )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: deathCam.ToMaybe(),
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithDefaultCeiling( string defaultCeiling )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: defaultCeiling.ToMaybe(),
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithDefaultFloor( string defaultFloor )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: defaultFloor.ToMaybe(),
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithEnsureInventories( IEnumerable<string> ensureInventories )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: ensureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithAdditionalEnsureInventory( string ensureInventory )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories.Add(ensureInventory),
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithExitFade( int exitFade )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: exitFade.ToMaybe(),
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithFloorNumber( int floorNumber )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: floorNumber.ToMaybe(),
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithHighScoresGraphic( string highScoresGraphic )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: highScoresGraphic.ToMaybe(),
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithLevelBonus( int levelBonus )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: levelBonus.ToMaybe(),
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithLevelNum( int levelNum )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: levelNum.ToMaybe(),
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithMusic( string music )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: music.ToMaybe(),
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithSpawnWithWeaponRaised( bool spawnWithWeaponRaised )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: spawnWithWeaponRaised.ToMaybe(),
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithSecretDeathSounds( bool secretDeathSounds )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: secretDeathSounds.ToMaybe(),
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithNext( NextMapInfo next )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: next.ToMaybe(),
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithSecretNext( NextMapInfo secretNext )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: secretNext.ToMaybe(),
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithVictoryNext( NextMapInfo victoryNext )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: victoryNext.ToMaybe(),
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithSpecialActions( IEnumerable<SpecialAction> specialActions )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: specialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithAdditionalSpecialAction( SpecialAction specialAction )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions.Add(specialAction),
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithNointermission( bool nointermission )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: nointermission.ToMaybe(),
                par: Par,
                translator: Translator);
        }
        public DefaultMap WithPar( int par )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: par.ToMaybe(),
                translator: Translator);
        }
        public DefaultMap WithTranslator( string translator )
        {
            return new DefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: translator.ToMaybe());
        }
        public DefaultMap WithAddDefaultMap( AddDefaultMap addDefaultMap )
        {
            return new DefaultMap(
                borderTexture: addDefaultMap.BorderTexture.Or(BorderTexture),
                cluster: addDefaultMap.Cluster.Or(Cluster),
                completionString: addDefaultMap.CompletionString.Or(CompletionString),
                deathCam: addDefaultMap.DeathCam.Or(DeathCam),
                defaultCeiling: addDefaultMap.DefaultCeiling.Or(DefaultCeiling),
                defaultFloor: addDefaultMap.DefaultFloor.Or(DefaultFloor),
                ensureInventories: EnsureInventories.AddRange(addDefaultMap.EnsureInventories),
                exitFade: addDefaultMap.ExitFade.Or(ExitFade),
                floorNumber: addDefaultMap.FloorNumber.Or(FloorNumber),
                highScoresGraphic: addDefaultMap.HighScoresGraphic.Or(HighScoresGraphic),
                levelBonus: addDefaultMap.LevelBonus.Or(LevelBonus),
                levelNum: addDefaultMap.LevelNum.Or(LevelNum),
                music: addDefaultMap.Music.Or(Music),
                spawnWithWeaponRaised: addDefaultMap.SpawnWithWeaponRaised.Or(SpawnWithWeaponRaised),
                secretDeathSounds: addDefaultMap.SecretDeathSounds.Or(SecretDeathSounds),
                next: addDefaultMap.Next.Or(Next),
                secretNext: addDefaultMap.SecretNext.Or(SecretNext),
                victoryNext: addDefaultMap.VictoryNext.Or(VictoryNext),
                specialActions: SpecialActions.AddRange(addDefaultMap.SpecialActions),
                nointermission: addDefaultMap.Nointermission.Or(Nointermission),
                par: addDefaultMap.Par.Or(Par),
                translator: addDefaultMap.Translator.Or(Translator));
        }
    }

    public sealed partial class AddDefaultMap : BaseMap
    {
        public static AddDefaultMap Default = new AddDefaultMap();
        private AddDefaultMap() { }
        public AddDefaultMap(
            Maybe<string> borderTexture,
            Maybe<int> cluster,
            Maybe<string> completionString,
            Maybe<bool> deathCam,
            Maybe<string> defaultCeiling,
            Maybe<string> defaultFloor,
            IEnumerable<string> ensureInventories,
            Maybe<int> exitFade,
            Maybe<int> floorNumber,
            Maybe<string> highScoresGraphic,
            Maybe<int> levelBonus,
            Maybe<int> levelNum,
            Maybe<string> music,
            Maybe<bool> spawnWithWeaponRaised,
            Maybe<bool> secretDeathSounds,
            Maybe<NextMapInfo> next,
            Maybe<NextMapInfo> secretNext,
            Maybe<NextMapInfo> victoryNext,
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
                ensureInventories,
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
                specialActions,
                nointermission,
                par,
                translator)
        {
        }
        public AddDefaultMap WithBorderTexture( string borderTexture )
        {
            return new AddDefaultMap(
                borderTexture: borderTexture.ToMaybe(),
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithCluster( int cluster )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: cluster.ToMaybe(),
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithCompletionString( string completionString )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: completionString.ToMaybe(),
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithDeathCam( bool deathCam )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: deathCam.ToMaybe(),
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithDefaultCeiling( string defaultCeiling )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: defaultCeiling.ToMaybe(),
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithDefaultFloor( string defaultFloor )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: defaultFloor.ToMaybe(),
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithEnsureInventories( IEnumerable<string> ensureInventories )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: ensureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithAdditionalEnsureInventory( string ensureInventory )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories.Add(ensureInventory),
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithExitFade( int exitFade )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: exitFade.ToMaybe(),
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithFloorNumber( int floorNumber )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: floorNumber.ToMaybe(),
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithHighScoresGraphic( string highScoresGraphic )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: highScoresGraphic.ToMaybe(),
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithLevelBonus( int levelBonus )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: levelBonus.ToMaybe(),
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithLevelNum( int levelNum )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: levelNum.ToMaybe(),
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithMusic( string music )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: music.ToMaybe(),
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithSpawnWithWeaponRaised( bool spawnWithWeaponRaised )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: spawnWithWeaponRaised.ToMaybe(),
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithSecretDeathSounds( bool secretDeathSounds )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: secretDeathSounds.ToMaybe(),
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithNext( NextMapInfo next )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: next.ToMaybe(),
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithSecretNext( NextMapInfo secretNext )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: secretNext.ToMaybe(),
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithVictoryNext( NextMapInfo victoryNext )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: victoryNext.ToMaybe(),
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithSpecialActions( IEnumerable<SpecialAction> specialActions )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: specialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithAdditionalSpecialAction( SpecialAction specialAction )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions.Add(specialAction),
                nointermission: Nointermission,
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithNointermission( bool nointermission )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: nointermission.ToMaybe(),
                par: Par,
                translator: Translator);
        }
        public AddDefaultMap WithPar( int par )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: par.ToMaybe(),
                translator: Translator);
        }
        public AddDefaultMap WithTranslator( string translator )
        {
            return new AddDefaultMap(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: translator.ToMaybe());
        }
    }

    public abstract partial class BaseMap
    {
        public Maybe<string> BorderTexture { get; } = Maybe<string>.Nothing;
        public Maybe<int> Cluster { get; } = Maybe<int>.Nothing;
        public Maybe<string> CompletionString { get; } = Maybe<string>.Nothing;
        public Maybe<bool> DeathCam { get; } = ((bool)false).ToMaybe();
        public Maybe<string> DefaultCeiling { get; } = Maybe<string>.Nothing;
        public Maybe<string> DefaultFloor { get; } = Maybe<string>.Nothing;
        public ImmutableList<string> EnsureInventories { get; } = ImmutableList<string>.Empty;
        public Maybe<int> ExitFade { get; } = Maybe<int>.Nothing;
        public Maybe<int> FloorNumber { get; } = Maybe<int>.Nothing;
        public Maybe<string> HighScoresGraphic { get; } = Maybe<string>.Nothing;
        public Maybe<int> LevelBonus { get; } = Maybe<int>.Nothing;
        public Maybe<int> LevelNum { get; } = Maybe<int>.Nothing;
        public Maybe<string> Music { get; } = Maybe<string>.Nothing;
        public Maybe<bool> SpawnWithWeaponRaised { get; } = ((bool)false).ToMaybe();
        public Maybe<bool> SecretDeathSounds { get; } = ((bool)false).ToMaybe();
        public Maybe<NextMapInfo> Next { get; } = Maybe<NextMapInfo>.Nothing;
        public Maybe<NextMapInfo> SecretNext { get; } = Maybe<NextMapInfo>.Nothing;
        public Maybe<NextMapInfo> VictoryNext { get; } = Maybe<NextMapInfo>.Nothing;
        public ImmutableList<SpecialAction> SpecialActions { get; } = ImmutableList<SpecialAction>.Empty;
        public Maybe<bool> Nointermission { get; } = ((bool)false).ToMaybe();
        public Maybe<int> Par { get; } = Maybe<int>.Nothing;
        public Maybe<string> Translator { get; } = Maybe<string>.Nothing;
        protected BaseMap() { }
        protected BaseMap(
            Maybe<string> borderTexture,
            Maybe<int> cluster,
            Maybe<string> completionString,
            Maybe<bool> deathCam,
            Maybe<string> defaultCeiling,
            Maybe<string> defaultFloor,
            IEnumerable<string> ensureInventories,
            Maybe<int> exitFade,
            Maybe<int> floorNumber,
            Maybe<string> highScoresGraphic,
            Maybe<int> levelBonus,
            Maybe<int> levelNum,
            Maybe<string> music,
            Maybe<bool> spawnWithWeaponRaised,
            Maybe<bool> secretDeathSounds,
            Maybe<NextMapInfo> next,
            Maybe<NextMapInfo> secretNext,
            Maybe<NextMapInfo> victoryNext,
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
            EnsureInventories = ensureInventories.ToImmutableList();
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
            SpecialActions = specialActions.ToImmutableList();
            Nointermission = nointermission;
            Par = par;
            Translator = translator;
        }
    }

    public sealed partial class Map : BaseMap
    {
        public Maybe<string> MapLump { get; } = Maybe<string>.Nothing;
        public Maybe<string> MapName { get; } = Maybe<string>.Nothing;
        public Maybe<string> MapNameLookup { get; } = Maybe<string>.Nothing;
        public static Map Default = new Map();
        private Map() { }
        public Map(
            Maybe<string> borderTexture,
            Maybe<int> cluster,
            Maybe<string> completionString,
            Maybe<bool> deathCam,
            Maybe<string> defaultCeiling,
            Maybe<string> defaultFloor,
            IEnumerable<string> ensureInventories,
            Maybe<int> exitFade,
            Maybe<int> floorNumber,
            Maybe<string> highScoresGraphic,
            Maybe<int> levelBonus,
            Maybe<int> levelNum,
            Maybe<string> music,
            Maybe<bool> spawnWithWeaponRaised,
            Maybe<bool> secretDeathSounds,
            Maybe<NextMapInfo> next,
            Maybe<NextMapInfo> secretNext,
            Maybe<NextMapInfo> victoryNext,
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
                ensureInventories,
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
                specialActions,
                nointermission,
                par,
                translator)
        {
            MapLump = mapLump;
            MapName = mapName;
            MapNameLookup = mapNameLookup;
        }
        public Map WithBorderTexture( string borderTexture )
        {
            return new Map(
                borderTexture: borderTexture.ToMaybe(),
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithCluster( int cluster )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: cluster.ToMaybe(),
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithCompletionString( string completionString )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: completionString.ToMaybe(),
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithDeathCam( bool deathCam )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: deathCam.ToMaybe(),
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithDefaultCeiling( string defaultCeiling )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: defaultCeiling.ToMaybe(),
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithDefaultFloor( string defaultFloor )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: defaultFloor.ToMaybe(),
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithEnsureInventories( IEnumerable<string> ensureInventories )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: ensureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithAdditionalEnsureInventory( string ensureInventory )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories.Add(ensureInventory),
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithExitFade( int exitFade )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: exitFade.ToMaybe(),
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithFloorNumber( int floorNumber )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: floorNumber.ToMaybe(),
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithHighScoresGraphic( string highScoresGraphic )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: highScoresGraphic.ToMaybe(),
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithLevelBonus( int levelBonus )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: levelBonus.ToMaybe(),
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithLevelNum( int levelNum )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: levelNum.ToMaybe(),
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithMusic( string music )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: music.ToMaybe(),
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithSpawnWithWeaponRaised( bool spawnWithWeaponRaised )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: spawnWithWeaponRaised.ToMaybe(),
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithSecretDeathSounds( bool secretDeathSounds )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: secretDeathSounds.ToMaybe(),
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithNext( NextMapInfo next )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: next.ToMaybe(),
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithSecretNext( NextMapInfo secretNext )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: secretNext.ToMaybe(),
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithVictoryNext( NextMapInfo victoryNext )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: victoryNext.ToMaybe(),
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithSpecialActions( IEnumerable<SpecialAction> specialActions )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: specialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithAdditionalSpecialAction( SpecialAction specialAction )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions.Add(specialAction),
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithNointermission( bool nointermission )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: nointermission.ToMaybe(),
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithPar( int par )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: par.ToMaybe(),
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithTranslator( string translator )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: translator.ToMaybe(),
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithMapLump( string mapLump )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: mapLump.ToMaybe(),
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
        public Map WithMapName( string mapName )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: mapName.ToMaybe(),
                mapNameLookup: MapNameLookup);
        }
        public Map WithMapNameLookup( string mapNameLookup )
        {
            return new Map(
                borderTexture: BorderTexture,
                cluster: Cluster,
                completionString: CompletionString,
                deathCam: DeathCam,
                defaultCeiling: DefaultCeiling,
                defaultFloor: DefaultFloor,
                ensureInventories: EnsureInventories,
                exitFade: ExitFade,
                floorNumber: FloorNumber,
                highScoresGraphic: HighScoresGraphic,
                levelBonus: LevelBonus,
                levelNum: LevelNum,
                music: Music,
                spawnWithWeaponRaised: SpawnWithWeaponRaised,
                secretDeathSounds: SecretDeathSounds,
                next: Next,
                secretNext: SecretNext,
                victoryNext: VictoryNext,
                specialActions: SpecialActions,
                nointermission: Nointermission,
                par: Par,
                translator: Translator,
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: mapNameLookup.ToMaybe());
        }
        public Map WithDefaultMap( DefaultMap defaultMap )
        {
            return new Map(
                borderTexture: defaultMap.BorderTexture.Or(BorderTexture),
                cluster: defaultMap.Cluster.Or(Cluster),
                completionString: defaultMap.CompletionString.Or(CompletionString),
                deathCam: defaultMap.DeathCam.Or(DeathCam),
                defaultCeiling: defaultMap.DefaultCeiling.Or(DefaultCeiling),
                defaultFloor: defaultMap.DefaultFloor.Or(DefaultFloor),
                ensureInventories: EnsureInventories.AddRange(defaultMap.EnsureInventories),
                exitFade: defaultMap.ExitFade.Or(ExitFade),
                floorNumber: defaultMap.FloorNumber.Or(FloorNumber),
                highScoresGraphic: defaultMap.HighScoresGraphic.Or(HighScoresGraphic),
                levelBonus: defaultMap.LevelBonus.Or(LevelBonus),
                levelNum: defaultMap.LevelNum.Or(LevelNum),
                music: defaultMap.Music.Or(Music),
                spawnWithWeaponRaised: defaultMap.SpawnWithWeaponRaised.Or(SpawnWithWeaponRaised),
                secretDeathSounds: defaultMap.SecretDeathSounds.Or(SecretDeathSounds),
                next: defaultMap.Next.Or(Next),
                secretNext: defaultMap.SecretNext.Or(SecretNext),
                victoryNext: defaultMap.VictoryNext.Or(VictoryNext),
                specialActions: SpecialActions.AddRange(defaultMap.SpecialActions),
                nointermission: defaultMap.Nointermission.Or(Nointermission),
                par: defaultMap.Par.Or(Par),
                translator: defaultMap.Translator.Or(Translator),
                mapLump: MapLump,
                mapName: MapName,
                mapNameLookup: MapNameLookup);
        }
    }

    public sealed partial class NextMapInfo
    {
        public Maybe<string> Name { get; } = Maybe<string>.Nothing;
        public Maybe<bool> EndSequence { get; } = ((bool)false).ToMaybe();
        public static NextMapInfo Default = new NextMapInfo();
        private NextMapInfo() { }
        public NextMapInfo(
            Maybe<string> name,
            Maybe<bool> endSequence)
        {
            Name = name;
            EndSequence = endSequence;
        }
        public NextMapInfo WithName( string name )
        {
            return new NextMapInfo(
                name: name.ToMaybe(),
                endSequence: EndSequence);
        }
        public NextMapInfo WithEndSequence( bool endSequence )
        {
            return new NextMapInfo(
                name: Name,
                endSequence: endSequence.ToMaybe());
        }
    }

    public sealed partial class SpecialAction
    {
        public Maybe<string> ActorClass { get; } = Maybe<string>.Nothing;
        public Maybe<string> Special { get; } = Maybe<string>.Nothing;
        public Maybe<int> Arg0 { get; } = Maybe<int>.Nothing;
        public Maybe<int> Arg1 { get; } = Maybe<int>.Nothing;
        public Maybe<int> Arg2 { get; } = Maybe<int>.Nothing;
        public Maybe<int> Arg3 { get; } = Maybe<int>.Nothing;
        public Maybe<int> Arg4 { get; } = Maybe<int>.Nothing;
        public static SpecialAction Default = new SpecialAction();
        private SpecialAction() { }
        public SpecialAction(
            Maybe<string> actorClass,
            Maybe<string> special,
            Maybe<int> arg0,
            Maybe<int> arg1,
            Maybe<int> arg2,
            Maybe<int> arg3,
            Maybe<int> arg4)
        {
            ActorClass = actorClass;
            Special = special;
            Arg0 = arg0;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
        }
        public SpecialAction WithActorClass( string actorClass )
        {
            return new SpecialAction(
                actorClass: actorClass.ToMaybe(),
                special: Special,
                arg0: Arg0,
                arg1: Arg1,
                arg2: Arg2,
                arg3: Arg3,
                arg4: Arg4);
        }
        public SpecialAction WithSpecial( string special )
        {
            return new SpecialAction(
                actorClass: ActorClass,
                special: special.ToMaybe(),
                arg0: Arg0,
                arg1: Arg1,
                arg2: Arg2,
                arg3: Arg3,
                arg4: Arg4);
        }
        public SpecialAction WithArg0( int arg0 )
        {
            return new SpecialAction(
                actorClass: ActorClass,
                special: Special,
                arg0: arg0.ToMaybe(),
                arg1: Arg1,
                arg2: Arg2,
                arg3: Arg3,
                arg4: Arg4);
        }
        public SpecialAction WithArg1( int arg1 )
        {
            return new SpecialAction(
                actorClass: ActorClass,
                special: Special,
                arg0: Arg0,
                arg1: arg1.ToMaybe(),
                arg2: Arg2,
                arg3: Arg3,
                arg4: Arg4);
        }
        public SpecialAction WithArg2( int arg2 )
        {
            return new SpecialAction(
                actorClass: ActorClass,
                special: Special,
                arg0: Arg0,
                arg1: Arg1,
                arg2: arg2.ToMaybe(),
                arg3: Arg3,
                arg4: Arg4);
        }
        public SpecialAction WithArg3( int arg3 )
        {
            return new SpecialAction(
                actorClass: ActorClass,
                special: Special,
                arg0: Arg0,
                arg1: Arg1,
                arg2: Arg2,
                arg3: arg3.ToMaybe(),
                arg4: Arg4);
        }
        public SpecialAction WithArg4( int arg4 )
        {
            return new SpecialAction(
                actorClass: ActorClass,
                special: Special,
                arg0: Arg0,
                arg1: Arg1,
                arg2: Arg2,
                arg3: Arg3,
                arg4: arg4.ToMaybe());
        }
    }

    public sealed partial class MapInfo
    {
        public ImmutableList<Cluster> Clusters { get; } = ImmutableList<Cluster>.Empty;
        public ImmutableList<Episode> Episodes { get; } = ImmutableList<Episode>.Empty;
        public Maybe<GameInfo> GameInfo { get; } = Maybe<GameInfo>.Nothing;
        public ImmutableList<Intermission> Intermissions { get; } = ImmutableList<Intermission>.Empty;
        public ImmutableList<Map> Maps { get; } = ImmutableList<Map>.Empty;
        public static MapInfo Default = new MapInfo();
        private MapInfo() { }
        public MapInfo(
            IEnumerable<Cluster> clusters,
            IEnumerable<Episode> episodes,
            Maybe<GameInfo> gameInfo,
            IEnumerable<Intermission> intermissions,
            IEnumerable<Map> maps)
        {
            Clusters = clusters.ToImmutableList();
            Episodes = episodes.ToImmutableList();
            GameInfo = gameInfo;
            Intermissions = intermissions.ToImmutableList();
            Maps = maps.ToImmutableList();
        }
        public MapInfo WithClusters( IEnumerable<Cluster> clusters )
        {
            return new MapInfo(
                clusters: clusters,
                episodes: Episodes,
                gameInfo: GameInfo,
                intermissions: Intermissions,
                maps: Maps);
        }
        public MapInfo WithAdditionalCluster( Cluster cluster )
        {
            return new MapInfo(
                clusters: Clusters.Add(cluster),
                episodes: Episodes,
                gameInfo: GameInfo,
                intermissions: Intermissions,
                maps: Maps);
        }
        public MapInfo WithEpisodes( IEnumerable<Episode> episodes )
        {
            return new MapInfo(
                clusters: Clusters,
                episodes: episodes,
                gameInfo: GameInfo,
                intermissions: Intermissions,
                maps: Maps);
        }
        public MapInfo WithAdditionalEpisode( Episode episode )
        {
            return new MapInfo(
                clusters: Clusters,
                episodes: Episodes.Add(episode),
                gameInfo: GameInfo,
                intermissions: Intermissions,
                maps: Maps);
        }
        public MapInfo WithGameInfo( GameInfo gameInfo )
        {
            return new MapInfo(
                clusters: Clusters,
                episodes: Episodes,
                gameInfo: gameInfo.ToMaybe(),
                intermissions: Intermissions,
                maps: Maps);
        }
        public MapInfo WithIntermissions( IEnumerable<Intermission> intermissions )
        {
            return new MapInfo(
                clusters: Clusters,
                episodes: Episodes,
                gameInfo: GameInfo,
                intermissions: intermissions,
                maps: Maps);
        }
        public MapInfo WithAdditionalIntermission( Intermission intermission )
        {
            return new MapInfo(
                clusters: Clusters,
                episodes: Episodes,
                gameInfo: GameInfo,
                intermissions: Intermissions.Add(intermission),
                maps: Maps);
        }
        public MapInfo WithMaps( IEnumerable<Map> maps )
        {
            return new MapInfo(
                clusters: Clusters,
                episodes: Episodes,
                gameInfo: GameInfo,
                intermissions: Intermissions,
                maps: maps);
        }
        public MapInfo WithAdditionalMap( Map map )
        {
            return new MapInfo(
                clusters: Clusters,
                episodes: Episodes,
                gameInfo: GameInfo,
                intermissions: Intermissions,
                maps: Maps.Add(map));
        }
    }

}
