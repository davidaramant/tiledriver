using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.GameInfo.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping;

public class Passage : IEquatable<Passage>
{
	public MapLocation Location { get; }

	public Trigger? Door => Location.Triggers.SingleOrDefault(t => t.Action == "Door_Open");

	public LockLevel LockLevel
	{
		get
		{
			var door = Door;
			if (null == door)
				return LockLevel.None;

			return (LockLevel)door.Arg3;
		}
	}

	public Trigger? Pushwall => Location.Triggers.SingleOrDefault(t => t.Action == "Pushwall_Move");

	public Passage(MapLocation loc)
	{
		Location = loc;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as Passage);
	}

	public override int GetHashCode()
	{
		return (Location != null ? Location.GetHashCode() : 0);
	}

	public bool Equals(Passage? other)
	{
		if (null == other)
			return false;

		return Location.Equals(other.Location);
	}
}
