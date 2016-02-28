using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Graphs;

namespace Tiledriver.Generator
{
    public static class GeometryCreator
    {
        public static AbstractGeometry Create(int width, int height, Random random)
        {
            const int maxErrorCount = 10;
            const int maxNumberOfRooms = 5;

            // Make the rooms
            var rooms = new Graph<Rectangle,int?>();

            int currentErrorCount = 0;
            while (rooms.NodeCount <= maxNumberOfRooms)
            {
                if (currentErrorCount == maxErrorCount)
                {
                    break;
                }

                var room = CreateRandomRectangle(mapWidth: width, mapHeight: height, random: random);

                if (rooms.Any(existingRegion => existingRegion.IntersectsWith(room)))
                {
                    currentErrorCount++;
                    continue;
                }

                rooms.AddNode(room);
                currentErrorCount = 0;
            }

            // Constuct all edges
            var allEdges = new HashSet<GraphEdge<Rectangle>>();
            foreach (var room1 in rooms)
            {
                foreach (var room2 in rooms.Except(new[] {room1}))
                {
                    allEdges.Add(new GraphEdge<Rectangle>(room1, room2));
                }
            }

            // Find distances between every room
            foreach (var edge in allEdges)
            {
                rooms.AddWeightedEdge(edge,edge.Node1.StraightDistanceFrom(edge.Node2));
            }


            // Make the hallways and place doors



            return new AbstractGeometry();
        }

        private static Rectangle CreateRandomRectangle(int mapWidth, int mapHeight, Random random)
        {
            var roomWidth = random.Next(minValue: 5, maxValue: 16);
            var roomHeight = random.Next(minValue: 5, maxValue: 16);

            var left = random.Next(0, mapWidth - roomWidth);
            var top = random.Next(0, mapHeight - roomHeight);

            return new Rectangle(x: left, y: top, width: roomWidth, height: roomHeight);
        }
    }
}
