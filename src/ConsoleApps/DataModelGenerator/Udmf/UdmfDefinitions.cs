// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.Udmf;

static class UdmfDefinitions
{
	public static readonly ImmutableArray<Block> Blocks = ImmutableArray.Create(
		new Block(
			FormatName: "linedef",
			ClassName: "LineDef",
			Properties: ImmutableArray.Create<Property>(
				new IntegerProperty("id", defaultValue: -1),
				new IntegerProperty("v1"),
				new IntegerProperty("v2"),
				new BooleanProperty("blocking", defaultValue: false),
				new BooleanProperty("blockMonsters", defaultValue: false),
				new BooleanProperty("twoSided", defaultValue: false),
				new BooleanProperty("dontPegTop", defaultValue: false),
				new BooleanProperty("dontPegBottom", defaultValue: false),
				new BooleanProperty("secret", defaultValue: false),
				new BooleanProperty("blockSound", defaultValue: false),
				new BooleanProperty("dontDraw", defaultValue: false),
				new BooleanProperty("mapped", defaultValue: false),
				new IntegerProperty("special", defaultValue: 0),
				new IntegerProperty("arg0", defaultValue: 0),
				new IntegerProperty("arg1", defaultValue: 0),
				new IntegerProperty("arg2", defaultValue: 0),
				new IntegerProperty("arg3", defaultValue: 0),
				new IntegerProperty("arg4", defaultValue: 0),
				new IntegerProperty("sideFront"),
				new IntegerProperty("sideBack", defaultValue: -1),
				new StringProperty("comment", defaultValue: string.Empty)
			)
		),
		new Block(
			FormatName: "sidedef",
			ClassName: "SideDef",
			Properties: ImmutableArray.Create<Property>(
				new IntegerProperty("offsetX", defaultValue: 0),
				new IntegerProperty("offsetY", defaultValue: 0),
				new TextureProperty("textureTop", optional: true),
				new TextureProperty("textureBottom", optional: true),
				new TextureProperty("textureMiddle", optional: true),
				new IntegerProperty("sector"),
				new StringProperty("comment", defaultValue: string.Empty)
			)
		),
		new Block(
			FormatName: "vertex",
			Properties: ImmutableArray.Create<Property>(
				new DoubleProperty("x"),
				new DoubleProperty("y"),
				new StringProperty("comment", defaultValue: string.Empty)
			)
		),
		new Block(
			FormatName: "sector",
			Properties: ImmutableArray.Create<Property>(
				new IntegerProperty("heightFloor", defaultValue: 0),
				new IntegerProperty("heightCeiling", defaultValue: 0),
				new TextureProperty("textureFloor"),
				new TextureProperty("textureCeiling"),
				new IntegerProperty("lightLevel", defaultValue: 160),
				new IntegerProperty("special", defaultValue: 0),
				new IntegerProperty("id", defaultValue: 0),
				new StringProperty("comment", defaultValue: string.Empty)
			)
		),
		new Block(
			FormatName: "thing",
			Properties: ImmutableArray.Create<Property>(
				new IntegerProperty("id", defaultValue: 0),
				new DoubleProperty("x"),
				new DoubleProperty("y"),
				new DoubleProperty("height", defaultValue: 0),
				new IntegerProperty("angle", defaultValue: 0),
				new IntegerProperty("type"),
				new BooleanProperty("skill1", defaultValue: false),
				new BooleanProperty("skill2", defaultValue: false),
				new BooleanProperty("skill3", defaultValue: false),
				new BooleanProperty("skill4", defaultValue: false),
				new BooleanProperty("skill5", defaultValue: false),
				new BooleanProperty("ambush", defaultValue: false),
				new BooleanProperty("single", defaultValue: false),
				new BooleanProperty("dm", defaultValue: false),
				new BooleanProperty("coop", defaultValue: false),
				new StringProperty("comment", defaultValue: string.Empty)
			)
		),
		new Block(
			"mapData",
			Properties: ImmutableArray.Create<Property>(
				new StringProperty("nameSpace", formatName: "namespace"),
				new StringProperty("comment", defaultValue: string.Empty),
				new BlockListProperty("lineDef"),
				new BlockListProperty("sideDef"),
				new BlockListProperty("vertices", elementType: "vertex"),
				new BlockListProperty("sector"),
				new BlockListProperty("thing")
			),
			Serialization: SerializationType.TopLevel
		)
	);
}
