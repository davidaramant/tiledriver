// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.DataModelGenerator.DoomGameInfo.Parsing;

sealed record ActorDefinition(int Id, IReadOnlyDictionary<string, object> Assignments)
{
	public ActorDefinition(int Id, IEnumerable<Assignment> assignments)
		: this(Id, assignments.ToDictionary(a => a.Name, a => a.Value)) { }
}
