using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry.Mapping;

public interface IRoom
{
	string Name { get; }
	bool IsStartingRoom { get; }
	bool IsEndingRoom { get; }
	int UnopenableDoors { get; set; }
	IList<Thing> Enemies { get; }
	IList<Thing> Bosses { get; }
	IList<Thing> Weapons { get; }
	int Ammo { get; }
	IList<Thing> Treasure { get; }
	IList<Thing> Health { get; }
	int Lives { get; }
	int BoringTiles { get; }
	bool HasGoldKey { get; }
	bool HasSilverKey { get; }

	IDictionary<IList<Passage>, IRoom> AdjacentRooms { get; }
	IList<MapLocation> Locations { get; }
}
