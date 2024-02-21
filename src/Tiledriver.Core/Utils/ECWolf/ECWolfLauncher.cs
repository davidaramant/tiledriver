// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Wad;

namespace Tiledriver.Core.Utils.ECWolf
{
	/// <summary>
	/// Launches ECWolf
	/// </summary>
	public class ECWolfLauncher
	{
		private readonly string _ecWolfExePath;

		public ECWolfLauncher(string ecWolfExePath)
		{
			if (!File.Exists(ecWolfExePath))
			{
				throw new FileNotFoundException("Could not find ECWolf EXE: " + ecWolfExePath);
			}
			_ecWolfExePath = ecWolfExePath;
		}

		/// <summary>
		/// Create a WAD with the given map
		/// </summary>
		/// <param name="uwmfMap">The map to save</param>
		/// <param name="wadFilePath">
		/// (Optional) The path to the WAD to create. If not specified, something will be created in the temporary directory.
		/// </param>
		public void LoadMapInEcWolf(MapData uwmfMap, string? wadFilePath = null)
		{
			var wad = new List<ILump> { new Marker("MAP01"), new UwmfLump("TEXTMAP", uwmfMap), new Marker("ENDMAP") };

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
			Process.Start(_ecWolfExePath, $"--file \"{wadPath}\" --data wl6 --hard --nowait --tedlevel map01");
		}
	}
}
