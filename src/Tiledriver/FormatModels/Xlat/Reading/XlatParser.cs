using System.Collections.Immutable;
using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.Xlat.Reading;

public static partial class XlatParser
{
	public static MapTranslation Parse(IEnumerable<Token> tokens, IResourceProvider resourceProvider)
	{
		List<TileMappings> tileMappings = [];
		List<IMapping> thingMappings = [];
		List<FlatMappings> flatMappings = [];

		var tokenSource = new TokenSource(tokens, resourceProvider, XlatLexer.Create);
		using var tokenStream = tokenSource.GetEnumerator();

		while (tokenStream.MoveNext())
		{
			var id = tokenStream.Current as IdentifierToken;
			if (id == null)
			{
				throw new ParsingException($"Unexpected token: {tokenStream.Current}");
			}

			if (id.Id.EqualsIgnoreCase("enable") || id.Id.EqualsIgnoreCase("disable"))
			{
				// global flag, ignore
				tokenStream.ExpectNext<IdentifierToken>();
				tokenStream.ExpectNext<SemicolonToken>();
			}
			else if (id.Id.EqualsIgnoreCase("music"))
			{
				throw new ParsingException("This should be ignored");
			}
			else if (id.Id.EqualsIgnoreCase("tiles"))
			{
				tileMappings.Add(ParseTileMappings(tokenStream));
			}
			else if (id.Id.EqualsIgnoreCase("things"))
			{
				thingMappings.AddRange(ParseThingMappings(tokenStream));
			}
			else if (id.Id.EqualsIgnoreCase("flats"))
			{
				flatMappings.Add(ParseFlatMappings(tokenStream));
			}
			else
			{
				throw new ParsingException($"Unexpected identifier: {id}");
			}
		}

		return new MapTranslation(Merge(tileMappings), thingMappings, flatMappings.LastOrDefault());
	}

	private static TileMappings Merge(IEnumerable<TileMappings> tileMappings)
	{
		var ambushModzones = new List<AmbushModzone>();
		var changeTriggerModzones = new List<ChangeTriggerModzone>();
		var tileTemplates = new List<TileTemplate>();
		var triggerTemplates = new List<TriggerTemplate>();
		var zoneTemplates = new List<ZoneTemplate>();

		foreach (var mapping in tileMappings)
		{
			ambushModzones.AddRange(mapping.AmbushModzones);
			changeTriggerModzones.AddRange(mapping.ChangeTriggerModzones);
			tileTemplates.AddRange(mapping.TileTemplates);
			triggerTemplates.AddRange(mapping.TriggerTemplates);
			zoneTemplates.AddRange(mapping.ZoneTemplates);
		}

		return new TileMappings(
			[.. ambushModzones],
			[.. changeTriggerModzones],
			[.. tileTemplates],
			[.. triggerTemplates],
			[.. zoneTemplates]
		);
	}

	private static TileMappings ParseTileMappings(IEnumerator<Token> tokenStream)
	{
		tokenStream.ExpectNext<OpenBraceToken>();

		var ambushModzones = new List<AmbushModzone>();
		var changeTriggerModzones = new List<ChangeTriggerModzone>();
		var tileTemplates = new List<TileTemplate>();
		var triggerTemplates = new List<TriggerTemplate>();
		var zoneTemplates = new List<ZoneTemplate>();

		while (true)
		{
			var token = tokenStream.GetNext();
			switch (token)
			{
				case IdentifierToken id:
					if (id.Id.EqualsIgnoreCase("modzone"))
					{
						ParseModzone(tokenStream, id, ambushModzones, changeTriggerModzones);
					}
					else if (id.Id.EqualsIgnoreCase("tile"))
					{
						tileTemplates.Add(ParseTileTemplate(tokenStream, id));
					}
					else if (id.Id.EqualsIgnoreCase("trigger"))
					{
						triggerTemplates.Add(ParseTriggerTemplate(tokenStream, id));
					}
					else if (id.Id.EqualsIgnoreCase("zone"))
					{
						zoneTemplates.Add(ParseZone(tokenStream, id));
					}
					else
					{
						throw ParsingException.CreateError(id, "unknown identifier");
					}
					break;

				case CloseBraceToken:
					return new TileMappings(
						[.. ambushModzones],
						[.. changeTriggerModzones],
						[.. tileTemplates],
						[.. triggerTemplates],
						[.. zoneTemplates]
					);

				default:
					throw ParsingException.CreateError(token, "identifier or end of block");
			}
		}
	}

	private static void ParseModzone(
		IEnumerator<Token> tokenStream,
		IdentifierToken id,
		List<AmbushModzone> ambushModzones,
		List<ChangeTriggerModzone> changeTriggerModzones
	)
	{
		var oldNum = tokenStream
			.ExpectNext<IntegerToken>()
			.ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

		bool fillZone = false;

		var next = tokenStream.ExpectNext<IdentifierToken>();

		if (next.Id.EqualsIgnoreCase("fillzone"))
		{
			fillZone = true;
			next = tokenStream.ExpectNext<IdentifierToken>();
		}

		if (next.Id.EqualsIgnoreCase("ambush"))
		{
			tokenStream.ExpectNext<SemicolonToken>();

			ambushModzones.Add(new AmbushModzone(oldNum, fillZone));
		}
		else if (next.Id.EqualsIgnoreCase("changetrigger"))
		{
			var action = tokenStream.ExpectNext<StringToken>().Value;

			tokenStream.ExpectNext<OpenBraceToken>();

			var block = tokenStream.ParseBlock(id);

			var triggerTemplate = ReadTriggerTemplate(oldNum, block);

			changeTriggerModzones.Add(new ChangeTriggerModzone(oldNum, action, triggerTemplate, fillZone));
		}
		else
		{
			throw ParsingException.CreateError(next, "ambush or changetrigger");
		}
	}

	private static TileTemplate ParseTileTemplate(IEnumerator<Token> tokenStream, IdentifierToken id)
	{
		var oldNum = tokenStream
			.ExpectNext<IntegerToken>()
			.ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

		tokenStream.ExpectNext<OpenBraceToken>();

		var block = tokenStream.ParseBlock(id);

		return ReadTileTemplate(oldNum, block);
	}

	private static TriggerTemplate ParseTriggerTemplate(IEnumerator<Token> tokenStream, IdentifierToken id)
	{
		var oldNum = tokenStream
			.ExpectNext<IntegerToken>()
			.ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

		tokenStream.ExpectNext<OpenBraceToken>();

		var block = tokenStream.ParseBlock(id);

		return ReadTriggerTemplate(oldNum, block);
	}

	private static ZoneTemplate ParseZone(IEnumerator<Token> tokenStream, IdentifierToken id)
	{
		var oldNum = tokenStream
			.ExpectNext<IntegerToken>()
			.ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

		tokenStream.ExpectNext<OpenBraceToken>();

		var block = tokenStream.ParseBlock(id);
		var fields = block.GetFieldAssignments();

		return new ZoneTemplate(oldNum, Comment: fields.GetOptionalFieldValue("comment", ""));
	}

	private static FlatMappings ParseFlatMappings(IEnumerator<Token> tokenStream)
	{
		var ceilings = new List<string>();
		var floors = new List<string>();

		tokenStream.ExpectNext<OpenBraceToken>();

		while (true)
		{
			var token = tokenStream.GetNext();
			switch (token)
			{
				case IdentifierToken id:
					if (id.Id.EqualsIgnoreCase("ceiling"))
					{
						ceilings.AddRange(ParseStringList(tokenStream));
					}
					else if (id.Id.EqualsIgnoreCase("floor"))
					{
						floors.AddRange(ParseStringList(tokenStream));
					}
					else
					{
						throw ParsingException.CreateError(id, "unknown identifier");
					}
					break;

				case CloseBraceToken:
					return new FlatMappings([.. ceilings], [.. floors]);

				default:
					throw ParsingException.CreateError(token, "identifier or end of block");
			}
		}
	}

	private static List<string> ParseStringList(IEnumerator<Token> tokenStream)
	{
		var strings = new List<string>();

		tokenStream.ExpectNext<OpenBraceToken>();

		while (true)
		{
			var token = tokenStream.GetNext();
			switch (token)
			{
				case CommaToken:
					break;

				case StringToken s:
					strings.Add(s.Value);
					break;

				case CloseBraceToken:
					return strings;

				default:
					throw ParsingException.CreateError(token, "identifier or end of block");
			}
		}
	}

	private static IEnumerable<IMapping> ParseThingMappings(IEnumerator<Token> tokenStream)
	{
		tokenStream.ExpectNext<OpenBraceToken>();

		var thingMappings = new List<IMapping>();

		while (true)
		{
			var token = tokenStream.GetNext();
			switch (token)
			{
				case IdentifierToken id:
					if (id.Id.EqualsIgnoreCase("elevator"))
					{
						thingMappings.Add(ParseElevator(tokenStream, id));
					}
					else if (id.Id.EqualsIgnoreCase("trigger"))
					{
						thingMappings.Add(ParseTriggerTemplate(tokenStream, id));
					}
					else
					{
						throw ParsingException.CreateError(id, "unknown identifier");
					}
					break;

				case OpenBraceToken:
					thingMappings.Add(ParseThingTemplate(tokenStream));
					break;

				case CloseBraceToken:
					return thingMappings;

				default:
					throw ParsingException.CreateError(token, "identifier or end of block");
			}
		}
	}

	private static Elevator ParseElevator(IEnumerator<Token> tokenStream, IdentifierToken id)
	{
		var oldNum = tokenStream
			.ExpectNext<IntegerToken>()
			.ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));
		tokenStream.ExpectNext<SemicolonToken>();

		return new Elevator(oldNum);
	}

	private static ThingTemplate ParseThingTemplate(IEnumerator<Token> tokenStream)
	{
		var oldNum = tokenStream
			.ExpectNext<IntegerToken>()
			.ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

		tokenStream.ExpectNext<CommaToken>();

		var actor = tokenStream.ExpectNext<IdentifierToken>().Id.ToString();

		tokenStream.ExpectNext<CommaToken>();

		var angles = tokenStream.ExpectNext<IntegerToken>().Value;

		tokenStream.ExpectNext<CommaToken>();

		var flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		var next = tokenStream.GetNext();

		if (next is IntegerToken i)
		{
			if (i.Value != 0)
			{
				throw ParsingException.CreateError(i, "Expected 0 value for flags");
			}

			tokenStream.ExpectNext<CommaToken>();
		}
		else if (next is IdentifierToken flagToken)
		{
			flags.Add(flagToken.Id.ToString());

			while (true)
			{
				next = tokenStream.GetNext();

				if (next is CommaToken)
				{
					break;
				}
				else if (next is PipeToken)
				{
					flags.Add(tokenStream.ExpectNext<IdentifierToken>().Id.ToString());
				}
				else
				{
					throw ParsingException.CreateError(next, "Comma or pipe");
				}
			}
		}
		else
		{
			throw ParsingException.CreateError(next, "Expected number or flags");
		}

		var minSkill = tokenStream.ExpectNext<IntegerToken>().Value;
		tokenStream.ExpectNext<CloseBraceToken>();

		return new ThingTemplate(
			oldNum,
			actor,
			angles,
			Holowall: flags.Contains("holowall"),
			Pathing: flags.Contains("pathing"),
			Ambush: flags.Contains("ambush"),
			Minskill: minSkill
		);
	}
}
