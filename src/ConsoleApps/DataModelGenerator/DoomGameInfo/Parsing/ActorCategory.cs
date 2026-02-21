namespace Tiledriver.DataModelGenerator.DoomGameInfo.Parsing;

sealed record ActorCategory(
	string Name,
	IReadOnlyDictionary<string, object> GlobalAssignments,
	IReadOnlyList<ActorDefinition> Actors
)
{
	public ActorCategory(string name, IEnumerable<Assignment> assignments, IEnumerable<ActorDefinition> actors)
		: this(name, assignments.ToDictionary(a => a.Name, a => a.Value), actors.ToList()) { }
}
