using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common.BinaryMaps;

namespace Tiledriver.Core.FormatModels.GameMaps;

public sealed class GameMapsBundle
{
	private readonly ushort _rlewMarker;
	public ImmutableArray<MapHeader> Maps { get; }

	private GameMapsBundle(ushort rlewMarker, ImmutableArray<MapHeader> mapHeaders)
	{
		_rlewMarker = rlewMarker;
		Maps = mapHeaders;
	}

	public static GameMapsBundle Load(Stream headerStream, Stream mapsStream)
	{
		var offsetData = OffsetData.ReadOffsets(headerStream);

		var headerBuffer = new byte[42];
		var headers = offsetData.Offsets.Select(offset =>
		{
			mapsStream.Position = offset;
			mapsStream.ReadExactly(headerBuffer, 0, headerBuffer.Length);
			return MapHeader.Parse(headerBuffer);
		});

		return new GameMapsBundle(offsetData.RlewMarker, [.. headers]);
	}

	public BinaryMap LoadMap(int mapIndex, Stream mapsStream)
	{
		var header = Maps[mapIndex];

		var plane0Data = new ushort[header.Height * header.Width];
		var plane1Data = new ushort[header.Height * header.Width];
		var plane2Data = new ushort[header.Height * header.Width];

		LoadPlane(plane0Data, header.Plane0Info, mapsStream);
		LoadPlane(plane1Data, header.Plane1Info, mapsStream);
		LoadPlane(plane2Data, header.Plane2Info, mapsStream);

		return new BinaryMap(
			name: header.Name,
			width: header.Width,
			height: header.Height,
			planes: [plane0Data, plane1Data, plane2Data]
		);
	}

	private void LoadPlane(ushort[] planeData, PlaneMetadata planeInfo, Stream mapsStream)
	{
		var buffer = new byte[planeInfo.CompressedLength];
		mapsStream.Position = planeInfo.Offset;
		mapsStream.ReadExactly(buffer, 0, buffer.Length);

		var uncarmacked = Expander.DecompressCarmack(buffer);

		var finalSize = BitConverter.ToUInt16(uncarmacked, 0);
		var rlewData = new byte[uncarmacked.Length - 2];
		Buffer.BlockCopy(uncarmacked, 2, rlewData, 0, rlewData.Length);

		// TODO: This array can also be created once per map and reused
		var finalBytes = Expander.DecompressRlew(_rlewMarker, rlewData, finalSize);

		Buffer.BlockCopy(finalBytes, 0, planeData, 0, finalBytes.Length);
	}
}
