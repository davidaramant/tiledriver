// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;

namespace Tiledriver.Core.Uwmf.Metadata
{
    /// <summary>
    /// The global properties and planemap are special cases
    /// </summary>
    public static class UwmfDefinitions
    {
        public static readonly IEnumerable<UwmfBlock> Blocks = new[]
        {
            new UwmfBlock("tile",
                UwmfProperty.RequiredString("textureEast"),
                UwmfProperty.RequiredString("textureNorth"),
                UwmfProperty.RequiredString("textureWest"),
                UwmfProperty.RequiredString("textureSouth"),
                UwmfProperty.OptionalBoolean("blockingEast", true),
                UwmfProperty.OptionalBoolean("blockingNorth", true),
                UwmfProperty.OptionalBoolean("blockingWest", true),
                UwmfProperty.OptionalBoolean("blockingSouth", true),
                UwmfProperty.OptionalBoolean("offsetVertical", false),
                UwmfProperty.OptionalBoolean("offsetHorizontal", false),
                UwmfProperty.OptionalBoolean("dontOverlay", false),
                UwmfProperty.OptionalIntegerNumber("mapped", 0),
                UwmfProperty.OptionalString("soundSequence", String.Empty),
                UwmfProperty.OptionalString("textureOverhead", String.Empty)),
            new UwmfBlock("sector",
                UwmfProperty.RequiredString("textureCeiling"),
                UwmfProperty.RequiredString("textureFloor")),
            new UwmfBlock("zone"),
            new UwmfBlock("plane",
                UwmfProperty.RequiredIntegerNumber("depth")),
            new UwmfBlock("thing",
                UwmfProperty.RequiredIntegerNumber("type"),
                UwmfProperty.RequiredFloatingPointNumber("x"),
                UwmfProperty.RequiredFloatingPointNumber("y"),
                UwmfProperty.RequiredFloatingPointNumber("z"),
                UwmfProperty.RequiredIntegerNumber("angle"),
                UwmfProperty.OptionalBoolean("ambush", false),
                UwmfProperty.OptionalBoolean("patrol", false),
                UwmfProperty.OptionalBoolean("skill1", false),
                UwmfProperty.OptionalBoolean("skill2", false),
                UwmfProperty.OptionalBoolean("skill3", false),
                UwmfProperty.OptionalBoolean("skill4", false),
                UwmfProperty.OptionalBoolean("skill5", false)),
            new UwmfBlock("trigger",
                UwmfProperty.RequiredIntegerNumber("x"),
                UwmfProperty.RequiredIntegerNumber("y"),
                UwmfProperty.RequiredIntegerNumber("z"),
                UwmfProperty.RequiredIntegerNumber("action"),
                UwmfProperty.OptionalIntegerNumber("arg0",0),
                UwmfProperty.OptionalIntegerNumber("arg1",0),
                UwmfProperty.OptionalIntegerNumber("arg2",0),
                UwmfProperty.OptionalIntegerNumber("arg3",0),
                UwmfProperty.OptionalIntegerNumber("arg4",0),
                UwmfProperty.OptionalBoolean("activateEast", true),
                UwmfProperty.OptionalBoolean("activateNorth", true),
                UwmfProperty.OptionalBoolean("activateWest", true),
                UwmfProperty.OptionalBoolean("activateSouth", true),
                UwmfProperty.OptionalBoolean("playerCross", false),
                UwmfProperty.OptionalBoolean("playerUse", false),
                UwmfProperty.OptionalBoolean("monsterUse", false),
                UwmfProperty.OptionalBoolean("repeatable", false),
                UwmfProperty.OptionalBoolean("secret", false)),
            new UwmfBlock("map1",
                UwmfProperty.RequiredIntegerNumber("x"),
                UwmfProperty.RequiredIntegerNumber("y"),
                UwmfProperty.RequiredIntegerNumber("z"),
                UwmfProperty.RequiredIntegerNumber("action"),
                UwmfProperty.OptionalIntegerNumber("arg0",0),
                UwmfProperty.OptionalIntegerNumber("arg1",0),
                UwmfProperty.OptionalIntegerNumber("arg2",0),
                UwmfProperty.OptionalIntegerNumber("arg3",0),
                UwmfProperty.OptionalIntegerNumber("arg4",0),
                UwmfProperty.OptionalBoolean("activateEast", true),
                UwmfProperty.OptionalBoolean("activateNorth", true),
                UwmfProperty.OptionalBoolean("activateWest", true),
                UwmfProperty.OptionalBoolean("activateSouth", true),
                UwmfProperty.OptionalBoolean("playerCross", false),
                UwmfProperty.OptionalBoolean("playerUse", false),
                UwmfProperty.OptionalBoolean("monsterUse", false),
                UwmfProperty.OptionalBoolean("repeatable", false),
                UwmfProperty.OptionalBoolean("secret", false)),
        };
    }
}