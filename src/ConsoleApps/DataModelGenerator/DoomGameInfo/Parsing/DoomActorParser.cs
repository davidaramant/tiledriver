using Pidgin;
using Pidgin.Comment;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace Tiledriver.DataModelGenerator.DoomGameInfo.Parsing;

static class DoomActorParser
{
	static readonly Parser<char, Unit> Separator = Whitespace
		.SkipAtLeastOnce()
		.Or(CommentParser.SkipLineComment(Parser.String("//")))
		.SkipMany();

	static readonly Parser<char, char> LBrace = Char('{');
	static readonly Parser<char, char> RBrace = Char('}');
	static readonly Parser<char, char> Quote = Char('"');
	static readonly Parser<char, char> Equal = Char('=');
	static readonly Parser<char, char> Semicolon = Char(';');

	private static readonly Parser<char, string> String = Token(c => c != '"').ManyString().Between(Quote);
	private static readonly Parser<char, int> Integer = DecimalNum;

	private static readonly Parser<char, object> Value = String.Cast<object>().Or(Integer.Cast<object>());

	static readonly Parser<char, string> Identifier = Token(char.IsLower).ManyString();

	static readonly Parser<char, Assignment> Assignment = Map(
		(id, _, value, _) => new Assignment(id, value),
		Identifier.Before(Separator),
		Equal.Before(Separator),
		Value.Before(Separator),
		Semicolon.Before(Separator)
	);

	static readonly Parser<char, ActorDefinition> ActorDefinition =
		from id in Integer.Before(Separator)
		from open in LBrace.Before(Separator)
		from assignments in Assignment.Many()
		from close in RBrace.Before(Separator)
		select new ActorDefinition(id, assignments);

	static readonly Parser<char, ActorCategory> Category =
		from name in Identifier.Before(Separator)
		from open in LBrace.Before(Separator)
		from assignments in Assignment.Many()
		from actors in ActorDefinition.Many()
		from close in RBrace.Before(Separator)
		select new ActorCategory(name, assignments, actors);

	static readonly Parser<char, IEnumerable<ActorCategory>> Categories = Separator.Then(Category.Many());

	public static IEnumerable<ActorCategory> ParseCategories(StreamReader reader)
	{
		var result = Categories.Parse(reader);

		if (!result.Success)
		{
			throw new Exception("Parsing failed!");
		}

		return result.Value;
	}
}
