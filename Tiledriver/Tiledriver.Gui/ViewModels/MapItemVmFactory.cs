// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;
using Tiledriver.Gui.Utilities;

namespace Tiledriver.Gui.ViewModels
{
    public class MapItemVmFactory
    {
        private Map _map;
        private IEnumerable<Actor> _actors = Actor.GetAll().Where(a => a.Wolf3D);

        public MapItemVmFactory(Map map)
        {
            this._map = map;
        }

        public ThingVm BuildThing(Thing thing)
        {
            var category = _actors.SingleOrDefault(a => a.ClassName == thing.Type)?.Category;
            return new ThingVm(thing, category);
        }

        public SquareVm BuildSquare(int x, int y)
        {
            var tileSpace = _map.TileSpaceAt(x, y);
            var tile = _map.TileAt(tileSpace.Tile);
            var sector = _map.SectorAt(tileSpace.Sector);
            return new SquareVm(x, y, tile, sector, tileSpace.Zone);
        }
    }
}
