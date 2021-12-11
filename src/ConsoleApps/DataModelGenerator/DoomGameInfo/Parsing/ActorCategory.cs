// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.
using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.DataModelGenerator.DoomGameInfo.Parsing;

sealed record ActorCategory(
    string Name,
    IReadOnlyDictionary<string, object> GlobalAssignments,
    IReadOnlyList<ActorDefinition> Actors)
{
    public ActorCategory(
        string name, 
        IEnumerable<Assignment> assignments, 
        IEnumerable<ActorDefinition> actors)
    : this(
          name, 
          assignments.ToDictionary(a => a.Name, a => a.Value), 
          actors.ToList())
    {
    }
}