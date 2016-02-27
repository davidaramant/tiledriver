using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;
using Tiledriver.Wolf3D;

namespace Tiledriver.Generator
{
    public interface IRegion
    {
        Rectangle BoundingBox { get; }

        IEnumerable<Thing> GetThings();

        IEnumerable<Trigger> GetTriggers();

        MapTile GetTileAtPosition(int mapRow, int mapCol);
    }
}
