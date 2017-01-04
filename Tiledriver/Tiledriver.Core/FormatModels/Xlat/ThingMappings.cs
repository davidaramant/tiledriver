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
            PositionlessTriggers.Merge(newMappings.PositionlessTriggers);
            ThingDefinitions.Merge(newMappings.ThingDefinitions);
        }
    }
}