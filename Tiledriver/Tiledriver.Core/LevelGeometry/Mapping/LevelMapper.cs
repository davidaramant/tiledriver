// Copyright (c) 2017, Leon Organ
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    public static class LevelMapper
    {
        public static IRoom Map(MapData data)
        {
            var startPosition = FindStart(data);

            return MapRoom(data, startPosition);
        }

        private static MapLocation FindStart(MapData data)
        {
            var startThing = data.Things.Single(thing => thing.Type == Actor.Player1Start.ClassName);

            return new MapLocation(data, (int)Math.Floor(startThing.X), (int)Math.Floor(startThing.Y));
        }

        private static IRoom MapRoom(MapData data, MapLocation firstLocation)
        {
            var discoveredLocations = new List<MapLocation>();

            discoveredLocations.Add(firstLocation);

            ExpandRoom(discoveredLocations, firstLocation);

            return null;
        }

        private static void ExpandRoom(IList<MapLocation> discoveredLocations, MapLocation fromLocation)
        {
            if (fromLocation.CanMoveUp())
            {
                
            }
        }

        
    }
}
