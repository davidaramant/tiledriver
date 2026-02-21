namespace Tiledriver.Core.FormatModels.Wdc31;

public sealed record Header(string FileVersion, int NumberOfMaps, ushort NumberOfMapPlanes, ushort MaxMapNameLength)
{
	public static Header Read(BinaryReader reader) =>
		new(
			FileVersion: new string(reader.ReadChars(6)),
			NumberOfMaps: reader.ReadInt32(),
			NumberOfMapPlanes: reader.ReadUInt16(),
			MaxMapNameLength: reader.ReadUInt16()
		);
}
