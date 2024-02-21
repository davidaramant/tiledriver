// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
	public class LevelMap
	{
		public IRoom StartingRoom { get; }

		public IEnumerable<IRoom> AllRooms { get; }

		public LevelMap(IRoom startingRoom, IEnumerable<IRoom> allRooms)
		{
			StartingRoom = startingRoom;
			AllRooms = allRooms;
		}

		public IEnumerable<IRoom> EndingRooms => AllRooms.Where(room => room.IsEndingRoom);
	}
}
