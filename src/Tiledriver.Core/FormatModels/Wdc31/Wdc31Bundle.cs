// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Text;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.BinaryMaps;

namespace Tiledriver.Core.FormatModels.Wdc31;

public static class Wdc31Bundle
{
	private const string FileVersion = "WDC3.1";

	public static bool IsValid(Stream mapStream)
	{
		using var reader = new BinaryReader(mapStream, Encoding.ASCII, leaveOpen: true);
		var version = new string(reader.ReadChars(6));
		bool isValid = version == FileVersion;
		mapStream.Position -= 6;
		return isValid;
	}

	public static IEnumerable<BinaryMap> ReadMaps(Stream mapStream)
	{
		using var reader = new BinaryReader(mapStream, Encoding.ASCII, leaveOpen: true);
		var header = Header.Read(reader);

		if (header.FileVersion != FileVersion)
		{
			throw new ParsingException("Invalid WDC file version.");
		}

		foreach (var mapIndex in Enumerable.Range(0, header.NumberOfMaps))
		{
			var mapName = new string(reader.ReadChars(header.MaxMapNameLength).TakeWhile(c => c != 0).ToArray()).Trim();
			var mapWidth = reader.ReadUInt16();
			var mapHeight = reader.ReadUInt16();
			var planes = new List<ushort[]>();

			foreach (var planeIndex in Enumerable.Range(0, header.NumberOfMapPlanes))
			{
				var planeData = new ushort[mapWidth * mapHeight];
				for (int i = 0; i < planeData.Length; i++)
				{
					planeData[i] = reader.ReadUInt16();
				}
				planes.Add(planeData);
			}

			yield return new BinaryMap(mapName, width: mapWidth, height: mapHeight, planes: planes);
		}
	}
}
