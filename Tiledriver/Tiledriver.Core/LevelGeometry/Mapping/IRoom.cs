// Copyright (c) 2017, Leon Organ and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    public interface IRoom
    {
        IDictionary<IPassage, IRoom> AdjacentRooms { get; }
        IList<MapLocation> Locations { get; }
    }
}