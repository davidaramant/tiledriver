// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Diagnostics;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.FormatModels.Wad;
using Tiledriver.Core.Settings;

namespace Tiledriver.Core.Utils.UZDoom;

public sealed class UZDoomLauncher
{
	private readonly DoomConfig _config;

	public UZDoomLauncher(DoomConfig config)
	{
		if (!File.Exists(config.UZDoomExePath))
		{
			throw new FileNotFoundException("Could not find UZDoom EXE: " + config.UZDoomExePath);
		}

		if (!File.Exists(config.Doom2IwadPath))
		{
			throw new FileNotFoundException("Could not find Doom 2 IWAD: " + config.Doom2IwadPath);
		}

		_config = config;
	}

	/// <summary>
	/// Create a WAD with the given map
	/// </summary>
	/// <param name="udmfMap">The map to save</param>
	/// <param name="wadFilePath">
	/// (Optional) The path to the WAD to create. If not specified, something will be created in the temporary directory.
	/// </param>
	public void LoadMap(MapData udmfMap, string? wadFilePath = null)
	{
		var wad = new List<ILump> { new Marker("MAP01"), new UdmfLump("TEXTMAP", udmfMap), new Marker("ENDMAP") };

		CreateAndLoadWad(wad, wadFilePath);
	}

	/// <summary>
	/// Create a WAD with the given map
	/// </summary>
	/// <param name="lumps">The lumps of the WAD to load</param>
	/// <param name="wadFilePath">
	/// (Optional) The path to the WAD to create. If not specified, something will be created in the temporary directory.
	/// </param>
	public void CreateAndLoadWad(IEnumerable<ILump> lumps, string? wadFilePath = null)
	{
		wadFilePath ??= Path.Combine(Path.GetTempPath(), "demo.lumps");
		var pathToLoad = Path.GetFullPath(wadFilePath);

		WadWriter.SaveTo(lumps, pathToLoad);

		LoadWad(pathToLoad);
	}

	public void LoadWad(string wadPath)
	{
		Process.Start(
			_config.UZDoomExePath,
			$"-iwad \"{_config.Doom2IwadPath}\" -file \"{wadPath}\" -skill 4 -warp 01"
		);
	}
}
