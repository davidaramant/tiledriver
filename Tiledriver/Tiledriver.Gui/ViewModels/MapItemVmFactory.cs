// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;
using Tiledriver.Gui.Utilities;

namespace Tiledriver.Gui.ViewModels
{
    public sealed class MapItemVmFactory
    {
        private readonly MapData _mapData;

        public MapItemVmFactory(MapData mapData)
        {
            _mapData = mapData;
        }

        public ThingVm BuildThing(Thing thing)
        {
            var category = Actor.GetAll().SingleOrDefault(a => a.ClassName == thing.Type)?.Category;
            return new ThingVm(thing, category);
        }

        public SquareVm BuildSquare(int x, int y)
        {
            var tileSpace = _mapData.TileSpaceAt(x, y);
            var tile = _mapData.TileAt(tileSpace.Tile);
            var sector = _mapData.SectorAt(tileSpace.Sector);
            return new SquareVm(x, y, tile, sector, tileSpace.Zone);
        }

        public TriggerVm BuildTrigger(Trigger trigger)
        {
            return new TriggerVm(trigger);
        }
    }
}
