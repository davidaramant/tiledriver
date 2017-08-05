// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    public class Room : IRoom
    {
        public IDictionary<IPassage, IRoom> AdjacentRooms { get; }

        public Room()
        {
            AdjacentRooms = new Dictionary<IPassage, IRoom>();
        }
    }
}