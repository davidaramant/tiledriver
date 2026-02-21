namespace Tiledriver.Core.FormatModels.MapInfo;

public sealed class NextMapInfo
{
	public string Destination { get; }
	public bool IsEndSequence { get; }

	private NextMapInfo(string destination, bool isEndSequence)
	{
		Destination = destination;
		IsEndSequence = isEndSequence;
	}

	public static NextMapInfo Map(string mapName) => new(mapName, false);

	public static NextMapInfo EndSequence(string sequenceName) => new(sequenceName, true);
}
