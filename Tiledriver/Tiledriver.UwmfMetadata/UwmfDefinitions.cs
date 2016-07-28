/*
** UwmfDefinitions.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

using System;
using System.Collections.Generic;

namespace Tiledriver.UwmfMetadata
{
    public static class UwmfDefinitions
    {
        public static readonly IEnumerable<UwmfBlock> Blocks = new[]
        {
            new UwmfBlock("tile").
                HasRequiredString("textureEast").
                HasRequiredString("textureNorth").
                HasRequiredString("textureWest").
                HasRequiredString("textureSouth").
                HasOptionalBoolean("blockingEast", true).
                HasOptionalBoolean("blockingNorth", true).
                HasOptionalBoolean("blockingWest", true).
                HasOptionalBoolean("blockingSouth", true).
                HasOptionalBoolean("offsetVertical", false).
                HasOptionalBoolean("offsetHorizontal", false).
                HasOptionalBoolean("dontOverlay", false).
                HasOptionalIntegerNumber("mapped", 0).
                HasOptionalString("soundSequence", String.Empty).
                HasOptionalString("textureOverhead", String.Empty).
                HasOptionalString("comment", String.Empty),

            new UwmfBlock("sector").
                HasRequiredString("textureCeiling").
                HasRequiredString("textureFloor").
                HasOptionalString("comment", String.Empty),

            new UwmfBlock("zone").
                HasOptionalString("comment", String.Empty),

            new UwmfBlock("plane").
                HasRequiredIntegerNumber("depth").
                HasOptionalString("comment", String.Empty),

            new UwmfBlock("tileSpace").
                HasRequiredIntegerNumber("tile").
                HasRequiredIntegerNumber("sector").
                HasRequiredIntegerNumber("zone").
                HasOptionalIntegerNumber("tag", 0).
                DisableNormalWriting().
                DisableNormalReading().
                CannotHaveUnknownProperties(),

            new UwmfBlock("planeMap").
                HasSubBlocks("tileSpace").
                DisableNormalReading().
                CannotHaveUnknownProperties(),

            new UwmfBlock("thing").
                HasRequiredString("type").
                HasRequiredFloatingPointNumber("x").
                HasRequiredFloatingPointNumber("y").
                HasRequiredFloatingPointNumber("z").
                HasRequiredIntegerNumber("angle").
                HasOptionalBoolean("ambush", false).
                HasOptionalBoolean("patrol", false).
                HasOptionalBoolean("skill1", false).
                HasOptionalBoolean("skill2", false).
                HasOptionalBoolean("skill3", false).
                HasOptionalBoolean("skill4", false).
                HasOptionalBoolean("skill5", false).
                HasOptionalString("comment", String.Empty),

            new UwmfBlock("trigger").
                HasRequiredIntegerNumber("x").
                HasRequiredIntegerNumber("y").
                HasRequiredIntegerNumber("z").
                HasRequiredString("action").
                HasOptionalIntegerNumber("arg0",0).
                HasOptionalIntegerNumber("arg1",0).
                HasOptionalIntegerNumber("arg2",0).
                HasOptionalIntegerNumber("arg3",0).
                HasOptionalIntegerNumber("arg4",0).
                HasOptionalBoolean("activateEast", true).
                HasOptionalBoolean("activateNorth", true).
                HasOptionalBoolean("activateWest", true).
                HasOptionalBoolean("activateSouth", true).
                HasOptionalBoolean("playerCross", false).
                HasOptionalBoolean("playerUse", false).
                HasOptionalBoolean("monsterUse", false).
                HasOptionalBoolean("repeatable", false).
                HasOptionalBoolean("secret", false).
                HasOptionalString("comment", String.Empty),

            new UwmfBlock("map").
                HasRequiredString("nameSpace", uwmfName:"namespace").
                HasRequiredIntegerNumber("tileSize").
                HasRequiredString("name").
                HasRequiredIntegerNumber("width").
                HasRequiredIntegerNumber("height").
                HasOptionalString("comment", String.Empty).
                HasSubBlocks("tile", "sector", "zone", "plane", "planeMap", "thing", "trigger").
                IsTopLevel(),
        };
    }
}