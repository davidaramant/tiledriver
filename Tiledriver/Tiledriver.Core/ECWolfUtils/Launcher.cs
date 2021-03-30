// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 


using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
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
        /// Create a WAD with the given 
        /// </summary>
        /// <param name="uwmfMap">The map to save</param>
        /// <param name="wadFilePath">
        /// (Optional) The path to the WAD to create. If not specified, something will be created in the temporary directory.
        /// </param>
        public void LoadMapInEcWolf(
            MapData uwmfMap, 
            [CanBeNull] string wadFilePath = null)
        {
            wadFilePath ??= Path.Combine(Path.GetTempPath(), "demo.wad");

            var pathToLoad = Path.GetFullPath(wadFilePath);

            var wad = new WadFile();
            wad.Append(new Marker("MAP01"));
            wad.Append(new UwmfLump("TEXTMAP", uwmfMap));
            wad.Append(new Marker("ENDMAP"));
            wad.SaveTo(wadFilePath);

            Process.Start(
                _ecWolfExePath,
                $"--file \"{pathToLoad}\" --data wl6 --hard --nowait --tedlevel map01");
        }
    }
}