// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.Wolf3D
{
    public static class DefaultTile
    {
        public static readonly Tile GrayStone1 = new(
            TextureNorth: "GSTONEA1",
            TextureSouth: "GSTONEA1",
            TextureEast: "GSTONEA2",
            TextureWest: "GSTONEA2"
        );
        public static readonly Tile GrayStone2 = new(
            TextureNorth: "GSTONEB1",
            TextureSouth: "GSTONEB1",
            TextureEast: "GSTONEB2",
            TextureWest: "GSTONEB2"
        );
        public static readonly Tile GrayStoneFlag = new(
            TextureNorth: "GSTFLAG1",
            TextureSouth: "GSTFLAG1",
            TextureEast: "GSTFLAG2",
            TextureWest: "GSTFLAG2"
        );
        public static readonly Tile GrayStoneHitler = new(
            TextureNorth: "GSTHTLR1",
            TextureSouth: "GSTHTLR1",
            TextureEast: "GSTHTLR2",
            TextureWest: "GSTHTLR2"
        );
        public static readonly Tile BlueStoneEmptyCell = new(
            TextureNorth: "BSTCELA1",
            TextureSouth: "BSTCELA1",
            TextureEast: "BSTCELA2",
            TextureWest: "BSTCELA2"
        );
        public static readonly Tile GrayStoneEagle = new(
            TextureNorth: "GSTEAGL1",
            TextureSouth: "GSTEAGL1",
            TextureEast: "GSTEAGL2",
            TextureWest: "GSTEAGL2"
        );
        public static readonly Tile BlueStoneCellSkeleton = new(
            TextureNorth: "BSTCELB1",
            TextureSouth: "BSTCELB1",
            TextureEast: "BSTCELB2",
            TextureWest: "BSTCELB2"
        );
        public static readonly Tile BlueStone1 = new(
            TextureNorth: "BSTONEA1",
            TextureSouth: "BSTONEA1",
            TextureEast: "BSTONEA2",
            TextureWest: "BSTONEA2"
        );
        public static readonly Tile BlueStone2 = new(
            TextureNorth: "BSTONEB1",
            TextureSouth: "BSTONEB1",
            TextureEast: "BSTONEB2",
            TextureWest: "BSTONEB2"
        );
        public static readonly Tile WoodEagle = new(
            TextureNorth: "WODEAGL1",
            TextureSouth: "WODEAGL1",
            TextureEast: "WODEAGL2",
            TextureWest: "WODEAGL2"
        );
        public static readonly Tile WoodHitler = new(
            TextureNorth: "WODHTLR1",
            TextureSouth: "WODHTLR1",
            TextureEast: "WODHTLR2",
            TextureWest: "WODHTLR2"
        );
        public static readonly Tile Wood = new(
            TextureNorth: "WOOD1",
            TextureSouth: "WOOD1",
            TextureEast: "WOOD2",
            TextureWest: "WOOD2"
        );
        public static readonly Tile FakeDoor = new(
            TextureNorth: "FAKEDOR1",
            TextureSouth: "FAKEDOR1",
            TextureEast: "FAKEDOR2",
            TextureWest: "FAKEDOR2"
        );
        public static readonly Tile SteelPanelSign = new(
            TextureNorth: "METLSGN1",
            TextureSouth: "METLSGN1",
            TextureEast: "METLSGN2",
            TextureWest: "METLSGN2"
        );
        public static readonly Tile Metal = new(
            TextureNorth: "METAL1",
            TextureSouth: "METAL1",
            TextureEast: "METAL2",
            TextureWest: "METAL2"
        );
        public static readonly Tile Sky = new(
            TextureNorth: "SKY1",
            TextureSouth: "SKY1",
            TextureEast: "SKY2",
            TextureWest: "SKY2"
        );
        public static readonly Tile RedBrick = new(
            TextureNorth: "BRICK1",
            TextureSouth: "BRICK1",
            TextureEast: "BRICK2",
            TextureWest: "BRICK2"
        );
        public static readonly Tile RedBrickWreath = new(
            TextureNorth: "BRIKWRT1",
            TextureSouth: "BRIKWRT1",
            TextureEast: "BRIKWRT2",
            TextureWest: "BRIKWRT2"
        );
        public static readonly Tile Purple = new(
            TextureNorth: "PURPLE1",
            TextureSouth: "PURPLE1",
            TextureEast: "PURPLE2",
            TextureWest: "PURPLE2"
        );
        public static readonly Tile RedBrickEagle = new(
            TextureNorth: "BRIKEGL1",
            TextureSouth: "BRIKEGL1",
            TextureEast: "BRIKEGL2",
            TextureWest: "BRIKEGL2"
        );
        public static readonly Tile Elevator1 = new(
            TextureNorth: "ELEV1_1",
            TextureSouth: "ELEV1_1",
            TextureEast: "ELEV1_2",
            TextureWest: "ELEV1_2"
        );
        public static readonly Tile Elevator2 = new(
            TextureNorth: "ELEV2_1",
            TextureSouth: "ELEV2_1",
            TextureEast: "ELEV2_2",
            TextureWest: "ELEV2_2"
        );
        public static readonly Tile WoodCross = new(
            TextureNorth: "WODCROS1",
            TextureSouth: "WODCROS1",
            TextureEast: "WODCROS2",
            TextureWest: "WODCROS2"
        );
        public static readonly Tile GrayStoneSlime1 = new(
            TextureNorth: "GSTSLME1",
            TextureSouth: "GSTSLME1",
            TextureEast: "GSTSLME2",
            TextureWest: "GSTSLME2"
        );
        public static readonly Tile PurpleBlood = new(
            TextureNorth: "PURPBLD1",
            TextureSouth: "PURPBLD1",
            TextureEast: "PURPBLD2",
            TextureWest: "PURPBLD2"
        );
        public static readonly Tile GrayStoneSlime2 = new(
            TextureNorth: "GSTLSLM1",
            TextureSouth: "GSTLSLM1",
            TextureEast: "GSTLSLM2",
            TextureWest: "GSTLSLM2"
        );
        public static readonly Tile GrayStone3 = new(
            TextureNorth: "GSTONEC1",
            TextureSouth: "GSTONEC1",
            TextureEast: "GSTONEC2",
            TextureWest: "GSTONEC2"
        );
        public static readonly Tile GrayStoneSign = new(
            TextureNorth: "GSTSIGN1",
            TextureSouth: "GSTSIGN1",
            TextureEast: "GSTSIGN2",
            TextureWest: "GSTSIGN2"
        );
        public static readonly Tile ChippedRock = new(
            TextureNorth: "CHIP1",
            TextureSouth: "CHIP1",
            TextureEast: "CHIP2",
            TextureWest: "CHIP2"
        );
        public static readonly Tile ChippedRockBlood1 = new(
            TextureNorth: "CHPBLDA1",
            TextureSouth: "CHPBLDA1",
            TextureEast: "CHPBLDA2",
            TextureWest: "CHPBLDA2"
        );
        public static readonly Tile ChippedRockBlood2 = new(
            TextureNorth: "CHPBLDB1",
            TextureSouth: "CHPBLDB1",
            TextureEast: "CHPBLDB2",
            TextureWest: "CHPBLDB2"
        );
        public static readonly Tile ChippedRockBlood3 = new(
            TextureNorth: "CHPBLDC1",
            TextureSouth: "CHPBLDC1",
            TextureEast: "CHPBLDC2",
            TextureWest: "CHPBLDC2"
        );
        public static readonly Tile StainedGlass = new(
            TextureNorth: "GLASS1",
            TextureSouth: "GLASS1",
            TextureEast: "GLASS2",
            TextureWest: "GLASS2"
        );
        public static readonly Tile BlueBrickSkull = new(
            TextureNorth: "BLUSKUL1",
            TextureSouth: "BLUSKUL1",
            TextureEast: "BLUSKUL2",
            TextureWest: "BLUSKUL2"
        );
        public static readonly Tile GrayBrick = new(
            TextureNorth: "GRYBRIK1",
            TextureSouth: "GRYBRIK1",
            TextureEast: "GRYBRIK2",
            TextureWest: "GRYBRIK2"
        );
        public static readonly Tile BlueBrickSwastika = new(
            TextureNorth: "BLUSWAS1",
            TextureSouth: "BLUSWAS1",
            TextureEast: "BLUSWAS2",
            TextureWest: "BLUSWAS2"
        );
        public static readonly Tile GrayStoneVent = new(
            TextureNorth: "GRYVENT1",
            TextureSouth: "GRYVENT1",
            TextureEast: "GRYVENT2",
            TextureWest: "GRYVENT2"
        );
        public static readonly Tile OddBricks = new(
            TextureNorth: "BRIKODD1",
            TextureSouth: "BRIKODD1",
            TextureEast: "BRIKODD2",
            TextureWest: "BRIKODD2"
        );
        public static readonly Tile GrayBrickCrack = new(
            TextureNorth: "GRYCRAK1",
            TextureSouth: "GRYCRAK1",
            TextureEast: "GRYCRAK2",
            TextureWest: "GRYCRAK2"
        );
        public static readonly Tile BlueBrickWall = new(
            TextureNorth: "BLUWALL1",
            TextureSouth: "BLUWALL1",
            TextureEast: "BLUWALL2",
            TextureWest: "BLUWALL2"
        );
        public static readonly Tile BlueStoneSign = new(
            TextureNorth: "BSTSIGN1",
            TextureSouth: "BSTSIGN1",
            TextureEast: "BSTSIGN2",
            TextureWest: "BSTSIGN2"
        );
        public static readonly Tile BrownMarble1 = new(
            TextureNorth: "MARB1_1",
            TextureSouth: "MARB1_1",
            TextureEast: "MARB1_2",
            TextureWest: "MARB1_2"
        );
        public static readonly Tile GrayBrickMap = new(
            TextureNorth: "GRYMAPS1",
            TextureSouth: "GRYMAPS1",
            TextureEast: "GRYMAPS2",
            TextureWest: "GRYMAPS2"
        );
        public static readonly Tile BrownStone1 = new(
            TextureNorth: "BRNSTNA1",
            TextureSouth: "BRNSTNA1",
            TextureEast: "BRNSTNA2",
            TextureWest: "BRNSTNA2"
        );
        public static readonly Tile BrownStone2 = new(
            TextureNorth: "BRNSTNB1",
            TextureSouth: "BRNSTNB1",
            TextureEast: "BRNSTNB2",
            TextureWest: "BRNSTNB2"
        );
        public static readonly Tile BrownMarble2 = new(
            TextureNorth: "MARB2_1",
            TextureSouth: "MARB2_1",
            TextureEast: "MARB2_2",
            TextureWest: "MARB2_2"
        );
        public static readonly Tile BrownMarbleFlag = new(
            TextureNorth: "MARB3_1",
            TextureSouth: "MARB3_1",
            TextureEast: "MARB3_2",
            TextureWest: "MARB3_2"
        );
        public static readonly Tile Plaster = new(
            TextureNorth: "PLASTER1",
            TextureSouth: "PLASTER1",
            TextureEast: "PLASTER2",
            TextureWest: "PLASTER2"
        );
        public static readonly Tile GrayBrickHitler = new(
            TextureNorth: "GRYHTLR1",
            TextureSouth: "GRYHTLR1",
            TextureEast: "GRYHTLR2",
            TextureWest: "GRYHTLR2"
        );
        public static readonly Tile Door = new(
            TextureNorth: "DOOR1_1",
            TextureSouth: "DOOR1_1",
            TextureEast: "DOOR1_2",
            TextureWest: "DOOR1_2"
        );
        public static readonly Tile Slot = new(
            TextureNorth: "SLOT1_1",
            TextureSouth: "SLOT1_1",
            TextureEast: "SLOT1_2",
            TextureWest: "SLOT1_2"
        );
        public static readonly Tile Door2 = new(
            TextureNorth: "DOOR2_1",
            TextureSouth: "DOOR2_1",
            TextureEast: "DOOR2_2",
            TextureWest: "DOOR2_2"
        );
        public static readonly Tile Door3 = new(
            TextureNorth: "DOOR3_1",
            TextureSouth: "DOOR3_1",
            TextureEast: "DOOR3_2",
            TextureWest: "DOOR3_2"
        );
        public static readonly Tile Elevator = new(
            TextureNorth: "ELEV1_1",
            TextureSouth: "ELEV1_1",
            TextureEast: "ELEV1_2",
            TextureWest: "ELEV1_2"
        );
        public static readonly Tile DoorEastWest = new(
            TextureNorth: "SLOT1_1",
            TextureSouth: "SLOT1_1",
            TextureEast: "DOOR1_2",
            TextureWest: "DOOR1_2",
            OffsetVertical: true
        );
        public static readonly Tile DoorNorthSouth = new(
            TextureNorth: "DOOR1_1",
            TextureSouth: "DOOR1_1",
            TextureEast: "SLOT1_2",
            TextureWest: "SLOT1_2",
            OffsetHorizontal: true
        );
        public static readonly Tile LockedDoorEastWest = new(
            TextureNorth: "SLOT1_1",
            TextureSouth: "SLOT1_1",
            TextureEast: "DOOR3_2",
            TextureWest: "DOOR3_2",
            OffsetVertical: true
        );
        public static readonly Tile LockedDoorNorthSouth = new(
            TextureNorth: "DOOR3_1",
            TextureSouth: "DOOR3_1",
            TextureEast: "SLOT1_2",
            TextureWest: "SLOT1_2",
            OffsetHorizontal: true
        );
        public static readonly Tile OtherDoorEastWest = new(
            TextureNorth: "SLOT1_1",
            TextureSouth: "SLOT1_1",
            TextureEast: "DOOR2_2",
            TextureWest: "DOOR2_2",
            OffsetVertical: true
        );
        public static readonly Tile OtherDoorNorthSouth = new(
            TextureNorth: "DOOR2_1",
            TextureSouth: "DOOR2_1",
            TextureEast: "SLOT1_2",
            TextureWest: "SLOT1_2",
            OffsetHorizontal: true
        );

        public static readonly IReadOnlyDictionary<int, Tile> Lookup = new Dictionary<int, Tile>
        {
            {1, GrayStone1},
            {2, GrayStone2},
            {3, GrayStoneFlag},
            {4, GrayStoneHitler},
            {5, BlueStoneEmptyCell},
            {6, GrayStoneEagle},
            {7, BlueStoneCellSkeleton},
            {8, BlueStone1},
            {9, BlueStone2},
            {10, WoodEagle},
            {11, WoodHitler},
            {12, Wood},
            {13, FakeDoor},
            {14, SteelPanelSign},
            {15, Metal},
            {16, Sky},
            {17, RedBrick},
            {18, RedBrickWreath},
            {19, Purple},
            {20, RedBrickEagle},
            {21, Elevator1},
            {22, Elevator2},
            {23, WoodCross},
            {24, GrayStoneSlime1},
            {25, PurpleBlood},
            {26, GrayStoneSlime2},
            {27, GrayStone3},
            {28, GrayStoneSign},
            {29, ChippedRock},
            {30, ChippedRockBlood1},
            {31, ChippedRockBlood2},
            {32, ChippedRockBlood3},
            {33, StainedGlass},
            {34, BlueBrickSkull},
            {35, GrayBrick},
            {36, BlueBrickSwastika},
            {37, GrayStoneVent},
            {38, OddBricks},
            {39, GrayBrickCrack},
            {40, BlueBrickWall},
            {41, BlueStoneSign},
            {42, BrownMarble1},
            {43, GrayBrickMap},
            {44, BrownStone1},
            {45, BrownStone2},
            {46, BrownMarble2},
            {47, BrownMarbleFlag},
            {48, Plaster},
            {49, GrayBrickHitler},
            {50, Door},
            {51, Slot},
            {52, Door2},
            {53, Door3},
            {85, Elevator},
            {90, DoorEastWest},
            {91, DoorNorthSouth},
            {92, LockedDoorEastWest},
            {93, LockedDoorNorthSouth},
            {94, LockedDoorEastWest},
            {95, LockedDoorNorthSouth},
            {96, LockedDoorEastWest},
            {97, LockedDoorNorthSouth},
            {98, LockedDoorEastWest},
            {99, LockedDoorNorthSouth},
            {100, OtherDoorEastWest},
            {101, OtherDoorNorthSouth},
        };
    }
}