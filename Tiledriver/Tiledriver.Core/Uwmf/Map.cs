// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.Uwmf
{
    /// <summary>
    /// A UWMF map.
    /// </summary>
    public partial class Map
    {
        partial void AdditionalSemanticChecks()
        {
            // TODO: Expand to check all planemaps
            //var expectedEntryCount = Width * Height;
            //var actualEntryCount = Planemaps.First().Entries.Count;

            //if (actualEntryCount != expectedEntryCount)
            //{
            //    throw new InvalidUwmfException($"Invalid number of planemap entries. Expected {expectedEntryCount} but got {actualEntryCount}.");
            //}

            //CheckCollection(Planemaps.First().Entries, entry => (int)entry.Tile, Tiles.Count, "Tiles");
            //CheckCollection(Planemaps.First().Entries, entry => (int)entry.Sector, Sectors.Count, "Sectors");
            //CheckCollection(Planemaps.First().Entries, entry => (int)entry.Zone, Zones.Count, "Zones");
        }

        private static void CheckCollection(
            IEnumerable<TileSpace> entries,
            Func<TileSpace, int> idGrabber,
            int definedCount,
            string name)
        {
            if (entries.Select(idGrabber).Any(id => id >= definedCount))
            {
                throw new InvalidUwmfException($"Invalid ids found for {name}.");
            }
        }
    }    
}
