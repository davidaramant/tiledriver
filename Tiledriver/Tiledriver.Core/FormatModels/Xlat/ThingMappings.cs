// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.Extensions;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class ThingMappings
    {
        public void Add(ThingMappings newMappings)
        {
            Elevator.AddRange(newMappings.Elevator);

            foreach (var triggerKvp in newMappings.PositionlessTriggers)
            {
                PositionlessTriggers[triggerKvp.Key] = triggerKvp.Value;
            }

            foreach (var thingDefKvp in newMappings.ThingDefinitions)
            {
                ThingDefinitions[thingDefKvp.Key] = thingDefKvp.Value;
            }
        }
    }
}