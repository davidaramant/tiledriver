// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.Wolf3D
{
    public sealed class TileTheme
    {
        private static int _currentId = 0;
        private static readonly List<TileTheme> AllTileThemes = new List<TileTheme>();

        public int Id { get; }
        public Tile Definition { get; }

        private TileTheme(Tile definition)
        {
            Id = _currentId++;
            Definition = definition;

            AllTileThemes.Add(this);
        }

        public static TileTheme GrayStone1 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTONEA1",
                TextureSouth = "GSTONEA1",
                TextureEast = "GSTONEA2",
                TextureWest = "GSTONEA2",
            });

        public static TileTheme GrayStone2 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTONEB1",
                TextureSouth = "GSTONEB1",
                TextureEast = "GSTONEB2",
                TextureWest = "GSTONEB2",
            });

        public static TileTheme DoorFacingNorthSouth = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "DOOR1_1",
                TextureSouth = "DOOR1_1",
                TextureEast = "SLOT1_1",
                TextureWest = "SLOT1_1",
                OffsetHorizontal = true,
            });

        public static TileTheme DoorFacingEastWest = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "SLOT1_2",
                TextureSouth = "SLOT1_2",
                TextureEast = "DOOR1_2",
                TextureWest = "DOOR1_2",
                OffsetVertical = true,
            });

        public static TileTheme LockedDoorFacingNorthSouth = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "DOOR3_1",
                TextureSouth = "DOOR3_1",
                TextureEast = "SLOT1_1",
                TextureWest = "SLOT1_1",
                OffsetHorizontal = true,
            });

        public static TileTheme LockedDoorFacingEastWest = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "SLOT3_2",
                TextureSouth = "SLOT3_2",
                TextureEast = "DOOR1_2",
                TextureWest = "DOOR1_2",
                OffsetVertical = true,
            });

        public static TileTheme GrayStoneFlag = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTFLAG1",
                TextureSouth = "GSTFLAG1",
                TextureEast = "GSTFLAG2",
                TextureWest = "GSTFLAG2",
            });

        public static TileTheme GrayStoneHitler = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTHTLR1",
                TextureSouth = "GSTHTLR1",
                TextureEast = "GSTHTLR2",
                TextureWest = "GSTHTLR2",
            });

        public static TileTheme BlueStoneEmptyCell = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BSTCELA1",
                TextureSouth = "BSTCELA1",
                TextureEast = "BSTCELA2",
                TextureWest = "BSTCELA2",
            });

        public static TileTheme GrayStoneEagle = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTEAGL1",
                TextureSouth = "GSTEAGL1",
                TextureEast = "GSTEAGL2",
                TextureWest = "GSTEAGL2",
            });

        public static TileTheme BlueStoneCellSkele = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BSTCELB1",
                TextureSouth = "BSTCELB1",
                TextureEast = "BSTCELB2",
                TextureWest = "BSTCELB2",
            });

        public static TileTheme BlueStone1 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BSTONEA1",
                TextureSouth = "BSTONEA1",
                TextureEast = "BSTONEA2",
                TextureWest = "BSTONEA2",
            });

        public static TileTheme BlueStone2 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BSTONEB1",
                TextureSouth = "BSTONEB1",
                TextureEast = "BSTONEB2",
                TextureWest = "BSTONEB2",
            });

        public static TileTheme WoodEagle = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "WODEAGL1",
                TextureSouth = "WODEAGL1",
                TextureEast = "WODEAGL2",
                TextureWest = "WODEAGL2",
            });

        public static TileTheme WoodHitler = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "WODHTLR1",
                TextureSouth = "WODHTLR1",
                TextureEast = "WODHTLR2",
                TextureWest = "WODHTLR2",
            });

        public static TileTheme Wood = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "WOOD1",
                TextureSouth = "WOOD1",
                TextureEast = "WOOD2",
                TextureWest = "WOOD2",
            });

        public static TileTheme FakeDoor = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "FAKEDOR1",
                TextureSouth = "FAKEDOR1",
                TextureEast = "FAKEDOR2",
                TextureWest = "FAKEDOR2",
            });

        public static TileTheme SteelPanelSign = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "METLSGN1",
                TextureSouth = "METLSGN1",
                TextureEast = "METLSGN2",
                TextureWest = "METLSGN2",
            });

        public static TileTheme Metal = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "METAL1",
                TextureSouth = "METAL1",
                TextureEast = "METAL2",
                TextureWest = "METAL2",
            });

        public static TileTheme DaytimeSky = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "SKY1",
                TextureSouth = "SKY1",
                TextureEast = "SKY1",
                TextureWest = "SKY1",
            });

        public static TileTheme NighttimeSky = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "SKY2",
                TextureSouth = "SKY2",
                TextureEast = "SKY2",
                TextureWest = "SKY2",
            });

        public static TileTheme RedBrick = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BRICK1",
                TextureSouth = "BRICK1",
                TextureEast = "BRICK2",
                TextureWest = "BRICK2",
            });

        public static TileTheme RedBrickWreath = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BRIKWRT1",
                TextureSouth = "BRIKWRT1",
                TextureEast = "BRIKWRT2",
                TextureWest = "BRIKWRT2",
            });

        public static TileTheme Purple = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "PURPLE1",
                TextureSouth = "PURPLE1",
                TextureEast = "PURPLE2",
                TextureWest = "PURPLE2",
            });

        public static TileTheme RedBrickEagle = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BRIKEGL1",
                TextureSouth = "BRIKEGL1",
                TextureEast = "BRIKEGL2",
                TextureWest = "BRIKEGL2",
            });

        public static TileTheme WoodCross = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "WODCROS1",
                TextureSouth = "WODCROS1",
                TextureEast = "WODCROS2",
                TextureWest = "WODCROS2",
            });

        public static TileTheme GrayStoneSlime1 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTSLME1",
                TextureSouth = "GSTSLME1",
                TextureEast = "GSTSLME2",
                TextureWest = "GSTSLME2",
            });

        public static TileTheme PurpleBlood = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "PURPBLD1",
                TextureSouth = "PURPBLD1",
                TextureEast = "PURPBLD2",
                TextureWest = "PURPBLD2",
            });

        public static TileTheme GrayStoneSlime2 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTLSLM1",
                TextureSouth = "GSTLSLM1",
                TextureEast = "GSTLSLM2",
                TextureWest = "GSTLSLM2",
            });
        public static TileTheme GrayStone3 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTONEC1",
                TextureSouth = "GSTONEC1",
                TextureEast = "GSTONEC2",
                TextureWest = "GSTONEC2",
            });

        public static TileTheme GrayStoneSign = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GSTSIGN1",
                TextureSouth = "GSTSIGN1",
                TextureEast = "GSTSIGN2",
                TextureWest = "GSTSIGN2",
            });

        public static TileTheme ChippedRock = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "CHIP1",
                TextureSouth = "CHIP1",
                TextureEast = "CHIP2",
                TextureWest = "CHIP2",
            });

        public static TileTheme ChippedRockBlood1 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "CHPBLDA1",
                TextureSouth = "CHPBLDA1",
                TextureEast = "CHPBLDA2",
                TextureWest = "CHPBLDA2",
            });

        public static TileTheme ChippedRockBlood2 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "CHPBLDB1",
                TextureSouth = "CHPBLDB1",
                TextureEast = "CHPBLDB2",
                TextureWest = "CHPBLDB2",
            });

        public static TileTheme ChippedRockBlood3 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "CHPBLDC1",
                TextureSouth = "CHPBLDC1",
                TextureEast = "CHPBLDC2",
                TextureWest = "CHPBLDC2",
            });

        public static TileTheme StainedGlass = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GLASS1",
                TextureSouth = "GLASS1",
                TextureEast = "GLASS2",
                TextureWest = "GLASS2",
            });

        public static TileTheme BlueBrickSkull = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BLUSKUL1",
                TextureSouth = "BLUSKUL1",
                TextureEast = "BLUSKUL2",
                TextureWest = "BLUSKUL2",
            });

        public static TileTheme GrayBrick = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GRYBRIK1",
                TextureSouth = "GRYBRIK1",
                TextureEast = "GRYBRIK2",
                TextureWest = "GRYBRIK2",
            });

        public static TileTheme BlueBrickSwastika = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BLUSWAS1",
                TextureSouth = "BLUSWAS1",
                TextureEast = "BLUSWAS2",
                TextureWest = "BLUSWAS2",
            });

        public static TileTheme GrayStoneVent = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GRYVENT1",
                TextureSouth = "GRYVENT1",
                TextureEast = "GRYVENT2",
                TextureWest = "GRYVENT2",
            });

        public static TileTheme OddBricks = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BRIKODD1",
                TextureSouth = "BRIKODD1",
                TextureEast = "BRIKODD2",
                TextureWest = "BRIKODD2",
            });

        public static TileTheme GrayBrickCrack = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GRYCRAK1",
                TextureSouth = "GRYCRAK1",
                TextureEast = "GRYCRAK2",
                TextureWest = "GRYCRAK2",
            });

        public static TileTheme BlueBrickWall = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BLUWALL1",
                TextureSouth = "BLUWALL1",
                TextureEast = "BLUWALL2",
                TextureWest = "BLUWALL2",
            });

        public static TileTheme BlueStoneSign = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BSTSIGN1",
                TextureSouth = "BSTSIGN1",
                TextureEast = "BSTSIGN2",
                TextureWest = "BSTSIGN2",
            });

        public static TileTheme BrownMarble1 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "MARB1_1",
                TextureSouth = "MARB1_1",
                TextureEast = "MARB1_2",
                TextureWest = "MARB1_2",
            });

        public static TileTheme GrayBrickMap = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GRYMAPS1",
                TextureSouth = "GRYMAPS1",
                TextureEast = "GRYMAPS2",
                TextureWest = "GRYMAPS2",
            });

        public static TileTheme BrownStone1 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BRNSTNA1",
                TextureSouth = "BRNSTNA1",
                TextureEast = "BRNSTNA2",
                TextureWest = "BRNSTNA2",
            });

        public static TileTheme BrownStone2 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "BRNSTNB1",
                TextureSouth = "BRNSTNB1",
                TextureEast = "BRNSTNB2",
                TextureWest = "BRNSTNB2",
            });

        public static TileTheme BrownMarble2 = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "MARB2_1",
                TextureSouth = "MARB2_1",
                TextureEast = "MARB2_2",
                TextureWest = "MARB2_2",
            });

        public static TileTheme BrownMarbleFlag = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "MARB3_1",
                TextureSouth = "MARB3_1",
                TextureEast = "MARB3_2",
                TextureWest = "MARB3_2",
            });

        public static TileTheme Plaster = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "PLASTER1",
                TextureSouth = "PLASTER1",
                TextureEast = "PLASTER2",
                TextureWest = "PLASTER2",
            });

        public static TileTheme GrayBrickHitler = new TileTheme(
            definition: new Tile
            {
                TextureNorth = "GRYHTLR1",
                TextureSouth = "GRYHTLR1",
                TextureEast = "GRYHTLR2",
                TextureWest = "GRYHTLR2",
            });

        public static IEnumerable<TileTheme> GetAll()
        {
            return AllTileThemes;
        }
    }
}
