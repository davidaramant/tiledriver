using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.Uwmf;
using Tiledriver.Core.Wolf3D;
using Tiledriver.UwmfViewer.Utilities;

namespace Tiledriver.UwmfViewer.Views
{
    public class MapItemFactory
    {
        private Map map;
        private IEnumerable<Actor> actors = Actor.GetAll().Where(a => a.Wolf3D);

        public MapItemFactory(Map map)
        {
            this.map = map;
        }

        public ThingVm VmForThing(Thing thing)
        {
            var category = actors.SingleOrDefault(a => a.ClassName == thing.Type)?.Category;
            return ThingVm.Create(thing, category);
        }

        public Square VmForCoordinates(int x, int y)
        {
            var tileSpace = map.TileSpaceAt(x, y);
            return new Square(x, y)
            {
                Tile = map.TileAt(tileSpace.Tile),
                Sector = map.SectorAt(tileSpace.Sector),
                Zone = tileSpace.Zone
            };
        }
    }
}
