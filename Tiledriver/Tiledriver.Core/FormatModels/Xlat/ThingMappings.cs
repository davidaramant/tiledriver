// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.Extensions;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class ThingMappings
    {
        public void Add(ThingMappings newMappings)
        {
            Elevators.AddRange(newMappings.Elevators);
            TriggerTemplates.Merge(newMappings.TriggerTemplates);
            ThingTemplates.Merge(newMappings.ThingTemplates);
        }
    }
}