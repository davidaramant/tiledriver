using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.MapRanker
{
    public class HasExit : IRule
    {
        public bool Passes(MapData data)
        {
            var exitTypes = new List<string>( new string[]{ "Exit_Normal", "Exit_Secret", "Exit_VictorySpin", "Exit_Victory" });
            return data.Triggers.Any(trigger => exitTypes.Contains(trigger.Action));
        }
    }
}
