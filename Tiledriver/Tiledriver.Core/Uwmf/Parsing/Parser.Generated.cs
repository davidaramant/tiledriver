﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

namespace Tiledriver.Core.Uwmf.Parsing
{
    public static partial class Parser
    {
		private static Tile ParseTile( ILexer lexer )
		{
			var tile = new Tile();

			if (lexer.DetermineNextToken() != TokenType.StartBlock)
            {
                throw new ParsingException("Expecting start of block when parsing Tile.");
            }
            lexer.AdvanceOneCharacter();

			TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndBlock)
            {
				if( nextToken == TokenType.Identifier )
				{
		            switch( lexer.ReadIdentifier().Name.ToLowerInvariant() )
					{
						case "textureeast":
							tile.TextureEast = ParseStringAssignment( lexer, "Tile.TextureEast" );
							break;
						case "texturenorth":
							tile.TextureNorth = ParseStringAssignment( lexer, "Tile.TextureNorth" );
							break;
						case "texturewest":
							tile.TextureWest = ParseStringAssignment( lexer, "Tile.TextureWest" );
							break;
						case "texturesouth":
							tile.TextureSouth = ParseStringAssignment( lexer, "Tile.TextureSouth" );
							break;
						case "blockingeast":
							tile.BlockingEast = ParseBooleanAssignment( lexer, "Tile.BlockingEast" );
							break;
						case "blockingnorth":
							tile.BlockingNorth = ParseBooleanAssignment( lexer, "Tile.BlockingNorth" );
							break;
						case "blockingwest":
							tile.BlockingWest = ParseBooleanAssignment( lexer, "Tile.BlockingWest" );
							break;
						case "blockingsouth":
							tile.BlockingSouth = ParseBooleanAssignment( lexer, "Tile.BlockingSouth" );
							break;
						case "offsetvertical":
							tile.OffsetVertical = ParseBooleanAssignment( lexer, "Tile.OffsetVertical" );
							break;
						case "offsethorizontal":
							tile.OffsetHorizontal = ParseBooleanAssignment( lexer, "Tile.OffsetHorizontal" );
							break;
						case "dontoverlay":
							tile.DontOverlay = ParseBooleanAssignment( lexer, "Tile.DontOverlay" );
							break;
						case "mapped":
							tile.Mapped = ParseIntegerNumberAssignment( lexer, "Tile.Mapped" );
							break;
						case "soundsequence":
							tile.SoundSequence = ParseStringAssignment( lexer, "Tile.SoundSequence" );
							break;
						case "textureoverhead":
							tile.TextureOverhead = ParseStringAssignment( lexer, "Tile.TextureOverhead" );
							break;
						default:
							lexer.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new ParsingException("Unexpected token in Tile.");
				}
            }
			lexer.AdvanceOneCharacter();

			tile.CheckSemanticValidity();
			return tile;
		}

		private static Sector ParseSector( ILexer lexer )
		{
			var sector = new Sector();

			if (lexer.DetermineNextToken() != TokenType.StartBlock)
            {
                throw new ParsingException("Expecting start of block when parsing Sector.");
            }
            lexer.AdvanceOneCharacter();

			TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndBlock)
            {
				if( nextToken == TokenType.Identifier )
				{
		            switch( lexer.ReadIdentifier().Name.ToLowerInvariant() )
					{
						case "textureceiling":
							sector.TextureCeiling = ParseStringAssignment( lexer, "Sector.TextureCeiling" );
							break;
						case "texturefloor":
							sector.TextureFloor = ParseStringAssignment( lexer, "Sector.TextureFloor" );
							break;
						default:
							lexer.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new ParsingException("Unexpected token in Sector.");
				}
            }
			lexer.AdvanceOneCharacter();

			sector.CheckSemanticValidity();
			return sector;
		}

		private static Zone ParseZone( ILexer lexer )
		{
			var zone = new Zone();

			if (lexer.DetermineNextToken() != TokenType.StartBlock)
            {
                throw new ParsingException("Expecting start of block when parsing Zone.");
            }
            lexer.AdvanceOneCharacter();

			TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndBlock)
            {
				if( nextToken == TokenType.Identifier )
				{
		            switch( lexer.ReadIdentifier().Name.ToLowerInvariant() )
					{
						default:
							lexer.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new ParsingException("Unexpected token in Zone.");
				}
            }
			lexer.AdvanceOneCharacter();

			zone.CheckSemanticValidity();
			return zone;
		}

		private static Plane ParsePlane( ILexer lexer )
		{
			var plane = new Plane();

			if (lexer.DetermineNextToken() != TokenType.StartBlock)
            {
                throw new ParsingException("Expecting start of block when parsing Plane.");
            }
            lexer.AdvanceOneCharacter();

			TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndBlock)
            {
				if( nextToken == TokenType.Identifier )
				{
		            switch( lexer.ReadIdentifier().Name.ToLowerInvariant() )
					{
						case "depth":
							plane.Depth = ParseIntegerNumberAssignment( lexer, "Plane.Depth" );
							break;
						default:
							lexer.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new ParsingException("Unexpected token in Plane.");
				}
            }
			lexer.AdvanceOneCharacter();

			plane.CheckSemanticValidity();
			return plane;
		}

		private static PlaneMap ParsePlaneMap( ILexer lexer )
		{
			var planeMap = new PlaneMap();

			if (lexer.DetermineNextToken() != TokenType.StartBlock)
            {
                throw new ParsingException("Expecting start of block when parsing PlaneMap.");
            }
            lexer.AdvanceOneCharacter();

			TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndBlock)
            {
				if( nextToken == TokenType.Identifier )
				{
		            switch( lexer.ReadIdentifier().Name.ToLowerInvariant() )
					{
						case "tilespace":
							planeMap.TileSpaces.Add(ParseTileSpace(lexer));
							break;
						default:
							lexer.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new ParsingException("Unexpected token in PlaneMap.");
				}
            }
			lexer.AdvanceOneCharacter();

			planeMap.CheckSemanticValidity();
			return planeMap;
		}

		private static Thing ParseThing( ILexer lexer )
		{
			var thing = new Thing();

			if (lexer.DetermineNextToken() != TokenType.StartBlock)
            {
                throw new ParsingException("Expecting start of block when parsing Thing.");
            }
            lexer.AdvanceOneCharacter();

			TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndBlock)
            {
				if( nextToken == TokenType.Identifier )
				{
		            switch( lexer.ReadIdentifier().Name.ToLowerInvariant() )
					{
						case "type":
							thing.Type = ParseIntegerNumberAssignment( lexer, "Thing.Type" );
							break;
						case "x":
							thing.X = ParseFloatingPointNumberAssignment( lexer, "Thing.X" );
							break;
						case "y":
							thing.Y = ParseFloatingPointNumberAssignment( lexer, "Thing.Y" );
							break;
						case "z":
							thing.Z = ParseFloatingPointNumberAssignment( lexer, "Thing.Z" );
							break;
						case "angle":
							thing.Angle = ParseIntegerNumberAssignment( lexer, "Thing.Angle" );
							break;
						case "ambush":
							thing.Ambush = ParseBooleanAssignment( lexer, "Thing.Ambush" );
							break;
						case "patrol":
							thing.Patrol = ParseBooleanAssignment( lexer, "Thing.Patrol" );
							break;
						case "skill1":
							thing.Skill1 = ParseBooleanAssignment( lexer, "Thing.Skill1" );
							break;
						case "skill2":
							thing.Skill2 = ParseBooleanAssignment( lexer, "Thing.Skill2" );
							break;
						case "skill3":
							thing.Skill3 = ParseBooleanAssignment( lexer, "Thing.Skill3" );
							break;
						case "skill4":
							thing.Skill4 = ParseBooleanAssignment( lexer, "Thing.Skill4" );
							break;
						case "skill5":
							thing.Skill5 = ParseBooleanAssignment( lexer, "Thing.Skill5" );
							break;
						default:
							lexer.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new ParsingException("Unexpected token in Thing.");
				}
            }
			lexer.AdvanceOneCharacter();

			thing.CheckSemanticValidity();
			return thing;
		}

		private static Trigger ParseTrigger( ILexer lexer )
		{
			var trigger = new Trigger();

			if (lexer.DetermineNextToken() != TokenType.StartBlock)
            {
                throw new ParsingException("Expecting start of block when parsing Trigger.");
            }
            lexer.AdvanceOneCharacter();

			TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndBlock)
            {
				if( nextToken == TokenType.Identifier )
				{
		            switch( lexer.ReadIdentifier().Name.ToLowerInvariant() )
					{
						case "x":
							trigger.X = ParseIntegerNumberAssignment( lexer, "Trigger.X" );
							break;
						case "y":
							trigger.Y = ParseIntegerNumberAssignment( lexer, "Trigger.Y" );
							break;
						case "z":
							trigger.Z = ParseIntegerNumberAssignment( lexer, "Trigger.Z" );
							break;
						case "action":
							trigger.Action = ParseIntegerNumberAssignment( lexer, "Trigger.Action" );
							break;
						case "arg0":
							trigger.Arg0 = ParseIntegerNumberAssignment( lexer, "Trigger.Arg0" );
							break;
						case "arg1":
							trigger.Arg1 = ParseIntegerNumberAssignment( lexer, "Trigger.Arg1" );
							break;
						case "arg2":
							trigger.Arg2 = ParseIntegerNumberAssignment( lexer, "Trigger.Arg2" );
							break;
						case "arg3":
							trigger.Arg3 = ParseIntegerNumberAssignment( lexer, "Trigger.Arg3" );
							break;
						case "arg4":
							trigger.Arg4 = ParseIntegerNumberAssignment( lexer, "Trigger.Arg4" );
							break;
						case "activateeast":
							trigger.ActivateEast = ParseBooleanAssignment( lexer, "Trigger.ActivateEast" );
							break;
						case "activatenorth":
							trigger.ActivateNorth = ParseBooleanAssignment( lexer, "Trigger.ActivateNorth" );
							break;
						case "activatewest":
							trigger.ActivateWest = ParseBooleanAssignment( lexer, "Trigger.ActivateWest" );
							break;
						case "activatesouth":
							trigger.ActivateSouth = ParseBooleanAssignment( lexer, "Trigger.ActivateSouth" );
							break;
						case "playercross":
							trigger.PlayerCross = ParseBooleanAssignment( lexer, "Trigger.PlayerCross" );
							break;
						case "playeruse":
							trigger.PlayerUse = ParseBooleanAssignment( lexer, "Trigger.PlayerUse" );
							break;
						case "monsteruse":
							trigger.MonsterUse = ParseBooleanAssignment( lexer, "Trigger.MonsterUse" );
							break;
						case "repeatable":
							trigger.Repeatable = ParseBooleanAssignment( lexer, "Trigger.Repeatable" );
							break;
						case "secret":
							trigger.Secret = ParseBooleanAssignment( lexer, "Trigger.Secret" );
							break;
						default:
							lexer.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new ParsingException("Unexpected token in Trigger.");
				}
            }
			lexer.AdvanceOneCharacter();

			trigger.CheckSemanticValidity();
			return trigger;
		}

		private static Map ParseMap( ILexer lexer )
		{
			var map = new Map();

			TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndOfFile)
            {
				if( nextToken == TokenType.Identifier )
				{
		            switch( lexer.ReadIdentifier().Name.ToLowerInvariant() )
					{
						case "namespace":
							map.Namespace = ParseStringAssignment( lexer, "Map.Namespace" );
							break;
						case "tilesize":
							map.TileSize = ParseIntegerNumberAssignment( lexer, "Map.TileSize" );
							break;
						case "name":
							map.Name = ParseStringAssignment( lexer, "Map.Name" );
							break;
						case "width":
							map.Width = ParseIntegerNumberAssignment( lexer, "Map.Width" );
							break;
						case "height":
							map.Height = ParseIntegerNumberAssignment( lexer, "Map.Height" );
							break;
						case "tile":
							map.Tiles.Add(ParseTile(lexer));
							break;
						case "sector":
							map.Sectors.Add(ParseSector(lexer));
							break;
						case "zone":
							map.Zones.Add(ParseZone(lexer));
							break;
						case "plane":
							map.Planes.Add(ParsePlane(lexer));
							break;
						case "planemap":
							map.PlaneMaps.Add(ParsePlaneMap(lexer));
							break;
						case "thing":
							map.Things.Add(ParseThing(lexer));
							break;
						case "trigger":
							map.Triggers.Add(ParseTrigger(lexer));
							break;
						default:
							lexer.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new ParsingException("Unexpected token in Map.");
				}
            }

			map.CheckSemanticValidity();
			return map;
		}

	}
}