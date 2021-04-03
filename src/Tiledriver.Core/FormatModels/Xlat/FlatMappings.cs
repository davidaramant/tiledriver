// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class FlatMappings
    {
        public void Add(FlatMappings newMappings)
        {
            var newCeiling = newMappings.Ceilings.Concat(Ceilings.Skip(newMappings.Ceilings.Count)).ToArray();
            var newFloor = newMappings.Floors.Concat(Floors.Skip(newMappings.Floors.Count)).ToArray();

            Ceilings.Clear();
            Ceilings.AddRange(newCeiling);

            Floors.Clear();
            Floors.AddRange(newFloor);
        }
    }
}