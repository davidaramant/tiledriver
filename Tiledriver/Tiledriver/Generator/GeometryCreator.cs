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
        public static AbstractGeometry Create(int mapWidth, int mapHeight, int maxNumberOfRooms, Random random)
        {
            const int maxErrorCount = 10;

            // Make the rooms
            var roomsWithDistances = new Graph<Rectangle,WeightedEdge<Rectangle,int?>>();

            int currentErrorCount = 0;
            while (roomsWithDistances.NodeCount <= maxNumberOfRooms)
            {
                if (currentErrorCount == maxErrorCount)
                {
                    break;
                }

                var room = CreateRandomRectangle(mapWidth: mapWidth, mapHeight: mapHeight, random: random);

                if (roomsWithDistances.Any(existingRegion => existingRegion.PaddedIntersectsWith(room, padding: 2)))
                {
                    currentErrorCount++;
                    continue;
                }

                roomsWithDistances.AddNode(room);
                currentErrorCount = 0;
            }

            // Constuct all edges and find distances between every room
            var allEdges = new HashSet<Edge<Rectangle>>();
            foreach (var room1 in roomsWithDistances)
            {
                foreach (var room2 in roomsWithDistances.Except(new[] { room1 }))
                {
                    allEdges.Add(new WeightedEdge<Rectangle,int?>(room1, room2, room1.StraightDistanceFrom(room2)));
                }
            }

            // If there are rooms without valid straight distances to other rooms, throw them out
            var roomsWithoutValidDistances =
                roomsWithDistances.Where(
                    room => !roomsWithDistances.AllEdges.
                        Where(weightedEdge => weightedEdge.Involves(room)).
                        All(weightedEdge => weightedEdge.Cost.HasValue)).
                    ToArray();
            foreach (var lonelyRoom in roomsWithoutValidDistances)
            {
                roomsWithDistances.Remove(lonelyRoom);
            }

            // Make the connection graph
            var connectionGraph = new Graph<Rectangle,Edge<Rectangle>>();
            foreach (var room in roomsWithDistances)
            {
                connectionGraph.AddNode(room);
            }

            // For every room, find the closest room
            foreach (var room in roomsWithDistances)
            {
                var closestRoom =
                    roomsWithDistances.AllEdges.
                    Where(weightedEdge => weightedEdge.Involves(room)).
                    Where(weightedEdge => weightedEdge.Cost.HasValue).
                    OrderBy(weightedEdge => weightedEdge.Cost.Value).
                    Single();


            }

            // Make the hallways and place doors



            var result = new AbstractGeometry();
            result.Rooms.AddRange(roomsWithDistances);
            //result.Hallways
            //result.Doors
            return result;
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
