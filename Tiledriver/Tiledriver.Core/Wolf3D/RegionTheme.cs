/*
** RegionTheme.cs
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.Core.Wolf3D
{
    public sealed class RegionTheme
    {
        public readonly List<TileTheme> NormalWalls;
        public readonly List<TileTheme> DecorationWalls;

        public readonly List<Actor> OrderedDecorations;
        public readonly List<Actor> RandomDecorations;

        public readonly List<Actor> EnemyTypes;

        private RegionTheme(
            IEnumerable<TileTheme> normalWalls,
            IEnumerable<TileTheme> decorationWalls,
            IEnumerable<Actor> orderedDecorations,
            IEnumerable<Actor> randomDecorations,
            IEnumerable<Actor> enemyTypes)
        {
            NormalWalls = normalWalls.ToList();
            DecorationWalls = decorationWalls.ToList();
            OrderedDecorations = orderedDecorations.ToList();
            RandomDecorations = randomDecorations.ToList();
            EnemyTypes = enemyTypes.ToList();
        }

        public static readonly RegionTheme Dungeon = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.BlueStone1,
                TileTheme.BlueStone2,
            },
            decorationWalls: new[]
            {
                TileTheme.BlueStoneSign,
            },
            orderedDecorations: new Actor[] {
                Actor.CeilingLight,
            },
            randomDecorations: new Actor[] {
                Actor.HangingCage,
                Actor.SkeletonCage,
                Actor.SkeletonFlat,
                Actor.Blood,
                Actor.Bones1,
                Actor.Bones2,
                Actor.Bones3,
                Actor.Bones4
            },
            enemyTypes: new Actor[]
            {
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Clip,
                Actor.Clip,
                Actor.Food,
             });

        public static readonly RegionTheme Kennel = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.BlueStone1,
                TileTheme.BlueStone2
            },
            decorationWalls: new[]{
                TileTheme.BlueStone1,
                TileTheme.BlueStone2
            },
            orderedDecorations: new Actor[] {
                Actor.CeilingLight
            },
            randomDecorations: new Actor[] {
                Actor.HangingCage,
                Actor.Basket
            },
            enemyTypes: new Actor[]
            {
                Actor.Dog,
                Actor.Dog,
                Actor.Dog,
                Actor.Dog,
                Actor.Dog,
                Actor.DogFood
             });

        public static readonly RegionTheme Cells = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.BlueStone1,
                TileTheme.BlueStone2
            },
            decorationWalls: new[]
            {
                TileTheme.BlueStoneCellSkele,
                TileTheme.BlueStoneEmptyCell
            },
            orderedDecorations: new Actor[] {
                Actor.CeilingLight
            },
            randomDecorations: new Actor[] {
                Actor.HangingCage,
                Actor.SkeletonCage,
                Actor.SkeletonFlat,
                Actor.Blood,
                Actor.Bones1,
                Actor.Bones2,
                Actor.Bones3,
                Actor.Bones4
            },
            enemyTypes: new Actor[]
            {
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.DogFood,
                Actor.DogFood
             });

        public static readonly RegionTheme OfficerLounge = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.Wood
            },
            decorationWalls: new[]
            {
                TileTheme.WoodCross,
                TileTheme.WoodEagle,
                TileTheme.WoodHitler
            },
            orderedDecorations: new Actor[] {
                Actor.SuitOfArmor,
                Actor.BrownPlant,
                Actor.Vase,
                Actor.FloorLamp
            },
            randomDecorations: new Actor[] {
                Actor.BareTable,
                Actor.TableWithChairs
            },
            enemyTypes: new Actor[]
            {
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.WolfensteinSS,
                Actor.Dog,
                Actor.Food
            });

        public static readonly RegionTheme DiningHall = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.Wood
            },
            decorationWalls: new[]
            {
                TileTheme.WoodCross,
                TileTheme.WoodEagle,
                TileTheme.WoodHitler
            },
            orderedDecorations: new Actor[] {
                Actor.BareTable,
                Actor.TableWithChairs
            },
            randomDecorations: new Actor[] {
                Actor.Food,
                Actor.WhitePillar
            },
            enemyTypes: new Actor[]
            {
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.WolfensteinSS,
                Actor.Dog,
                Actor.Food
            });

        public static readonly RegionTheme Kitchen = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.GrayStone1,
                TileTheme.GrayStone2,
                TileTheme.GrayStone3
            },
            decorationWalls: new[]
            {
                TileTheme.GrayStone1,
                TileTheme.GrayStone2,
                TileTheme.GrayStone3
            },
            orderedDecorations: new Actor[] {
                Actor.BareTable
            },
            randomDecorations: new Actor[] {
                Actor.KitchenStuff,
                Actor.BareTable,
                Actor.TableWithChairs,
                Actor.Stove,
                Actor.Sink
            },
            enemyTypes: new Actor[]
            {
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.Food,
                Actor.Food,
                Actor.Food
            });

        public static readonly RegionTheme Office = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.BrownMarble1,
                TileTheme.BrownMarble2
            },
            decorationWalls: new[]
            {
                TileTheme.BrownMarbleFlag
            },
            orderedDecorations: new Actor[] { },
            randomDecorations: new Actor[] { },
            enemyTypes: new Actor[]
            {
                Actor.Guard,
                Actor.Guard,
                Actor.Guard,
                Actor.WolfensteinSS,
                Actor.Officer,
                Actor.Clip
            });

        public static readonly RegionTheme StoneHallway = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.GrayStone1,
                TileTheme.GrayStone2,
                TileTheme.GrayStone3
            },
            decorationWalls: new[]
            {
                TileTheme.GrayStoneEagle,
                TileTheme.GrayStoneHitler,
                TileTheme.GrayStoneFlag
            },
            orderedDecorations: new Actor[] {
                Actor.CeilingLight
            },
            randomDecorations: new Actor[] {
                Actor.Barrel,
                Actor.GreenBarrel,
                Actor.GreenPlant
            },
            enemyTypes: new Actor[]
            {
                Actor.Guard,
                Actor.Guard,
                Actor.Clip
            });
        public static readonly RegionTheme Armory = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.GrayStone1,
                TileTheme.GrayStone2,
                TileTheme.GrayStone3
            },
            decorationWalls: new[]
            {
                TileTheme.GrayStone1,
                TileTheme.GrayStone2,
                TileTheme.GrayStone3
            },
            orderedDecorations: new Actor[] {
                Actor.SuitOfArmor,
                Actor.Spears,
                Actor.BareTable
            },
            randomDecorations: new Actor[] {
                Actor.Clip,
                Actor.Clip,
                Actor.Clip,
                Actor.Clip,
                Actor.Clip,
                Actor.Clip,
                Actor.Clip,
                Actor.Clip,
                Actor.Clip,
                Actor.MachineGun
            },
            enemyTypes: new Actor[]
            {
                Actor.Guard,
                Actor.Guard,
                Actor.Clip
            });
        public static readonly RegionTheme EndRoom = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.DaytimeSky,
            },
            decorationWalls: new[]
            {
                TileTheme.DaytimeSky
            },
            orderedDecorations: new Actor[] {
                Actor.Vines
            },
            randomDecorations: new Actor[] {
                Actor.GatlingGunUpgrade,
                Actor.OneUp
            },
            enemyTypes: new Actor[]
            {
                Actor.WolfensteinSS,
                Actor.Clip
            });

        public static readonly RegionTheme HitlerRoom = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.GrayBrick,
                TileTheme.GrayBrickCrack,
            },
            decorationWalls: new[]
            {
                TileTheme.GrayBrickHitler,
                TileTheme.GrayBrickMap
            },
            orderedDecorations: new Actor[] {
                Actor.Medikit
            },
            randomDecorations: new Actor[] {
                Actor.GatlingGunUpgrade,
                Actor.Clip
            },
            enemyTypes: new Actor[]
            {
                Actor.MechaHitler
            });


        public static IEnumerable<RegionTheme> GetAvailableThemes()
        {
            yield return Dungeon;
            yield return Kennel;
            yield return Cells;
            yield return OfficerLounge;
            yield return DiningHall;
            yield return Kitchen;
            yield return Office;
            yield return StoneHallway;
            yield return Armory;
        }
    }
}
