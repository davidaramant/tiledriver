﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
            var thingVm = ThingVm.Create(thing, category);

            thingVm.LayerType = LayerType.Thing;
                    thingVm.Coordinates = new Point(Math.Floor(thing.X), Math.Floor(thing.Y));
            return thingVm;
        }

        public Square VmForCoordinates(int x, int y)
        {
            var tileSpace = map.TileSpaceAt(x, y);
            return new Square(x, y)
            {
                Tile = map.TileAt(tileSpace.Tile),
                Sector = map.SectorAt(tileSpace.Sector),
                Zone = tileSpace.Zone,
                LayerType = LayerType.Tile,
                Coordinates = new Point(x, y),
            };
        }
    }
}