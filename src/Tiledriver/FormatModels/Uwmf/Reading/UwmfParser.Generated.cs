using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Uwmf;

#nullable enable
namespace Tiledriver.FormatModels.Uwmf.Reading;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial class UwmfParser
{
	[global::System.Flags]
	private enum TileFields : uint
	{
		None = 0,
		TextureEast = 1U << 0,
		TextureNorth = 1U << 1,
		TextureWest = 1U << 2,
		TextureSouth = 1U << 3,
		BlockingEast = 1U << 4,
		BlockingNorth = 1U << 5,
		BlockingWest = 1U << 6,
		BlockingSouth = 1U << 7,
		OffsetVertical = 1U << 8,
		OffsetHorizontal = 1U << 9,
		DontOverlay = 1U << 10,
		Mapped = 1U << 11,
		SoundSequence = 1U << 12,
		TextureOverhead = 1U << 13,
		Comment = 1U << 14,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(TileFields value, TileFields flag) => (value & flag) == flag;

	private Tile ParseTileBlock(Identifier blockName)
	{
		Texture textureEast = default!;
		Texture textureNorth = default!;
		Texture textureWest = default!;
		Texture textureSouth = default!;
		bool blockingEast = true;
		bool blockingNorth = true;
		bool blockingWest = true;
		bool blockingSouth = true;
		bool offsetVertical = false;
		bool offsetHorizontal = false;
		bool dontOverlay = false;
		int mapped = 0;
		string soundSequence = "";
		string textureOverhead = "";
		string comment = "";
		TileFields seenFields = TileFields.None;
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
					if (identifier.EqualsIgnoreCase("mapped"))
					{
						if (HasFlag(seenFields, TileFields.Mapped))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.Mapped;
						mapped = _lexer.ReadInteger();
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
						if (HasFlag(seenFields, TileFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.Comment;
						comment = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 11:
					if (identifier.EqualsIgnoreCase("textureEast"))
					{
						if (HasFlag(seenFields, TileFields.TextureEast))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.TextureEast;
						textureEast = ParseTextureFieldValue();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("textureWest"))
					{
						if (HasFlag(seenFields, TileFields.TextureWest))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.TextureWest;
						textureWest = ParseTextureFieldValue();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("dontOverlay"))
					{
						if (HasFlag(seenFields, TileFields.DontOverlay))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.DontOverlay;
						dontOverlay = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 12:
					if (identifier.EqualsIgnoreCase("textureNorth"))
					{
						if (HasFlag(seenFields, TileFields.TextureNorth))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.TextureNorth;
						textureNorth = ParseTextureFieldValue();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("textureSouth"))
					{
						if (HasFlag(seenFields, TileFields.TextureSouth))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.TextureSouth;
						textureSouth = ParseTextureFieldValue();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("blockingEast"))
					{
						if (HasFlag(seenFields, TileFields.BlockingEast))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.BlockingEast;
						blockingEast = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("blockingWest"))
					{
						if (HasFlag(seenFields, TileFields.BlockingWest))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.BlockingWest;
						blockingWest = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 13:
					if (identifier.EqualsIgnoreCase("blockingNorth"))
					{
						if (HasFlag(seenFields, TileFields.BlockingNorth))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.BlockingNorth;
						blockingNorth = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("blockingSouth"))
					{
						if (HasFlag(seenFields, TileFields.BlockingSouth))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.BlockingSouth;
						blockingSouth = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("soundSequence"))
					{
						if (HasFlag(seenFields, TileFields.SoundSequence))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.SoundSequence;
						soundSequence = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 14:
					if (identifier.EqualsIgnoreCase("offsetVertical"))
					{
						if (HasFlag(seenFields, TileFields.OffsetVertical))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.OffsetVertical;
						offsetVertical = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 15:
					if (identifier.EqualsIgnoreCase("textureOverhead"))
					{
						if (HasFlag(seenFields, TileFields.TextureOverhead))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.TextureOverhead;
						textureOverhead = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 16:
					if (identifier.EqualsIgnoreCase("offsetHorizontal"))
					{
						if (HasFlag(seenFields, TileFields.OffsetHorizontal))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TileFields.OffsetHorizontal;
						offsetHorizontal = _lexer.ReadBoolean();
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

		if (!HasFlag(seenFields, TileFields.TextureEast)) throw MissingRequiredField("textureEast");
		if (!HasFlag(seenFields, TileFields.TextureNorth)) throw MissingRequiredField("textureNorth");
		if (!HasFlag(seenFields, TileFields.TextureWest)) throw MissingRequiredField("textureWest");
		if (!HasFlag(seenFields, TileFields.TextureSouth)) throw MissingRequiredField("textureSouth");
		return new Tile(
			TextureEast: textureEast,
			TextureNorth: textureNorth,
			TextureWest: textureWest,
			TextureSouth: textureSouth,
			BlockingEast: blockingEast,
			BlockingNorth: blockingNorth,
			BlockingWest: blockingWest,
			BlockingSouth: blockingSouth,
			OffsetVertical: offsetVertical,
			OffsetHorizontal: offsetHorizontal,
			DontOverlay: dontOverlay,
			Mapped: mapped,
			SoundSequence: soundSequence,
			TextureOverhead: textureOverhead,
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum SectorFields : uint
	{
		None = 0,
		TextureCeiling = 1U << 0,
		TextureFloor = 1U << 1,
		Comment = 1U << 2,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(SectorFields value, SectorFields flag) => (value & flag) == flag;

	private Sector ParseSectorBlock(Identifier blockName)
	{
		Texture textureCeiling = default!;
		Texture textureFloor = default!;
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
				case 7:
					if (identifier.EqualsIgnoreCase("comment"))
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
				case 12:
					if (identifier.EqualsIgnoreCase("textureFloor"))
					{
						if (HasFlag(seenFields, SectorFields.TextureFloor))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= SectorFields.TextureFloor;
						textureFloor = ParseTextureFieldValue();
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
						textureCeiling = ParseTextureFieldValue();
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

		if (!HasFlag(seenFields, SectorFields.TextureCeiling)) throw MissingRequiredField("textureCeiling");
		if (!HasFlag(seenFields, SectorFields.TextureFloor)) throw MissingRequiredField("textureFloor");
		return new Sector(
			TextureCeiling: textureCeiling,
			TextureFloor: textureFloor,
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum ZoneFields : uint
	{
		None = 0,
		Comment = 1U << 0,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(ZoneFields value, ZoneFields flag) => (value & flag) == flag;

	private Zone ParseZoneBlock(Identifier blockName)
	{
		string comment = "";
		ZoneFields seenFields = ZoneFields.None;
		HashSet<Identifier>? unknownFields = null;

		while (!_lexer.TryExpectCloseBrace())
		{
			Identifier identifier = _lexer.ReadIdentifier();
			string identifierText = (string)identifier;
			_lexer.ExpectEquals();
			bool handledKnownField = false;
			switch (identifierText.Length)
			{
				case 7:
					if (identifier.EqualsIgnoreCase("comment"))
					{
						if (HasFlag(seenFields, ZoneFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ZoneFields.Comment;
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

		return new Zone(
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum PlaneFields : uint
	{
		None = 0,
		Depth = 1U << 0,
		Comment = 1U << 1,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(PlaneFields value, PlaneFields flag) => (value & flag) == flag;

	private Plane ParsePlaneBlock(Identifier blockName)
	{
		int depth = default;
		string comment = "";
		PlaneFields seenFields = PlaneFields.None;
		HashSet<Identifier>? unknownFields = null;

		while (!_lexer.TryExpectCloseBrace())
		{
			Identifier identifier = _lexer.ReadIdentifier();
			string identifierText = (string)identifier;
			_lexer.ExpectEquals();
			bool handledKnownField = false;
			switch (identifierText.Length)
			{
				case 5:
					if (identifier.EqualsIgnoreCase("depth"))
					{
						if (HasFlag(seenFields, PlaneFields.Depth))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= PlaneFields.Depth;
						depth = _lexer.ReadInteger();
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
						if (HasFlag(seenFields, PlaneFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= PlaneFields.Comment;
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

		if (!HasFlag(seenFields, PlaneFields.Depth)) throw MissingRequiredField("depth");
		return new Plane(
			Depth: depth,
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum ThingFields : uint
	{
		None = 0,
		Type = 1U << 0,
		X = 1U << 1,
		Y = 1U << 2,
		Z = 1U << 3,
		Angle = 1U << 4,
		Ambush = 1U << 5,
		Patrol = 1U << 6,
		Skill1 = 1U << 7,
		Skill2 = 1U << 8,
		Skill3 = 1U << 9,
		Skill4 = 1U << 10,
		Comment = 1U << 11,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(ThingFields value, ThingFields flag) => (value & flag) == flag;

	private Thing ParseThingBlock(Identifier blockName)
	{
		string type = default!;
		double x = default;
		double y = default;
		double z = default;
		int angle = default;
		bool ambush = false;
		bool patrol = false;
		bool skill1 = false;
		bool skill2 = false;
		bool skill3 = false;
		bool skill4 = false;
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
					else if (identifier.EqualsIgnoreCase("z"))
					{
						if (HasFlag(seenFields, ThingFields.Z))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Z;
						z = _lexer.ReadDouble();
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
						type = _lexer.ReadString();
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
					if (identifier.EqualsIgnoreCase("ambush"))
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
					else if (identifier.EqualsIgnoreCase("patrol"))
					{
						if (HasFlag(seenFields, ThingFields.Patrol))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= ThingFields.Patrol;
						patrol = _lexer.ReadBoolean();
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

		if (!HasFlag(seenFields, ThingFields.Type)) throw MissingRequiredField("type");
		if (!HasFlag(seenFields, ThingFields.X)) throw MissingRequiredField("x");
		if (!HasFlag(seenFields, ThingFields.Y)) throw MissingRequiredField("y");
		if (!HasFlag(seenFields, ThingFields.Z)) throw MissingRequiredField("z");
		if (!HasFlag(seenFields, ThingFields.Angle)) throw MissingRequiredField("angle");
		return new Thing(
			Type: type,
			X: x,
			Y: y,
			Z: z,
			Angle: angle,
			Ambush: ambush,
			Patrol: patrol,
			Skill1: skill1,
			Skill2: skill2,
			Skill3: skill3,
			Skill4: skill4,
			Comment: comment
		);
	}
	[global::System.Flags]
	private enum TriggerFields : uint
	{
		None = 0,
		X = 1U << 0,
		Y = 1U << 1,
		Z = 1U << 2,
		Action = 1U << 3,
		Arg0 = 1U << 4,
		Arg1 = 1U << 5,
		Arg2 = 1U << 6,
		Arg3 = 1U << 7,
		Arg4 = 1U << 8,
		ActivateEast = 1U << 9,
		ActivateNorth = 1U << 10,
		ActivateWest = 1U << 11,
		ActivateSouth = 1U << 12,
		PlayerCross = 1U << 13,
		PlayerUse = 1U << 14,
		MonsterUse = 1U << 15,
		Repeatable = 1U << 16,
		Secret = 1U << 17,
		Comment = 1U << 18,
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasFlag(TriggerFields value, TriggerFields flag) => (value & flag) == flag;

	private Trigger ParseTriggerBlock(Identifier blockName)
	{
		int x = default;
		int y = default;
		int z = default;
		string action = default!;
		int arg0 = 0;
		int arg1 = 0;
		int arg2 = 0;
		int arg3 = 0;
		int arg4 = 0;
		bool activateEast = true;
		bool activateNorth = true;
		bool activateWest = true;
		bool activateSouth = true;
		bool playerCross = false;
		bool playerUse = false;
		bool monsterUse = false;
		bool repeatable = false;
		bool secret = false;
		string comment = "";
		TriggerFields seenFields = TriggerFields.None;
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
						if (HasFlag(seenFields, TriggerFields.X))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.X;
						x = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("y"))
					{
						if (HasFlag(seenFields, TriggerFields.Y))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Y;
						y = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("z"))
					{
						if (HasFlag(seenFields, TriggerFields.Z))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Z;
						z = _lexer.ReadInteger();
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
						if (HasFlag(seenFields, TriggerFields.Arg0))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Arg0;
						arg0 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("arg1"))
					{
						if (HasFlag(seenFields, TriggerFields.Arg1))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Arg1;
						arg1 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("arg2"))
					{
						if (HasFlag(seenFields, TriggerFields.Arg2))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Arg2;
						arg2 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("arg3"))
					{
						if (HasFlag(seenFields, TriggerFields.Arg3))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Arg3;
						arg3 = _lexer.ReadInteger();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("arg4"))
					{
						if (HasFlag(seenFields, TriggerFields.Arg4))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Arg4;
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
					if (identifier.EqualsIgnoreCase("action"))
					{
						if (HasFlag(seenFields, TriggerFields.Action))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Action;
						action = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("secret"))
					{
						if (HasFlag(seenFields, TriggerFields.Secret))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Secret;
						secret = _lexer.ReadBoolean();
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
						if (HasFlag(seenFields, TriggerFields.Comment))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Comment;
						comment = _lexer.ReadString();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 9:
					if (identifier.EqualsIgnoreCase("playerUse"))
					{
						if (HasFlag(seenFields, TriggerFields.PlayerUse))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.PlayerUse;
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
					if (identifier.EqualsIgnoreCase("monsterUse"))
					{
						if (HasFlag(seenFields, TriggerFields.MonsterUse))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.MonsterUse;
						monsterUse = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("repeatable"))
					{
						if (HasFlag(seenFields, TriggerFields.Repeatable))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.Repeatable;
						repeatable = _lexer.ReadBoolean();
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
						if (HasFlag(seenFields, TriggerFields.PlayerCross))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.PlayerCross;
						playerCross = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 12:
					if (identifier.EqualsIgnoreCase("activateEast"))
					{
						if (HasFlag(seenFields, TriggerFields.ActivateEast))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.ActivateEast;
						activateEast = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("activateWest"))
					{
						if (HasFlag(seenFields, TriggerFields.ActivateWest))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.ActivateWest;
						activateWest = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else
					{
						handledKnownField = false;
					}
					break;
				case 13:
					if (identifier.EqualsIgnoreCase("activateNorth"))
					{
						if (HasFlag(seenFields, TriggerFields.ActivateNorth))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.ActivateNorth;
						activateNorth = _lexer.ReadBoolean();
						_lexer.ExpectSemicolon();
						handledKnownField = true;
					}
					else if (identifier.EqualsIgnoreCase("activateSouth"))
					{
						if (HasFlag(seenFields, TriggerFields.ActivateSouth))
						{
							throw DuplicateField(identifier);
						}
						seenFields |= TriggerFields.ActivateSouth;
						activateSouth = _lexer.ReadBoolean();
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

		if (!HasFlag(seenFields, TriggerFields.X)) throw MissingRequiredField("x");
		if (!HasFlag(seenFields, TriggerFields.Y)) throw MissingRequiredField("y");
		if (!HasFlag(seenFields, TriggerFields.Z)) throw MissingRequiredField("z");
		if (!HasFlag(seenFields, TriggerFields.Action)) throw MissingRequiredField("action");
		return new Trigger(
			X: x,
			Y: y,
			Z: z,
			Action: action,
			Arg0: arg0,
			Arg1: arg1,
			Arg2: arg2,
			Arg3: arg3,
			Arg4: arg4,
			ActivateEast: activateEast,
			ActivateNorth: activateNorth,
			ActivateWest: activateWest,
			ActivateSouth: activateSouth,
			PlayerCross: playerCross,
			PlayerUse: playerUse,
			MonsterUse: monsterUse,
			Repeatable: repeatable,
			Secret: secret,
			Comment: comment
		);
	}
	private void AddGeneratedParsedBlock(
		Identifier blockName
	)
	{
		if (blockName.EqualsIgnoreCase("tile"))
		{
			_tileBuilder.Add(ParseTileBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("sector"))
		{
			_sectorBuilder.Add(ParseSectorBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("zone"))
		{
			_zoneBuilder.Add(ParseZoneBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("plane"))
		{
			_planeBuilder.Add(ParsePlaneBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("thing"))
		{
			_thingBuilder.Add(ParseThingBlock(blockName));
		}
		else if (blockName.EqualsIgnoreCase("trigger"))
		{
			_triggerBuilder.Add(ParseTriggerBlock(blockName));
		}
		else
		{
			throw new ParsingException($"Unknown block: {blockName}");
		}
	}
}
