// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;

namespace Tiledriver.Core.FormatModels.Xlat
{
    public sealed partial class FlatMappings
    {
        public void Add(FlatMappings newMappings)
        {
            var newCeiling = newMappings.Ceiling.Concat(Ceiling.Skip(newMappings.Ceiling.Count)).ToArray();
            var newFloor = newMappings.Floor.Concat(Floor.Skip(newMappings.Floor.Count)).ToArray();

            Ceiling.Clear();
            Ceiling.AddRange(newCeiling);

            Floor.Clear();
            Floor.AddRange(newFloor);
        }
    }
}