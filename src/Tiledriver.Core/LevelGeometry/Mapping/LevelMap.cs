namespace Tiledriver.Core.LevelGeometry.Mapping;

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
