using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Udmf;

#nullable enable
namespace Tiledriver.FormatModels.Udmf.Reading;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial class UdmfParser
{
	[global::System.Flags]
	private enum ThingFields : uint
	{
		None = 0,
		Id = 1U << 0,
		X = 1U << 1,
		Y = 1U << 2,
		Height = 1U << 3,
		Angle = 1U << 4,
		Type = 1U << 5,
		Skill1 = 1U << 6,
		Skill2 = 1U << 7,
		Skill3 = 1U << 8,
		Skill4 = 1U << 9,
		Skill5 = 1U << 10,
		Single = 1U << 11,
		Coop = 1U << 12,
		Dm = 1U << 13,
		Ambush = 1U << 14,
		Comment = 1U << 15,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(ThingFields value, ThingFields flag) => (value & flag) == flag;

	private Thing ParseThingBlock(Identifier blockName)
	{
		int id = 0;
		double x = default;
		double y = default;
		double height = 0;
		int angle = default;
		int type = default;
		bool skill1 = false;
		bool skill2 = false;
		bool skill3 = false;
		bool skill4 = false;
		bool skill5 = false;
		bool single = false;
		bool coop = false;
		bool dm = false;
		bool ambush = false;
		string comment = "";
		ThingFields seenFields = ThingFields.None;
		HashSet<Identifier>? unknownFields = null;

		while (!_lexer.TryExpectCloseBrace())
		{
			Identifier identifier = _lexer.ReadIdentifier();
			string identifierText = (string)identifier;
			_lexer.ExpectEquals();
			bool handledKnownField = false;
			switch (identifierText.Length)
			{
				case 1:
					if (identifier.EqualsIgnoreCase("x"))
					{
						if (HasFlag(seenFields, ThingFields.X))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.X;
						x = _lexer.ReadDouble();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("y"))
					{
						if (HasFlag(seenFields, ThingFields.Y))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Y;
						y = _lexer.ReadDouble();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 2:
					if (identifier.EqualsIgnoreCase("id"))
					{
						if (HasFlag(seenFields, ThingFields.Id))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Id;
						id = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("dm"))
					{
						if (HasFlag(seenFields, ThingFields.Dm))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Dm;
						dm = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 4:
					if (identifier.EqualsIgnoreCase("type"))
					{
						if (HasFlag(seenFields, ThingFields.Type))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Type;
						type = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("coop"))
					{
						if (HasFlag(seenFields, ThingFields.Coop))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Coop;
						coop = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 5:
					if (identifier.EqualsIgnoreCase("angle"))
					{
						if (HasFlag(seenFields, ThingFields.Angle))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Angle;
						angle = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 6:
					if (identifier.EqualsIgnoreCase("height"))
					{
						if (HasFlag(seenFields, ThingFields.Height))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Height;
						height = _lexer.ReadDouble();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("skill1"))
					{
						if (HasFlag(seenFields, ThingFields.Skill1))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Skill1;
						skill1 = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("skill2"))
					{
						if (HasFlag(seenFields, ThingFields.Skill2))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Skill2;
						skill2 = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("skill3"))
					{
						if (HasFlag(seenFields, ThingFields.Skill3))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Skill3;
						skill3 = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("skill4"))
					{
						if (HasFlag(seenFields, ThingFields.Skill4))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Skill4;
						skill4 = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("skill5"))
					{
						if (HasFlag(seenFields, ThingFields.Skill5))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Skill5;
						skill5 = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("single"))
					{
						if (HasFlag(seenFields, ThingFields.Single))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Single;
						single = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("ambush"))
					{
						if (HasFlag(seenFields, ThingFields.Ambush))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Ambush;
						ambush = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 7:
					if (identifier.EqualsIgnoreCase("comment"))
					{
						if (HasFlag(seenFields, ThingFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Comment;
						comment = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				default:
					handledKnownField = false;
					break;
			}
			if (!handledKnownField)
			{
				unknownFields ??= [];
				if (!unknownFields.Add(identifier))
				{
					throw DuplicateField(identifier);
				}
				_lexer.SkipValueAndSemicolon();
			}
		}

		if (!HasFlag(seenFields, ThingFields.X)) throw MissingRequiredField(blockName, "x");
		if (!HasFlag(seenFields, ThingFields.Y)) throw MissingRequiredField(blockName, "y");
		if (!HasFlag(seenFields, ThingFields.Angle)) throw MissingRequiredField(blockName, "angle");
		if (!HasFlag(seenFields, ThingFields.Type)) throw MissingRequiredField(blockName, "type");
		return new Thing(
			X: x,
			Y: y,
			Angle: angle,
			Type: type,
			Id: id,
			Height: height,
			Skill1: skill1,
			Skill2: skill2,
			Skill3: skill3,
			Skill4: skill4,
			Skill5: skill5,
			Single: single,
			Coop: coop,
			Dm: dm,
			Ambush: ambush,
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum VertexFields : uint
	{
		None = 0,
		X = 1U << 0,
		Y = 1U << 1,
		Comment = 1U << 2,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(VertexFields value, VertexFields flag) => (value & flag) == flag;

	private Vertex ParseVertexBlock(Identifier blockName)
	{
		double x = default;
		double y = default;
		string comment = "";
		VertexFields seenFields = VertexFields.None;
		HashSet<Identifier>? unknownFields = null;

		while (!_lexer.TryExpectCloseBrace())
		{
			Identifier identifier = _lexer.ReadIdentifier();
			string identifierText = (string)identifier;
			_lexer.ExpectEquals();
			bool handledKnownField = false;
			switch (identifierText.Length)
			{
				case 1:
					if (identifier.EqualsIgnoreCase("x"))
					{
						if (HasFlag(seenFields, VertexFields.X))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= VertexFields.X;
						x = _lexer.ReadDouble();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("y"))
					{
						if (HasFlag(seenFields, VertexFields.Y))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= VertexFields.Y;
						y = _lexer.ReadDouble();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 7:
					if (identifier.EqualsIgnoreCase("comment"))
					{
						if (HasFlag(seenFields, VertexFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= VertexFields.Comment;
						comment = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				default:
					handledKnownField = false;
					break;
			}
			if (!handledKnownField)
			{
				unknownFields ??= [];
				if (!unknownFields.Add(identifier))
				{
					throw DuplicateField(identifier);
				}
				_lexer.SkipValueAndSemicolon();
			}
		}

		if (!HasFlag(seenFields, VertexFields.X)) throw MissingRequiredField(blockName, "x");
		if (!HasFlag(seenFields, VertexFields.Y)) throw MissingRequiredField(blockName, "y");
		return new Vertex(
			X: x,
			Y: y,
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum LineDefFields : uint
	{
		None = 0,
		Id = 1U << 0,
		V1 = 1U << 1,
		V2 = 1U << 2,
		SideFront = 1U << 3,
		SideBack = 1U << 4,
		Special = 1U << 5,
		Arg0 = 1U << 6,
		Arg1 = 1U << 7,
		Arg2 = 1U << 8,
		Arg3 = 1U << 9,
		Arg4 = 1U << 10,
		TwoSided = 1U << 11,
		DontPegTop = 1U << 12,
		DontPegBottom = 1U << 13,
		BlockMonsters = 1U << 14,
		BlockSound = 1U << 15,
		Secret = 1U << 16,
		MonsterActivate = 1U << 17,
		PlayerUse = 1U << 18,
		Blocking = 1U << 19,
		RepeatSpecial = 1U << 20,
		PlayerCross = 1U << 21,
		DontDraw = 1U << 22,
		Mapped = 1U << 23,
		Comment = 1U << 24,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(LineDefFields value, LineDefFields flag) => (value & flag) == flag;

	private LineDef ParseLineDefBlock(Identifier blockName)
	{
		int id = -1;
		int v1 = default;
		int v2 = default;
		int sideFront = default;
		int sideBack = -1;
		int special = 0;
		int arg0 = 0;
		int arg1 = 0;
		int arg2 = 0;
		int arg3 = 0;
		int arg4 = 0;
		bool twoSided = false;
		bool dontPegTop = false;
		bool dontPegBottom = false;
		bool blockMonsters = false;
		bool blockSound = false;
		bool secret = false;
		bool monsterActivate = false;
		bool playerUse = false;
		bool blocking = false;
		bool repeatSpecial = false;
		bool playerCross = false;
		bool dontDraw = false;
		bool mapped = false;
		string comment = "";
		LineDefFields seenFields = LineDefFields.None;
		HashSet<Identifier>? unknownFields = null;

		while (!_lexer.TryExpectCloseBrace())
		{
			Identifier identifier = _lexer.ReadIdentifier();
			string identifierText = (string)identifier;
			_lexer.ExpectEquals();
			bool handledKnownField = false;
			switch (identifierText.Length)
			{
				case 2:
					if (identifier.EqualsIgnoreCase("id"))
					{
						if (HasFlag(seenFields, LineDefFields.Id))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Id;
						id = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("v1"))
					{
						if (HasFlag(seenFields, LineDefFields.V1))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.V1;
						v1 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("v2"))
					{
						if (HasFlag(seenFields, LineDefFields.V2))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.V2;
						v2 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 4:
					if (identifier.EqualsIgnoreCase("arg0"))
					{
						if (HasFlag(seenFields, LineDefFields.Arg0))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Arg0;
						arg0 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("arg1"))
					{
						if (HasFlag(seenFields, LineDefFields.Arg1))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Arg1;
						arg1 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("arg2"))
					{
						if (HasFlag(seenFields, LineDefFields.Arg2))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Arg2;
						arg2 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("arg3"))
					{
						if (HasFlag(seenFields, LineDefFields.Arg3))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Arg3;
						arg3 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("arg4"))
					{
						if (HasFlag(seenFields, LineDefFields.Arg4))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Arg4;
						arg4 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 6:
					if (identifier.EqualsIgnoreCase("secret"))
					{
						if (HasFlag(seenFields, LineDefFields.Secret))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Secret;
						secret = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("mapped"))
					{
						if (HasFlag(seenFields, LineDefFields.Mapped))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Mapped;
						mapped = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 7:
					if (identifier.EqualsIgnoreCase("special"))
					{
						if (HasFlag(seenFields, LineDefFields.Special))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Special;
						special = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("comment"))
					{
						if (HasFlag(seenFields, LineDefFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Comment;
						comment = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 8:
					if (identifier.EqualsIgnoreCase("sideBack"))
					{
						if (HasFlag(seenFields, LineDefFields.SideBack))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.SideBack;
						sideBack = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("twoSided"))
					{
						if (HasFlag(seenFields, LineDefFields.TwoSided))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.TwoSided;
						twoSided = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("blocking"))
					{
						if (HasFlag(seenFields, LineDefFields.Blocking))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.Blocking;
						blocking = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("dontDraw"))
					{
						if (HasFlag(seenFields, LineDefFields.DontDraw))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.DontDraw;
						dontDraw = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 9:
					if (identifier.EqualsIgnoreCase("sideFront"))
					{
						if (HasFlag(seenFields, LineDefFields.SideFront))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.SideFront;
						sideFront = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("playerUse"))
					{
						if (HasFlag(seenFields, LineDefFields.PlayerUse))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.PlayerUse;
						playerUse = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 10:
					if (identifier.EqualsIgnoreCase("dontPegTop"))
					{
						if (HasFlag(seenFields, LineDefFields.DontPegTop))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.DontPegTop;
						dontPegTop = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("blockSound"))
					{
						if (HasFlag(seenFields, LineDefFields.BlockSound))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.BlockSound;
						blockSound = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 11:
					if (identifier.EqualsIgnoreCase("playerCross"))
					{
						if (HasFlag(seenFields, LineDefFields.PlayerCross))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.PlayerCross;
						playerCross = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 13:
					if (identifier.EqualsIgnoreCase("dontPegBottom"))
					{
						if (HasFlag(seenFields, LineDefFields.DontPegBottom))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.DontPegBottom;
						dontPegBottom = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("blockMonsters"))
					{
						if (HasFlag(seenFields, LineDefFields.BlockMonsters))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.BlockMonsters;
						blockMonsters = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("repeatSpecial"))
					{
						if (HasFlag(seenFields, LineDefFields.RepeatSpecial))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.RepeatSpecial;
						repeatSpecial = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 15:
					if (identifier.EqualsIgnoreCase("monsterActivate"))
					{
						if (HasFlag(seenFields, LineDefFields.MonsterActivate))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= LineDefFields.MonsterActivate;
						monsterActivate = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				default:
					handledKnownField = false;
					break;
			}
			if (!handledKnownField)
			{
				unknownFields ??= [];
				if (!unknownFields.Add(identifier))
				{
					throw DuplicateField(identifier);
				}
				_lexer.SkipValueAndSemicolon();
			}
		}

		if (!HasFlag(seenFields, LineDefFields.V1)) throw MissingRequiredField(blockName, "v1");
		if (!HasFlag(seenFields, LineDefFields.V2)) throw MissingRequiredField(blockName, "v2");
		if (!HasFlag(seenFields, LineDefFields.SideFront)) throw MissingRequiredField(blockName, "sideFront");
		return new LineDef(
			V1: v1,
			V2: v2,
			SideFront: sideFront,
			Id: id,
			SideBack: sideBack,
			Special: special,
			Arg0: arg0,
			Arg1: arg1,
			Arg2: arg2,
			Arg3: arg3,
			Arg4: arg4,
			TwoSided: twoSided,
			DontPegTop: dontPegTop,
			DontPegBottom: dontPegBottom,
			BlockMonsters: blockMonsters,
			BlockSound: blockSound,
			Secret: secret,
			MonsterActivate: monsterActivate,
			PlayerUse: playerUse,
			Blocking: blocking,
			RepeatSpecial: repeatSpecial,
			PlayerCross: playerCross,
			DontDraw: dontDraw,
			Mapped: mapped,
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum SideDefFields : uint
	{
		None = 0,
		Sector = 1U << 0,
		OffsetX = 1U << 1,
		OffsetY = 1U << 2,
		TextureTop = 1U << 3,
		TextureBottom = 1U << 4,
		TextureMiddle = 1U << 5,
		Comment = 1U << 6,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(SideDefFields value, SideDefFields flag) => (value & flag) == flag;

	private SideDef ParseSideDefBlock(Identifier blockName)
	{
		int sector = default;
		int offsetX = 0;
		int offsetY = 0;
		Texture textureTop = Texture.None;
		Texture textureBottom = Texture.None;
		Texture textureMiddle = Texture.None;
		string comment = "";
		SideDefFields seenFields = SideDefFields.None;
		HashSet<Identifier>? unknownFields = null;

		while (!_lexer.TryExpectCloseBrace())
		{
			Identifier identifier = _lexer.ReadIdentifier();
			string identifierText = (string)identifier;
			_lexer.ExpectEquals();
			bool handledKnownField = false;
			switch (identifierText.Length)
			{
				case 6:
					if (identifier.EqualsIgnoreCase("sector"))
					{
						if (HasFlag(seenFields, SideDefFields.Sector))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SideDefFields.Sector;
						sector = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 7:
					if (identifier.EqualsIgnoreCase("offsetX"))
					{
						if (HasFlag(seenFields, SideDefFields.OffsetX))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SideDefFields.OffsetX;
						offsetX = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("offsetY"))
					{
						if (HasFlag(seenFields, SideDefFields.OffsetY))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SideDefFields.OffsetY;
						offsetY = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("comment"))
					{
						if (HasFlag(seenFields, SideDefFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SideDefFields.Comment;
						comment = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 10:
					if (identifier.EqualsIgnoreCase("textureTop"))
					{
						if (HasFlag(seenFields, SideDefFields.TextureTop))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SideDefFields.TextureTop;
						textureTop = ParseTextureFieldValue(optional: true);
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 13:
					if (identifier.EqualsIgnoreCase("textureBottom"))
					{
						if (HasFlag(seenFields, SideDefFields.TextureBottom))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SideDefFields.TextureBottom;
						textureBottom = ParseTextureFieldValue(optional: true);
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("textureMiddle"))
					{
						if (HasFlag(seenFields, SideDefFields.TextureMiddle))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SideDefFields.TextureMiddle;
						textureMiddle = ParseTextureFieldValue(optional: true);
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				default:
					handledKnownField = false;
					break;
			}
			if (!handledKnownField)
			{
				unknownFields ??= [];
				if (!unknownFields.Add(identifier))
				{
					throw DuplicateField(identifier);
				}
				_lexer.SkipValueAndSemicolon();
			}
		}

		if (!HasFlag(seenFields, SideDefFields.Sector)) throw MissingRequiredField(blockName, "sector");
		return new SideDef(
			Sector: sector,
			TextureTop: textureTop,
			TextureBottom: textureBottom,
			TextureMiddle: textureMiddle,
			OffsetX: offsetX,
			OffsetY: offsetY,
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum SectorFields : uint
	{
		None = 0,
		HeightFloor = 1U << 0,
		HeightCeiling = 1U << 1,
		TextureFloor = 1U << 2,
		TextureCeiling = 1U << 3,
		LightLevel = 1U << 4,
		Special = 1U << 5,
		Id = 1U << 6,
		DropActors = 1U << 7,
		Comment = 1U << 8,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(SectorFields value, SectorFields flag) => (value & flag) == flag;

	private Sector ParseSectorBlock(Identifier blockName)
	{
		int heightFloor = default;
		int heightCeiling = default;
		Texture textureFloor = default!;
		Texture textureCeiling = default!;
		int lightLevel = default;
		int special = 0;
		int id = 0;
		bool dropActors = false;
		string comment = "";
		SectorFields seenFields = SectorFields.None;
		HashSet<Identifier>? unknownFields = null;

		while (!_lexer.TryExpectCloseBrace())
		{
			Identifier identifier = _lexer.ReadIdentifier();
			string identifierText = (string)identifier;
			_lexer.ExpectEquals();
			bool handledKnownField = false;
			switch (identifierText.Length)
			{
				case 2:
					if (identifier.EqualsIgnoreCase("id"))
					{
						if (HasFlag(seenFields, SectorFields.Id))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.Id;
						id = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 7:
					if (identifier.EqualsIgnoreCase("special"))
					{
						if (HasFlag(seenFields, SectorFields.Special))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.Special;
						special = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("comment"))
					{
						if (HasFlag(seenFields, SectorFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.Comment;
						comment = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 10:
					if (identifier.EqualsIgnoreCase("lightLevel"))
					{
						if (HasFlag(seenFields, SectorFields.LightLevel))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.LightLevel;
						lightLevel = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("dropActors"))
					{
						if (HasFlag(seenFields, SectorFields.DropActors))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.DropActors;
						dropActors = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 11:
					if (identifier.EqualsIgnoreCase("heightFloor"))
					{
						if (HasFlag(seenFields, SectorFields.HeightFloor))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.HeightFloor;
						heightFloor = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 12:
					if (identifier.EqualsIgnoreCase("textureFloor"))
					{
						if (HasFlag(seenFields, SectorFields.TextureFloor))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.TextureFloor;
						textureFloor = ParseTextureFieldValue(optional: false);
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 13:
					if (identifier.EqualsIgnoreCase("heightCeiling"))
					{
						if (HasFlag(seenFields, SectorFields.HeightCeiling))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.HeightCeiling;
						heightCeiling = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 14:
					if (identifier.EqualsIgnoreCase("textureCeiling"))
					{
						if (HasFlag(seenFields, SectorFields.TextureCeiling))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.TextureCeiling;
						textureCeiling = ParseTextureFieldValue(optional: false);
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				default:
					handledKnownField = false;
					break;
			}
			if (!handledKnownField)
			{
				unknownFields ??= [];
				if (!unknownFields.Add(identifier))
				{
					throw DuplicateField(identifier);
				}
				_lexer.SkipValueAndSemicolon();
			}
		}

		if (!HasFlag(seenFields, SectorFields.HeightFloor)) throw MissingRequiredField(blockName, "heightFloor");
		if (!HasFlag(seenFields, SectorFields.HeightCeiling)) throw MissingRequiredField(blockName, "heightCeiling");
		if (!HasFlag(seenFields, SectorFields.TextureFloor)) throw MissingRequiredField(blockName, "textureFloor");
		if (!HasFlag(seenFields, SectorFields.TextureCeiling)) throw MissingRequiredField(blockName, "textureCeiling");
		if (!HasFlag(seenFields, SectorFields.LightLevel)) throw MissingRequiredField(blockName, "lightLevel");
		return new Sector(
			HeightFloor: heightFloor,
			HeightCeiling: heightCeiling,
			TextureFloor: textureFloor,
			TextureCeiling: textureCeiling,
			LightLevel: lightLevel,
			Special: special,
			Id: id,
			DropActors: dropActors,
			Comment: comment
		);
	}
	private void AddParsedBlock(
		Identifier blockName
	)
	{
		if (blockName.EqualsIgnoreCase("thing"))
		{
			_thingBuilder.Add(ParseThingBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("vertex"))
		{
			_verticesBuilder.Add(ParseVertexBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("linedef"))
		{
			_lineDefBuilder.Add(ParseLineDefBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("sidedef"))
		{
			_sideDefBuilder.Add(ParseSideDefBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("sector"))
		{
			_sectorBuilder.Add(ParseSectorBlock(blockName));
		}
		else
		{
			throw new ParsingException($"Unknown block: {blockName}");
		}
	}
	private MapData CreateMapData()
	{
		return new MapData(
			NameSpace: _namespace ?? throw new ParsingException("Missing required field 'namespace'"),
			Things: _thingBuilder.ToImmutable(),
			Vertices: _verticesBuilder.ToImmutable(),
			LineDefs: _lineDefBuilder.ToImmutable(),
			SideDefs: _sideDefBuilder.ToImmutable(),
			Sectors: _sectorBuilder.ToImmutable(),
			Comment: _comment
		);
	}
}
