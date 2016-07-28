using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Tiledriver.Core.Uwmf;
using Tiledriver.Core.Wolf3D;
using Tiledriver.UwmfViewer.Utilities;
using Tiledriver.UwmfViewer.ViewModels;

namespace Tiledriver.UwmfViewer.ViewModels
{
    public class MapItemVmFactory
    {
        private Map map;
        private IEnumerable<Actor> actors = Actor.GetAll().Where(a => a.Wolf3D);

        public MapItemVmFactory(Map map)
        {
            this.map = map;
        }

        public ThingVm BuildThing(Thing thing)
        {
            var category = actors.SingleOrDefault(a => a.ClassName == thing.Type)?.Category;
            return new ThingVm(thing, category);
        }

        public SquareVm BuildSquare(int x, int y)
        {
            var tileSpace = map.TileSpaceAt(x, y);
            var tile = map.TileAt(tileSpace.Tile);
            var sector = map.SectorAt(tileSpace.Sector);
            return new SquareVm(x, y, tile, sector, tileSpace.Zone);
        }
    }
}
