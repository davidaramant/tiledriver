// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.FormatModels.Text.Uwmf
{
    /// <summary>
    /// A UWMF map.
    /// </summary>
    public partial class Map
    {
        partial void AdditionalSemanticChecks()
        {
            if (Planes.Count != PlaneMaps.Count)
            {
                throw new InvalidUwmfException("Unequal number of planes and plane maps");
            }

            var expectedEntryCount = Width * Height;

            foreach (var planeMap in PlaneMaps)
            {
                var actualEntryCount = planeMap.TileSpaces.Count;

                if (actualEntryCount != expectedEntryCount)
                {
                    throw new InvalidUwmfException(
                        $"Invalid number of tile spaces. Expected {expectedEntryCount} but got {actualEntryCount}.");
                }

                CheckCollection(planeMap.TileSpaces, entry => entry.Tile, Tiles.Count, "Tiles");
                CheckCollection(planeMap.TileSpaces, entry => entry.Sector, Sectors.Count, "Sectors");
                CheckCollection(planeMap.TileSpaces, entry => entry.Zone, Zones.Count, "Zones");
            }
        }

        private static void CheckCollection(
            IEnumerable<TileSpace> entries,
            Func<TileSpace, int> idGrabber,
            int definedCount,
            string name)
        {
            // TODO: Add something about which plane map this was found in
            if (entries.Select(idGrabber).Any(id => id >= definedCount))
            {
                throw new InvalidUwmfException($"Invalid ids found for {name}.");
            }
        }
    }    
}
