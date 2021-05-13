// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Diagnostics;
using System.IO;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Wad;

namespace Tiledriver.Core.ECWolfUtils
{
    /// <summary>
    /// Launches ECWolf
    /// </summary>
    public class Launcher
    {
        private readonly string _ecWolfExePath;

        public Launcher(string ecWolfExePath)
        {
            if (!File.Exists(ecWolfExePath))
            {
                throw new FileNotFoundException("Could not find ECWolf EXE.");
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
        public void LoadMapInEcWolf(
            MapData uwmfMap, 
            string? wadFilePath = null)
        {
            var wad = new WadFile();
            wad.Append(new Marker("MAP01"));
            wad.Append(new UwmfLump("TEXTMAP", uwmfMap));
            wad.Append(new Marker("ENDMAP"));

            LoadWadInEcWolf(wad, wadFilePath);
        }

        /// <summary>
        /// Create a WAD with the given map
        /// </summary>
        /// <param name="wad">The WAD to load</param>
        /// <param name="wadFilePath">
        /// (Optional) The path to the WAD to create. If not specified, something will be created in the temporary directory.
        /// </param>
        public void LoadWadInEcWolf(
            WadFile wad, 
            string? wadFilePath = null)
        {
            wadFilePath ??= Path.Combine(Path.GetTempPath(), "demo.wad");
            var pathToLoad = Path.GetFullPath(wadFilePath);

            wad.SaveTo(pathToLoad);

            LoadWadInEcWolf(pathToLoad);
        }

        public void LoadWadInEcWolf(string wadPath)
        {
            Process.Start(
                _ecWolfExePath,
                $"--file \"{wadPath}\" --data wl6 --hard --nowait --tedlevel map01");
        }
    }
}