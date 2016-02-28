using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.Generator.SimpleGeometry
{
    public class SimpleGeometryCreator
    {
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Random _random;

        public SimpleGeometryCreator(int mapWidth, int mapHeight, Random random)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _random = random;
        }

        public static AbstractGeometry Create(int mapWidth, int mapHeight, Random random)
        {
            var results = new AbstractGeometry();

            return results;
        }


    }
}
