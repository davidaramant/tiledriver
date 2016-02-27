using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;
using Tiledriver.Wolf3D;

namespace Tiledriver.Generator
{
    public static class RandomGenerator
    {
        public static Map Create(int seed)
        {
            var random = new Random(seed);

            var tagSequence = new TagSequence();
            var sparseMap = new SparseMap(64, 64);

            var regionsToGenerate = 5;

            for (int i = 0; i < regionsToGenerate; i++)
            {
                var regionShape = PickRegionShape(random);
                switch (regionShape)
                {
                    case RegionShape.RectangularRoom:
                        sparseMap.AddRegion(BuildRectangularRoom(random, tagSequence));
                        break;
                }
            }

            return sparseMap.Compile();
        }

        private static RegionShape PickRegionShape(Random random)
        {
            var probability = random.Next(100);

            return RegionShape.RectangularRoom;
            //if (probability < 80)
            //    return RegionShape.RectangularRoom;
            //else if (probability < 90)
            //    return RegionShape.HorizontalHallway;
            //else
            //    return RegionShape.VerticalHallway;
        }

        private static IRegion BuildRectangularRoom(Random random, TagSequence tagSequence)
        {
            var regionTheme = GetRegionThemeForRoom(random);

            // Inclusive lower bound, exclusive upper bound
            var width = random.Next(5, 16);
            var height = random.Next(5, 16);

            // TODO: Add in decoration walls
            // TODO: Location
            return new Room(
                boundingBox: new Rectangle(x: 0, y: 0, width: width, height: height),
                tiles: CreateBoxOfMapTiles(width, height, regionTheme.NormalWalls, random),
                tagSequence: tagSequence);
        }

        private static MapTile[,] CreateBoxOfMapTiles(int width, int height, IEnumerable<TileTheme> wallChoices, Random random)
        {
            // TODO: Probability distributions for wall themes

            var entries = new MapTile[height, width];

            // Top wall
            for (var col = 0; col < width; col++)
            {
                entries[0, col] = MapTile.Textured(GetRandomTheme(wallChoices, random));
            }

            for (var row = 1; row < height - 1; row++)
            {
                entries[row, 0] = MapTile.Textured(GetRandomTheme(wallChoices, random));
                for (var col = 1; col < width - 1; col++)
                {
                    entries[row, col] = MapTile.EmptyTile;
                }
                entries[row, width - 1] = MapTile.Textured(GetRandomTheme(wallChoices, random));
            }

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = MapTile.Textured(GetRandomTheme(wallChoices, random));
            }

            return entries;
        }

        private static TileTheme GetRandomTheme(IEnumerable<TileTheme> options, Random random)
        {
            var choiceIndex = random.Next(options.Count());
            return options.ElementAt(choiceIndex);
        }

        private static RegionTheme GetRegionThemeForRoom(Random random)
        {
            var probability = random.Next(100);

            if (probability < 50)
                return RegionTheme.Dungeon;
            else
                return RegionTheme.OfficerLounge;
        }
    }
}
