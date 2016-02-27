using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;

namespace Tiledriver.Generator
{
    public interface IRegion
    {
        Rectangle BoundingBox { get; }

        IEnumerable<Thing> GetThings();

        IEnumerable<Trigger> GetTriggers();

        PlanemapEntry GetForPosition(int row, int col);
    }
}
