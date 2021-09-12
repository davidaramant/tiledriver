// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 


using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.FormatModels.Wad;

namespace Tiledriver.Core.Utils.GZDoom
{
    public sealed class GZDoomLauncher
    {
        private readonly string _gzDoomExePath;

        public GZDoomLauncher(string gzDoomExePath)
        {
            if (!File.Exists(gzDoomExePath))
            {
                throw new FileNotFoundException("Could not find GZDoom EXE: " + gzDoomExePath);
            }
            _gzDoomExePath = gzDoomExePath;
        }

        /// <summary>
        /// Create a WAD with the given map
        /// </summary>
        /// <param name="udmfMap">The map to save</param>
        /// <param name="wadFilePath">
        /// (Optional) The path to the WAD to create. If not specified, something will be created in the temporary directory.
        /// </param>
        public void LoadMap(
            MapData udmfMap,
            string? wadFilePath = null)
        {
            var wad = new List<ILump>
            {
                new Marker("MAP01"),
                new UdmfLump("TEXTMAP", udmfMap),
                new Marker("ENDMAP")
            };

            CreateAndLoadWad(wad, wadFilePath);
        }

        /// <summary>
        /// Create a WAD with the given map
        /// </summary>
        /// <param name="lumps">The lumps of the WAD to load</param>
        /// <param name="wadFilePath">
        /// (Optional) The path to the WAD to create. If not specified, something will be created in the temporary directory.
        /// </param>
        public void CreateAndLoadWad(
            IEnumerable<ILump> lumps,
            string? wadFilePath = null)
        {
            wadFilePath ??= Path.Combine(Path.GetTempPath(), "demo.lumps");
            var pathToLoad = Path.GetFullPath(wadFilePath);

            WadWriter.SaveTo(lumps,pathToLoad);

            LoadWad(pathToLoad);
        }

        public void LoadWad(string wadPath)
        {
            Process.Start(
                _gzDoomExePath,
                $"-iwad doom2.wad -file \"{wadPath}\" -skill 4 -warp 01");
        }
    }
}