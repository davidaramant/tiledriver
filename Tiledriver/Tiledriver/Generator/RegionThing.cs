using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Wolf3D;

namespace Tiledriver.Generator
{
    public sealed class RegionThing
    {
        public Point LocationOffset { get; }
        public WolfActor Actor { get; }
        public Direction Facing { get; }

        public RegionThing(
            Point locationOffset,
            WolfActor actor,
            Direction facing)
        {
            LocationOffset = locationOffset;
            Actor = actor;
            Facing = facing;
        }
    }
}
