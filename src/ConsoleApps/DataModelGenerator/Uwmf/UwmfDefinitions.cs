// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MetadataModel;
using Tiledriver.DataModelGenerator.Uwmf.MetadataModel;

namespace Tiledriver.DataModelGenerator.Uwmf
{
	static class UwmfDefinitions
	{
		public static readonly ImmutableArray<Block> Blocks = ImmutableArray.Create(
			new Block(
				"tile",
				Properties: ImmutableArray.Create<Property>(
					new TextureProperty("textureEast"),
					new TextureProperty("textureNorth"),
					new TextureProperty("textureWest"),
					new TextureProperty("textureSouth"),
					new BooleanProperty("blockingEast", defaultValue: true),
					new BooleanProperty("blockingNorth", defaultValue: true),
					new BooleanProperty("blockingWest", defaultValue: true),
					new BooleanProperty("blockingSouth", defaultValue: true),
					new BooleanProperty("offsetVertical", defaultValue: false),
					new BooleanProperty("offsetHorizontal", defaultValue: false),
					new BooleanProperty("dontOverlay", defaultValue: false),
					new IntegerProperty("mapped", defaultValue: 0),
					new StringProperty("soundSequence", defaultValue: string.Empty),
					new StringProperty("textureOverhead", defaultValue: string.Empty),
					new StringProperty("comment", defaultValue: string.Empty)
				)
			),
			new Block(
				"sector",
				Properties: ImmutableArray.Create<Property>(
					new TextureProperty("textureCeiling"),
					new TextureProperty("textureFloor"),
					new StringProperty("comment", defaultValue: string.Empty)
				)
			),
			new Block(
				"zone",
				Properties: ImmutableArray.Create<Property>(new StringProperty("comment", defaultValue: string.Empty))
			),
			new Block(
				"plane",
				Properties: ImmutableArray.Create<Property>(
					new IntegerProperty("depth"),
					new StringProperty("comment", defaultValue: string.Empty)
				)
			),
			new Block(
				"mapSquare",
				Properties: ImmutableArray.Create<Property>(
					new IntegerProperty("tile"),
					new IntegerProperty("sector"),
					new IntegerProperty("zone"),
					new IntegerProperty("tag", defaultValue: 0)
				),
				Serialization: SerializationType.Custom
			),
			new Block(
				"thing",
				Properties: ImmutableArray.Create<Property>(
					new StringProperty("type"),
					new DoubleProperty("x"),
					new DoubleProperty("y"),
					new DoubleProperty("z"),
					new IntegerProperty("angle"),
					new BooleanProperty("ambush", defaultValue: false),
					new BooleanProperty("patrol", defaultValue: false),
					new BooleanProperty("skill1", defaultValue: false),
					new BooleanProperty("skill2", defaultValue: false),
					new BooleanProperty("skill3", defaultValue: false),
					new BooleanProperty("skill4", defaultValue: false),
					new StringProperty("comment", defaultValue: string.Empty)
				)
			),
			new Block(
				"trigger",
				Properties: ImmutableArray.Create<Property>(
					new IntegerProperty("x"),
					new IntegerProperty("y"),
					new IntegerProperty("z"),
					new StringProperty("action"),
					new IntegerProperty("arg0", defaultValue: 0),
					new IntegerProperty("arg1", defaultValue: 0),
					new IntegerProperty("arg2", defaultValue: 0),
					new IntegerProperty("arg3", defaultValue: 0),
					new IntegerProperty("arg4", defaultValue: 0),
					new BooleanProperty("activateEast", defaultValue: true),
					new BooleanProperty("activateNorth", defaultValue: true),
					new BooleanProperty("activateWest", defaultValue: true),
					new BooleanProperty("activateSouth", defaultValue: true),
					new BooleanProperty("playerCross", defaultValue: false),
					new BooleanProperty("playerUse", defaultValue: false),
					new BooleanProperty("monsterUse", defaultValue: false),
					new BooleanProperty("repeatable", defaultValue: false),
					new BooleanProperty("secret", defaultValue: false),
					new StringProperty("comment", defaultValue: string.Empty)
				)
			),
			new Block(
				"mapData",
				Properties: ImmutableArray.Create<Property>(
					new StringProperty("nameSpace", formatName: "namespace"),
					new IntegerProperty("tileSize"),
					new StringProperty("name"),
					new IntegerProperty("width"),
					new IntegerProperty("height"),
					new StringProperty("comment", defaultValue: string.Empty),
					new BlockListProperty("tile"),
					new BlockListProperty("sector"),
					new BlockListProperty("zone"),
					new BlockListProperty("plane"),
					new PlaneMapsProperty(),
					new BlockListProperty("thing"),
					new BlockListProperty("trigger")
				),
				Serialization: SerializationType.TopLevel
			)
		);
	}
}
