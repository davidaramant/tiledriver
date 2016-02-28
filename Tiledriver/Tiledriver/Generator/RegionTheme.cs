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
            orderedDecorations: new WolfActor[] { },
            randomDecorations: new WolfActor[] { },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
             } );

        public static readonly RegionTheme OfficerLounge = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.Wood,
            },
            decorationWalls: new[]
            {
                TileTheme.WoodCross,
                TileTheme.WoodEagle,
                TileTheme.WoodHitler,
            },
            orderedDecorations: new WolfActor[] { },
            randomDecorations: new WolfActor[] { },
            enemyTypes: new WolfActor[]
            {
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.Guard,
                WolfActor.SSGuard,
                WolfActor.Dog
            });

        public static readonly RegionTheme StoneHallway = new RegionTheme(
            normalWalls: new[]
            {
                TileTheme.GrayStone1,
                TileTheme.GrayStone2,
                TileTheme.GrayStone3,
            },
            decorationWalls: new[]
            {
                TileTheme.GrayStoneEagle,
                TileTheme.GrayStoneHitler,
                TileTheme.GrayStoneFlag,
            },
            orderedDecorations: new WolfActor[] { },
            randomDecorations: new WolfActor[] { },
            enemyTypes: new WolfActor[] { });
    }
}
