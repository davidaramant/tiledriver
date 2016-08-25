// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.Uwmf.Parsing
{
    public static partial class Parser
    {
		private static Tile ParseTile( ILexerOld lexerOld )
		{
			var tile = new Tile();

			TokenTypeOld nextToken;
			nextToken = lexerOld.DetermineNextToken();
			if (nextToken != TokenTypeOld.StartBlock)
            {
                throw new UwmfParsingException($"Expecting start of block when parsing Tile but found {nextToken}.");
            }
            lexerOld.AdvanceOneCharacter();

            while ((nextToken = lexerOld.DetermineNextToken()) != TokenTypeOld.EndBlock)
            {
				if( nextToken == TokenTypeOld.Identifier )
				{
		            switch( lexerOld.ReadIdentifier().ToString() )
					{
						case "textureeast":
							tile.TextureEast = ParseStringAssignment( lexerOld, "Tile.TextureEast" );
							break;
						case "texturenorth":
							tile.TextureNorth = ParseStringAssignment( lexerOld, "Tile.TextureNorth" );
							break;
						case "texturewest":
							tile.TextureWest = ParseStringAssignment( lexerOld, "Tile.TextureWest" );
							break;
						case "texturesouth":
							tile.TextureSouth = ParseStringAssignment( lexerOld, "Tile.TextureSouth" );
							break;
						case "blockingeast":
							tile.BlockingEast = ParseBooleanAssignment( lexerOld, "Tile.BlockingEast" );
							break;
						case "blockingnorth":
							tile.BlockingNorth = ParseBooleanAssignment( lexerOld, "Tile.BlockingNorth" );
							break;
						case "blockingwest":
							tile.BlockingWest = ParseBooleanAssignment( lexerOld, "Tile.BlockingWest" );
							break;
						case "blockingsouth":
							tile.BlockingSouth = ParseBooleanAssignment( lexerOld, "Tile.BlockingSouth" );
							break;
						case "offsetvertical":
							tile.OffsetVertical = ParseBooleanAssignment( lexerOld, "Tile.OffsetVertical" );
							break;
						case "offsethorizontal":
							tile.OffsetHorizontal = ParseBooleanAssignment( lexerOld, "Tile.OffsetHorizontal" );
							break;
						case "dontoverlay":
							tile.DontOverlay = ParseBooleanAssignment( lexerOld, "Tile.DontOverlay" );
							break;
						case "mapped":
							tile.Mapped = ParseIntegerNumberAssignment( lexerOld, "Tile.Mapped" );
							break;
						case "soundsequence":
							tile.SoundSequence = ParseStringAssignment( lexerOld, "Tile.SoundSequence" );
							break;
						case "textureoverhead":
							tile.TextureOverhead = ParseStringAssignment( lexerOld, "Tile.TextureOverhead" );
							break;
						case "comment":
							tile.Comment = ParseStringAssignment( lexerOld, "Tile.Comment" );
							break;
						default:
							lexerOld.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new UwmfParsingException($"Unexpected token in Tile: {nextToken}");
				}
            }
			lexerOld.AdvanceOneCharacter();

			tile.CheckSemanticValidity();
			return tile;
		}

		private static Sector ParseSector( ILexerOld lexerOld )
		{
			var sector = new Sector();

			TokenTypeOld nextToken;
			nextToken = lexerOld.DetermineNextToken();
			if (nextToken != TokenTypeOld.StartBlock)
            {
                throw new UwmfParsingException($"Expecting start of block when parsing Sector but found {nextToken}.");
            }
            lexerOld.AdvanceOneCharacter();

            while ((nextToken = lexerOld.DetermineNextToken()) != TokenTypeOld.EndBlock)
            {
				if( nextToken == TokenTypeOld.Identifier )
				{
		            switch( lexerOld.ReadIdentifier().ToString() )
					{
						case "textureceiling":
							sector.TextureCeiling = ParseStringAssignment( lexerOld, "Sector.TextureCeiling" );
							break;
						case "texturefloor":
							sector.TextureFloor = ParseStringAssignment( lexerOld, "Sector.TextureFloor" );
							break;
						case "comment":
							sector.Comment = ParseStringAssignment( lexerOld, "Sector.Comment" );
							break;
						default:
							lexerOld.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new UwmfParsingException($"Unexpected token in Sector: {nextToken}");
				}
            }
			lexerOld.AdvanceOneCharacter();

			sector.CheckSemanticValidity();
			return sector;
		}

		private static Zone ParseZone( ILexerOld lexerOld )
		{
			var zone = new Zone();

			TokenTypeOld nextToken;
			nextToken = lexerOld.DetermineNextToken();
			if (nextToken != TokenTypeOld.StartBlock)
            {
                throw new UwmfParsingException($"Expecting start of block when parsing Zone but found {nextToken}.");
            }
            lexerOld.AdvanceOneCharacter();

            while ((nextToken = lexerOld.DetermineNextToken()) != TokenTypeOld.EndBlock)
            {
				if( nextToken == TokenTypeOld.Identifier )
				{
		            switch( lexerOld.ReadIdentifier().ToString() )
					{
						case "comment":
							zone.Comment = ParseStringAssignment( lexerOld, "Zone.Comment" );
							break;
						default:
							lexerOld.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new UwmfParsingException($"Unexpected token in Zone: {nextToken}");
				}
            }
			lexerOld.AdvanceOneCharacter();

			zone.CheckSemanticValidity();
			return zone;
		}

		private static Plane ParsePlane( ILexerOld lexerOld )
		{
			var plane = new Plane();

			TokenTypeOld nextToken;
			nextToken = lexerOld.DetermineNextToken();
			if (nextToken != TokenTypeOld.StartBlock)
            {
                throw new UwmfParsingException($"Expecting start of block when parsing Plane but found {nextToken}.");
            }
            lexerOld.AdvanceOneCharacter();

            while ((nextToken = lexerOld.DetermineNextToken()) != TokenTypeOld.EndBlock)
            {
				if( nextToken == TokenTypeOld.Identifier )
				{
		            switch( lexerOld.ReadIdentifier().ToString() )
					{
						case "depth":
							plane.Depth = ParseIntegerNumberAssignment( lexerOld, "Plane.Depth" );
							break;
						case "comment":
							plane.Comment = ParseStringAssignment( lexerOld, "Plane.Comment" );
							break;
						default:
							lexerOld.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new UwmfParsingException($"Unexpected token in Plane: {nextToken}");
				}
            }
			lexerOld.AdvanceOneCharacter();

			plane.CheckSemanticValidity();
			return plane;
		}

		private static Thing ParseThing( ILexerOld lexerOld )
		{
			var thing = new Thing();

			TokenTypeOld nextToken;
			nextToken = lexerOld.DetermineNextToken();
			if (nextToken != TokenTypeOld.StartBlock)
            {
                throw new UwmfParsingException($"Expecting start of block when parsing Thing but found {nextToken}.");
            }
            lexerOld.AdvanceOneCharacter();

            while ((nextToken = lexerOld.DetermineNextToken()) != TokenTypeOld.EndBlock)
            {
				if( nextToken == TokenTypeOld.Identifier )
				{
		            switch( lexerOld.ReadIdentifier().ToString() )
					{
						case "type":
							thing.Type = ParseStringAssignment( lexerOld, "Thing.Type" );
							break;
						case "x":
							thing.X = ParseFloatingPointNumberAssignment( lexerOld, "Thing.X" );
							break;
						case "y":
							thing.Y = ParseFloatingPointNumberAssignment( lexerOld, "Thing.Y" );
							break;
						case "z":
							thing.Z = ParseFloatingPointNumberAssignment( lexerOld, "Thing.Z" );
							break;
						case "angle":
							thing.Angle = ParseIntegerNumberAssignment( lexerOld, "Thing.Angle" );
							break;
						case "ambush":
							thing.Ambush = ParseBooleanAssignment( lexerOld, "Thing.Ambush" );
							break;
						case "patrol":
							thing.Patrol = ParseBooleanAssignment( lexerOld, "Thing.Patrol" );
							break;
						case "skill1":
							thing.Skill1 = ParseBooleanAssignment( lexerOld, "Thing.Skill1" );
							break;
						case "skill2":
							thing.Skill2 = ParseBooleanAssignment( lexerOld, "Thing.Skill2" );
							break;
						case "skill3":
							thing.Skill3 = ParseBooleanAssignment( lexerOld, "Thing.Skill3" );
							break;
						case "skill4":
							thing.Skill4 = ParseBooleanAssignment( lexerOld, "Thing.Skill4" );
							break;
						case "skill5":
							thing.Skill5 = ParseBooleanAssignment( lexerOld, "Thing.Skill5" );
							break;
						case "comment":
							thing.Comment = ParseStringAssignment( lexerOld, "Thing.Comment" );
							break;
						default:
							lexerOld.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new UwmfParsingException($"Unexpected token in Thing: {nextToken}");
				}
            }
			lexerOld.AdvanceOneCharacter();

			thing.CheckSemanticValidity();
			return thing;
		}

		private static Trigger ParseTrigger( ILexerOld lexerOld )
		{
			var trigger = new Trigger();

			TokenTypeOld nextToken;
			nextToken = lexerOld.DetermineNextToken();
			if (nextToken != TokenTypeOld.StartBlock)
            {
                throw new UwmfParsingException($"Expecting start of block when parsing Trigger but found {nextToken}.");
            }
            lexerOld.AdvanceOneCharacter();

            while ((nextToken = lexerOld.DetermineNextToken()) != TokenTypeOld.EndBlock)
            {
				if( nextToken == TokenTypeOld.Identifier )
				{
		            switch( lexerOld.ReadIdentifier().ToString() )
					{
						case "x":
							trigger.X = ParseIntegerNumberAssignment( lexerOld, "Trigger.X" );
							break;
						case "y":
							trigger.Y = ParseIntegerNumberAssignment( lexerOld, "Trigger.Y" );
							break;
						case "z":
							trigger.Z = ParseIntegerNumberAssignment( lexerOld, "Trigger.Z" );
							break;
						case "action":
							trigger.Action = ParseStringAssignment( lexerOld, "Trigger.Action" );
							break;
						case "arg0":
							trigger.Arg0 = ParseIntegerNumberAssignment( lexerOld, "Trigger.Arg0" );
							break;
						case "arg1":
							trigger.Arg1 = ParseIntegerNumberAssignment( lexerOld, "Trigger.Arg1" );
							break;
						case "arg2":
							trigger.Arg2 = ParseIntegerNumberAssignment( lexerOld, "Trigger.Arg2" );
							break;
						case "arg3":
							trigger.Arg3 = ParseIntegerNumberAssignment( lexerOld, "Trigger.Arg3" );
							break;
						case "arg4":
							trigger.Arg4 = ParseIntegerNumberAssignment( lexerOld, "Trigger.Arg4" );
							break;
						case "activateeast":
							trigger.ActivateEast = ParseBooleanAssignment( lexerOld, "Trigger.ActivateEast" );
							break;
						case "activatenorth":
							trigger.ActivateNorth = ParseBooleanAssignment( lexerOld, "Trigger.ActivateNorth" );
							break;
						case "activatewest":
							trigger.ActivateWest = ParseBooleanAssignment( lexerOld, "Trigger.ActivateWest" );
							break;
						case "activatesouth":
							trigger.ActivateSouth = ParseBooleanAssignment( lexerOld, "Trigger.ActivateSouth" );
							break;
						case "playercross":
							trigger.PlayerCross = ParseBooleanAssignment( lexerOld, "Trigger.PlayerCross" );
							break;
						case "playeruse":
							trigger.PlayerUse = ParseBooleanAssignment( lexerOld, "Trigger.PlayerUse" );
							break;
						case "monsteruse":
							trigger.MonsterUse = ParseBooleanAssignment( lexerOld, "Trigger.MonsterUse" );
							break;
						case "repeatable":
							trigger.Repeatable = ParseBooleanAssignment( lexerOld, "Trigger.Repeatable" );
							break;
						case "secret":
							trigger.Secret = ParseBooleanAssignment( lexerOld, "Trigger.Secret" );
							break;
						case "comment":
							trigger.Comment = ParseStringAssignment( lexerOld, "Trigger.Comment" );
							break;
						default:
							lexerOld.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new UwmfParsingException($"Unexpected token in Trigger: {nextToken}");
				}
            }
			lexerOld.AdvanceOneCharacter();

			trigger.CheckSemanticValidity();
			return trigger;
		}

		private static Map ParseMap( ILexerOld lexerOld )
		{
			var map = new Map();

			TokenTypeOld nextToken;
            while ((nextToken = lexerOld.DetermineNextToken()) != TokenTypeOld.EndOfFile)
            {
				if( nextToken == TokenTypeOld.Identifier )
				{
		            switch( lexerOld.ReadIdentifier().ToString() )
					{
						case "namespace":
							map.Namespace = ParseStringAssignment( lexerOld, "Map.Namespace" );
							break;
						case "tilesize":
							map.TileSize = ParseIntegerNumberAssignment( lexerOld, "Map.TileSize" );
							break;
						case "name":
							map.Name = ParseStringAssignment( lexerOld, "Map.Name" );
							break;
						case "width":
							map.Width = ParseIntegerNumberAssignment( lexerOld, "Map.Width" );
							break;
						case "height":
							map.Height = ParseIntegerNumberAssignment( lexerOld, "Map.Height" );
							break;
						case "comment":
							map.Comment = ParseStringAssignment( lexerOld, "Map.Comment" );
							break;
						case "tile":
							map.Tiles.Add(ParseTile(lexerOld));
							break;
						case "sector":
							map.Sectors.Add(ParseSector(lexerOld));
							break;
						case "zone":
							map.Zones.Add(ParseZone(lexerOld));
							break;
						case "plane":
							map.Planes.Add(ParsePlane(lexerOld));
							break;
						case "planemap":
							map.PlaneMaps.Add(ParsePlaneMap(lexerOld));
							break;
						case "thing":
							map.Things.Add(ParseThing(lexerOld));
							break;
						case "trigger":
							map.Triggers.Add(ParseTrigger(lexerOld));
							break;
						default:
							lexerOld.MovePastAssignment();
							break;
					}
				}
				else
				{
					throw new UwmfParsingException($"Unexpected token in Map: {nextToken}");
				}
            }

			map.CheckSemanticValidity();
			return map;
		}

	}
}