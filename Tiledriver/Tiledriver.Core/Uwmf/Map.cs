/*
** Map.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

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
