using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Wolf3D;

namespace Tiledriver.Generator
{
    // TODO: Change how likely it is for each type of thing/wall to be spawned
    public sealed class RegionTheme
    {
        public readonly List<TileTheme> NormalWalls;
        public readonly List<TileTheme> DecorationWalls;

        public readonly List<WolfActor> OrderedDecorations;
        public readonly List<WolfActor> RandomDecorations;

        public readonly List<WolfActor> EnemyTypes;

        private RegionTheme(
            IEnumerable<TileTheme> normalWalls,
            IEnumerable<TileTheme> decorationWalls,
            IEnumerable<WolfActor> orderedDecorations,
            IEnumerable<WolfActor> randomDecorations,
            IEnumerable<WolfActor> enemyTypes)
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
            orderedDecorations: new WolfActor[] {
                WolfActor.CeilingLight,
            },
            randomDecorations: new WolfActor[] {
                WolfActor.EmptyCage,
                WolfActor.CageWithSkeleton,
                WolfActor.SkeletonFlat,
                WolfActor.Blood,
                WolfActor.Bones1,
                WolfActor.Bones2,
                WolfActor.Bones3,
                WolfActor.Bones4
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.Food,
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
            orderedDecorations: new WolfActor[] {
                WolfActor.CeilingLight
            },
            randomDecorations: new WolfActor[] {
                WolfActor.EmptyCage,
                WolfActor.Basket
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Dog,
                WolfActor.Dog,
                WolfActor.Dog,
                WolfActor.Dog,
                WolfActor.Dog,
                WolfActor.DogFood
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
            orderedDecorations: new WolfActor[] {
                WolfActor.CeilingLight
            },
            randomDecorations: new WolfActor[] {
                WolfActor.EmptyCage,
                WolfActor.CageWithSkeleton,
                WolfActor.SkeletonFlat,
                WolfActor.Blood,
                WolfActor.Bones1,
                WolfActor.Bones2,
                WolfActor.Bones3,
                WolfActor.Bones4
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.DogFood,
                WolfActor.DogFood
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
            orderedDecorations: new WolfActor[] {
                WolfActor.SuitOfArmor,
                WolfActor.BrownPlant,
                WolfActor.Vase,
                WolfActor.FloorLamp
            },
            randomDecorations: new WolfActor[] {
                WolfActor.Table,
                WolfActor.TableWithChairs
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.SSGuard,
                WolfActor.Dog,
                WolfActor.Food
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
            orderedDecorations: new WolfActor[] {
                WolfActor.Table,
                WolfActor.TableWithChairs
            },
            randomDecorations: new WolfActor[] {
                WolfActor.Food,
                WolfActor.WhitePillar
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.SSGuard,
                WolfActor.Dog,
                WolfActor.Food
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
            orderedDecorations: new WolfActor[] {
                WolfActor.Table
            },
            randomDecorations: new WolfActor[] {
                WolfActor.KitchenStuff,
                WolfActor.Table,
                WolfActor.TableWithChairs,
                WolfActor.Stove,
                WolfActor.Sink
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Food,
                WolfActor.Food,
                WolfActor.Food
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
            orderedDecorations: new WolfActor[] { },
            randomDecorations: new WolfActor[] { },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.SSGuard,
                WolfActor.Officer,
                WolfActor.Clip
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
            orderedDecorations: new WolfActor[] {
                WolfActor.CeilingLight
            },
            randomDecorations: new WolfActor[] {
                WolfActor.Barrel,
                WolfActor.GreenBarrel,
                WolfActor.GreenPlant
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Clip
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
            orderedDecorations: new WolfActor[] {
                WolfActor.SuitOfArmor,
                WolfActor.Spears,
                WolfActor.Table
            },
            randomDecorations: new WolfActor[] {
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.Clip,
                WolfActor.MachineGun
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Clip
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
            orderedDecorations: new WolfActor[] {
                WolfActor.Vines
            },
            randomDecorations: new WolfActor[] {
                WolfActor.GatlingGun,
                WolfActor.ExtraLife
            },
            enemyTypes: new WolfActor[]
            {
                WolfActor.SSGuard,
                WolfActor.Clip
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
